using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyRestful.Api.Config
{
    public class FirstConfig
    {
        public string key1 { get; set; }
        public string key2 { get; set; }
        public childConfig key3 { get; set; }
    }
    public class childConfig
    {
        public string childkey1 { get; set; }
    }
}
