using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
    public  class RoomType
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public List<Room> Rooms { get; set; }
    }
}
