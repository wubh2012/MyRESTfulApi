using MyRestful.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyRestful.Core.Interface
{
    public interface ICityRepository
    {
        Task<City> GetCityForCountryAsync(int countryId, int cityId);
        Task<List<City>> GetCityForCountryAsync(int countryId);
        void AddCity(int countryId, City cityModel);
        void DeleteCity(City city);
        void UpdateCityForCountry(City city);
    }
}
