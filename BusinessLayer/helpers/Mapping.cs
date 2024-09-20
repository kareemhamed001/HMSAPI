using AutoMapper;
using BusinessLayer.Responses;
using DataAccessLayer.Entities;
using SharedClasses.Responses;
namespace BusinessLayer.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            //user mapping
            CreateMap<UserRequest, User>();
            CreateMap<User, UserResponse>();

            //role mapping
            CreateMap<Role, RoleResponse>();
            CreateMap<CreateRoleRequest, Role>();

            //permission mapping
            CreateMap<Permission, PermissionResponse>();

            //building mapping
            CreateMap<Building, BuildingResponse>();
            CreateMap<BuildingResponse, Building>();
            CreateMap<BuildingRequest, Building>();

            //floor mapping
            CreateMap<Floor, FloorResponse>();
            CreateMap<FloorResponse, Floor>();
            CreateMap<FloorRequest, Floor>();

            //roomType mapping
            CreateMap<RoomType, RoomTypeResponse>();
            CreateMap<RoomTypeResponse, RoomType>();
            CreateMap<RoomTypeRequest, RoomType>();


        }

    }
}