using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestApplication.Models
{
    public class Person
    {
        public int id { get; set; }

        public string name { get; set; }

        public string gender { get; set; }

        public string father_id { get; set; }

        public string mother_id { get; set; }

        public int place_id { get; set; }

        public int level { get; set; }
    }
}