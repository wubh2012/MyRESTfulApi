using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyRestful.Core.Entity
{
    public class Country
    {
        public int Id { get; set; }
        public string EnglishName { get; set; }
        public string ChineseName { get; set; }
        /// <summary>
        /// 缩写
        /// </summary>
        public string Abbreviation { get; set; }
    }
}
