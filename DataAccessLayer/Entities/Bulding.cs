using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
    public class Building
    {   
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string address { get; set; }
        public List<Floor> Floors { get; set; }=new List<Floor>();


    }
}
