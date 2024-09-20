using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Requests
{
    public class FloorRequest
    {
        public string Name { get; set; }
        public int BuildingId { get; set; }
    }
}
