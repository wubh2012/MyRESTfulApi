using System.Collections.Generic;
using System.Threading.Tasks;
using MyRestful.Core;
using MyRestful.Core.Entity;

namespace MyRestful.Core.Interface
{
    public interface ICountryRepository
    {
        Task<IEnumerable<Country>> GetCountriesAsync();
        void AddCountry(Country newCountry);
    }
}