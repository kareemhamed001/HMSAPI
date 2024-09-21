using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.Responses
{
    public class WarehouseResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int RoomId { get; set; }
        public int SectionId { get; set; }
    }
}
