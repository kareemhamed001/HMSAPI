using AutoMapper;
using HMS.Api.App.Options;
using HMS.API.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


namespace LMSApi.App.Controllers.Auth
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private readonly ILogger<AuthController> _logger;
        private readonly JwtOptions _jwtOptions;
        private readonly IMapper _mapper;
        private readonly PasswordHasher<User> _passwordHasher = new PasswordHasher<User>();

        public AuthController(AppDbContext appDbContext, ILogger<AuthController> logger, JwtOptions jwtOptions, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _logger = logger;
            _jwtOptions = jwtOptions;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<ApiResponse<string>>> Login(LoginRequest request)
        {
            var user = await _appDbContext.Users.Include(u => u.Roles)
                .ThenInclude(r => r.Permissions)
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null)
                return NotFound(ApiResponseFactory.Create("User not found", 404, false));

            if (!VerifyPassword(user.Password, request.Password))
                return BadRequest(ApiResponseFactory.Create("Invalid password", 400, false));

            return Ok(ApiResponseFactory.Create((object)GenerateJwtToken(user, _jwtOptions), "Succeeded", 201, true));
        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult<ApiResponse<string>>> Register(UserRequest request)
        {
            // Check if user already exists
            var existingUser = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (existingUser != null)
                return BadRequest(ApiResponseFactory.Create("User already exists", 400, false));

            var userRole = await _appDbContext.Roles.Include(u => u.Permissions).SingleOrDefaultAsync(r => r.Name == "User");
            if (userRole == null)
            {
                userRole = new Role { Name = "User" };
                await _appDbContext.Roles.AddAsync(userRole);
                await _appDbContext.SaveChangesAsync(); 
            }

            User user = _mapper.Map<User>(request);
            user.Roles = new List<Role> { userRole };

            user.Password = _passwordHasher.HashPassword(user, request.Password);

            await _appDbContext.Users.AddAsync(user);
            await _appDbContext.SaveChangesAsync();

            Dictionary<string, object> data = new Dictionary<string, object>
            {
                { "Token", GenerateJwtToken(user, _jwtOptions) },
                { "Permission", userRole.Permissions.Select(p => p.RouteName).ToList() }
            };

            return Ok(ApiResponseFactory.Create(
                data.Select(kvp => new { key = kvp.Key, value = kvp.Value }).ToList(),
                "User registered successfully",
                201,
                true));
        }

        [HttpGet]
        [Route("{userId}")]
        [Authorize]
        [CheckPermission("users.show")]
        public async Task<ActionResult<ApiResponse<UserResponse>>> GetUserById(int userId)
        {
            var user = await _appDbContext.Users
                .Include(u => u.Roles)
                .ThenInclude(r => r.Permissions)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return NotFound(ApiResponseFactory.Create("User not found", 404, false));

            var userDto = _mapper.Map<UserResponse>(user);

            return Ok(userDto);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        private string GenerateJwtToken(User user, JwtOptions jwtOptions)
        {
            var userPermissions = user.Roles.SelectMany(r => r.Permissions).Select(p => p.RouteName).ToList();

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name)
            };

            foreach (var permission in userPermissions)
            {
                claims.Add(new Claim("permissions", permission));
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = jwtOptions.Issuer,
                Audience = jwtOptions.Audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key)), SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(claims)
            };

            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(securityToken);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        private bool VerifyPassword(string hashedPassword, string providedPassword)
        {
            var result = _passwordHasher.VerifyHashedPassword(null, hashedPassword, providedPassword);
            return result == PasswordVerificationResult.Success;
        }
    }
}
