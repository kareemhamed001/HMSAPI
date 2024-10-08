﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
    public class Section
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Clinic> Clinics { get; set; } = new List<Clinic>();
        public List<Warehouse> Warehouses { get; set; } = new List<Warehouse>();

    }
}
