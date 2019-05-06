using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
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
        /// <summary>
        /// 删除城市
        /// </summary>
        /// <param name="countryId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 更新城市
        /// </summary>
        /// <param name="countryId"></param>
        /// <param name="cityId"></param>
        /// <param name="cityUpdate"></param>
        /// <returns></returns>
        [HttpPut("{cityId}")]
        public async Task<IActionResult> UpdateCityForCountry(int countryId, int cityId, [FromBody]CityUpdateVM cityUpdate)
        {
            if (cityUpdate == null)
            {
                return BadRequest();
            }
            if (!await _countryRepository.CountryExistsAsync(countryId))
            {
                return NotFound();
            }
            var city = await _cityRepository.GetCityForCountryAsync(countryId, cityId);
            if (city == null)
            {
                return NotFound();
            }
            // 把 cityUpdate 的属性都映射给 city
            _mapper.Map(cityUpdate, city);
            _cityRepository.UpdateCityForCountry(city);
            if (!await _unitOfWork.SaveAsync())
            {
                return StatusCode(500, $"更新 country:{countryId} 下的 city:{cityId}失败");
            }
            return NoContent();
        }

        [HttpPatch("{cityId}")]
        public async Task<IActionResult> PatchUpdateCityForCountry(int countryId, int cityId, [FromBody]JsonPatchDocument<CityUpdateVM> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }
            if (!await _countryRepository.CountryExistsAsync(countryId))
            {
                return NotFound();
            }
            var city = await _cityRepository.GetCityForCountryAsync(countryId, cityId);
            if (city == null)
            {
                return NotFound();
            }
            /*
             * Path 请求的body是一个操作数组
             * [
             *   {
             *      "op":"replace",
             *      "path":"/name",
             *      "value":"new name"
             *   },
             *   {
             *      "op":"remove",
             *      "path":"/name",             
             *   }
             * ]
             * 
             * 
             */

            //第一个是“replace”操作(op的值就是操作的类型)，path代表着资源的属性名，value表示的是更新后的值。
            //第二个操作类型是“remove”，表示要删除资源的某个属性的值，例子里是name属性。

            //JSON PATCH的操作类型主要有六种：
            //添加：{“op”: "add", "path": "/xxx", "value": "xxx"}，如果该属性不存，那么就添加该属性，如果属性存在，就改变属性的值。这个对静态类型不适用。
            //删除：{“op”: "remove", "path": "/xxx"}，删除某个属性，或把它设为默认值（例如空值）。
            //替换：{“op”: "replace", "path": "/xxx", "value": "xxx"}，改变属性的值，也可以理解为先执行了删除，然后进行添加。
            //复制：{“op”: "copy", "from": "/xxx", "path": "/yyy"}，把某个属性的值赋给目标属性。
            //移动：{“op”: "move", "from": "/xxx", "path": "/yyy"}，把源属性的值赋值给目标属性，并把源属性删除或设成默认值。
            //测试：{“op”: "test", "path": "/xxx", "value": "xxx"}，测试目标属性的值和指定的值是一样的。


            // 1. 先将数据库中的原始数据映射为临时的 CityUpdateVM 对象 
            var cityToPatch = _mapper.Map<CityUpdateVM>(city);
            // 2. 调用 JsonPatchDocument 的 ApplyTo 方法，局部更新 cityToPatch
            patchDoc.ApplyTo(cityToPatch);
            // 3. 再将局部更新后的 cityToPath 对象映射回去 city 对象
            _mapper.Map(cityToPatch, city);
            // 4. 调用仓储的更新方法
            _cityRepository.UpdateCityForCountry(city);

            if (!await _unitOfWork.SaveAsync())
            {
                return StatusCode(500, $"批量更新 country:{countryId} 下的 city:{cityId}失败");
            }
            return NoContent();
        }
    }


}