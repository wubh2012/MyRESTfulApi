using Microsoft.EntityFrameworkCore;
using MyRestful.Core;
using MyRestful.Core.Entity;
using MyRestful.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRestful.Infrastructure.Repository
{
    public class CountryRepository : ICountryRepository
    {
        private readonly MyContext _myContext;
        public CountryRepository(MyContext myContext)
        {
            _myContext = myContext;
        }

        public void AddCountry(Country newCountry)
        {
            _myContext.Countries.Add(newCountry);
        }

        public async Task<bool> CountryExistsAsync(int countryId)
        {
            return await _myContext.Countries.AnyAsync(m => m.Id == countryId);
        }

        public void DeleteCountry(Country country)
        {
            _myContext.Countries.Remove(country);
        }

        public async Task<IEnumerable<Country>> GetCountriesAsync()
        {
            return await _myContext.Countries.ToListAsync();
        }

        public async Task<IEnumerable<Country>> GetCountriesAsync(IEnumerable<int> ids)
        {
            return await _myContext.Countries.Where(x => ids.Contains(x.Id)).ToListAsync();
        }

        public async Task<Country> GetCountryByIdAsync(int id)
        {
            return await _myContext.Countries.FindAsync(id);
        }
    }
}
