using System;
using System.Collections.Generic;
using System.Text;

namespace MyRestful.Core.Entity
{
    public class City
    {
        public int Id { get; set; }
        public int CountryId { get; set; }
        public string Name { get; set; }
        public Country Country { get; set; }
    }
}
