using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using MyRestful.Api.Helper;
using MyRestful.Api.ViewModel;
using MyRestful.Core.Entity;
using MyRestful.Core.Interface;

namespace MyRestful.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryCollectionController : ControllerBase
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CountryCollectionController(IUnitOfWork unitOfWork, ICountryRepository repository, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _countryRepository = repository;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCountryCollection([FromBody]IEnumerable<CountryAddVM> countries)
        {
            if (countries == null)
            {
                return BadRequest();
            }

            var countriesModel = _mapper.Map<IEnumerable<Country>>(countries);
            foreach (var country in countriesModel)
            {
                _countryRepository.AddCountry(country);
            }
            if (!await _unitOfWork.SaveAsync())
            {
                return StatusCode(500, "Error occurred when adding");
            }
            var countriesVMs = _mapper.Map<IEnumerable<CountryVM>>(countriesModel);
            var idsStr = string.Join(",", countriesVMs.Select(x => x.Id));
            return CreatedAtRoute(nameof(GetCountryCollection), new { ids = idsStr }, countriesVMs);
        }

        [HttpGet("{ids}", Name = "GetCountryCollection")]
        public async Task<IActionResult> GetCountryCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))]IEnumerable<int> ids)
        {
            if (ids == null)
            {
                return BadRequest();
            }
            var countries = await _countryRepository.GetCountriesAsync(ids);
            if (ids.Count() != countries.Count())
            {
                return NotFound();
            }
            var countryVms = _mapper.Map<IEnumerable<CountryVM>>(countries);
            return Ok(countryVms);
        }
    }
}