using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Requests
{
    public class RoomRequest
    {
        public string Name { get; set; }
        public int FloorId { get; set; }
        public int RoomsTypesTd { get; set; }
    }
}
