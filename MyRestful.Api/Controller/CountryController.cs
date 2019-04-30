using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyRestful.Api.Models;
using MyRestful.Api.ViewModel;
using MyRestful.Core;
using MyRestful.Core.Entity;
using MyRestful.Core.Interface;
using MyRestful.Infrastructure;
using MyRestful.Infrastructure.Repository;

namespace MyRestful.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CountryController(IUnitOfWork unitOfWork, ICountryRepository repository, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _countryRepository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            // throw new Exception("test");
            var newCountry = new Country()
            {
                ChineseName = "俄罗斯",
                EnglishName = "Russia",
                Abbreviation = "Russia"
            };
            _countryRepository.AddCountry(newCountry);
            await _unitOfWork.SaveAsync();

            var countries = await _countryRepository.GetCountriesAsync();
            var countryVMs = _mapper.Map<List<CountryVM>>(countries);
            return Ok(countryVMs);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCountry(int id)
        {
            
            var country = await _countryRepository.GetCountryByIdAsync(id);
            if (country == null)
            {
                return NotFound();
            }
            var countryVM = _mapper.Map<CountryVM>(country);
            return Ok(country);
        }

        [HttpPost]
        public async Task<IActionResult> Add()
        {
            return Ok();
        }
    }
}