using System.Collections.Generic;

namespace MyRestful.Api.ViewModel
{
    public class CountryAddVM
    {
        public string EnglishName { get; set; }
        public string ChineseName { get; set; }
        /// <summary>
        /// 缩写
        /// </summary>
        public string Abbreviation { get; set; }
        public List<CityAddVM> Cities { get; set; }
    }
}