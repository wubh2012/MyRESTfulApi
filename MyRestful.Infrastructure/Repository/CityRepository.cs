﻿using Microsoft.EntityFrameworkCore;
using MyRestful.Core.Entity;
using MyRestful.Core.Interface;
using MyRestful.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyRestful.Api
{
    public class CityRepository : ICityRepository
    {
        private MyContext _myContext;

        public CityRepository(MyContext myContext)
        {
            _myContext = myContext;
        }
        public async Task<List<City>> GetCityForCountryAsync(int countryId)
        {
            return await _myContext.Cities.Where(m => m.CountryId == countryId).ToListAsync();
        }

        public async Task<City> GetCityForCountryAsync(int countryId, int cityId)
        {
            return await _myContext.Cities.SingleOrDefaultAsync(m => m.CountryId == countryId && m.Id == cityId);
        }
    }
}