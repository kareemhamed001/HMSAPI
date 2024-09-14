using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int FloorId { get; set; }
        public Floor Floor { get; set; }
        public int RoomsTypesTd { get; set; }
        public RoomType RoomType { get; set; }
        public Pharmacy Pharmacy { get; set; }
        public Warehouse warehouse { get; set; }

    }
}
