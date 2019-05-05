using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyRestful.Api.ViewModel;
using MyRestful.Core.Interface;

namespace MyRestful.Api.Controller
{
    [Route("api/country/{countryId}/city")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        private ICountryRepository _countryRepository;
        private ICityRepository _cityRepository;
        private IMapper _mapper;

        public CityController(IUnitOfWork unitOfWork, ICountryRepository countryRepository, ICityRepository cityRepository, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _countryRepository = countryRepository;
            _cityRepository = cityRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetCityForCountry(int countryId)
        {
            if (!await _countryRepository.CountryExistsAsync(countryId))
            {
                return NotFound();
            }
            var cities = await _cityRepository.GetCityForCountryAsync(countryId);
            var cityVMList = _mapper.Map<IEnumerable<CityVM>>(cities);
            return Ok(cityVMList);
        }

        [HttpGet("{cityId}")]
        public async Task<IActionResult> GetCityForCountry(int countryId, int cityId)
        {
            if (!await _countryRepository.CountryExistsAsync(countryId))
            {
                return NotFound();
            }
            var cityForCountry = await _cityRepository.GetCityForCountryAsync(countryId, cityId);
            if (cityForCountry == null)
            {
                return NotFound();
            }
            var cityVm = _mapper.Map<CityVM>(cityForCountry);
            return Ok(cityVm);
        }
    }


}