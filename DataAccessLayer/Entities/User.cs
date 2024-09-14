using DataAccessLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? Address { get; set; }
        public string? NationalId { get; set; }
        public GendersEnum? Gender { get; set; }
        public string? Image { get; set; }
        public UserTypesEnum Type { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public BloodGroupEnum BloodGroup { get; set; }
        public Nurse? Nurse { get; set; }
        public Doctor? Doctor { get; set; }
        public Staff? Staff { get; set; }
        public Patient? Patient { get; set; }
        public Employee? Employee { get; set; }
        public Pharmacist? Pharmacist { get; set; }
    }
}
