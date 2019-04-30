using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyRESTfulApi.Models;
using MyRESTfulApi.ViewModel;

namespace MyRESTfulApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly MyContext _context;
        private readonly IMapper _mapper;
        public CountryController(MyContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var countries = await _context.Countries.ToListAsync();
            var countryVMs = _mapper.Map<List<CountryVM>>(countries);
            return Ok(countryVMs);
        }
    }
}