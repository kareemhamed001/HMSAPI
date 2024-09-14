using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
    public class Staff
    {
        public int Id { get; set; }
        public string? Position { get; set; }
        public int Experience { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
