using Microsoft.EntityFrameworkCore;
using MyRestful.Core;
using MyRestful.Core.Entity;
using MyRestful.Core.Interface;
using System;
using System.Collections.Generic;
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

        public async Task<IEnumerable<Country>> GetCountriesAsync()
        {
            return await _myContext.Countries.ToListAsync();
        }
    }
}
