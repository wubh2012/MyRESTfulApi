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

        [HttpPost]
        public async Task<IActionResult> Add()
        {
            return Ok();
        }
    }
}