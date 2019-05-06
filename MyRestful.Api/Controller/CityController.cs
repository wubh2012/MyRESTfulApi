using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyRestful.Api.ViewModel;
using MyRestful.Core.Entity;
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
        /// <summary>
        /// 获取国家所有的城市
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 获取国家中的单个城市
        /// </summary>
        /// <param name="countryId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        [HttpGet("{cityId}")]
        public async Task<IActionResult> GetCity(int countryId, int cityId)
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

        /// <summary>
        /// 创建国家中的城市
        /// </summary>
        /// <param name="countryId"></param>
        /// <param name="city"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateCityForCountry(int countryId, [FromBody]CityAddVM city)
        {
            if (city == null)
            {
                return BadRequest();
            }

            if (!await _countryRepository.CountryExistsAsync(countryId))
            {
                return NotFound();
            }
            var cityModel = _mapper.Map<City>(city);
            _cityRepository.AddCity(countryId, cityModel);
            if (!await _unitOfWork.SaveAsync())
            {
                return StatusCode(500, "添加数据失败");
            }
            var cityVM = _mapper.Map<CityVM>(cityModel);
            return CreatedAtAction(nameof(GetCity), new { countryId, cityId = cityVM.Id }, cityVM);
        }

        [HttpDelete("{cityId}")]
        public async Task<IActionResult> DeleteCityForCountry(int countryId, int cityId)
        {
            if (!await _countryRepository.CountryExistsAsync(countryId))
            {
                return NotFound();
            }
            var city = await _cityRepository.GetCityForCountryAsync(countryId, cityId);
            if (city == null)
            {
                return NotFound();
            }
            _cityRepository.DeleteCity(city);
            if (!await _unitOfWork.SaveAsync())
            {
                return StatusCode(500, $"删除 country:{countryId} 下的 city:{cityId}失败");
            }
            return NoContent();

        }
    }


}