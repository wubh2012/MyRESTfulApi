using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyRESTfulApi.Models;

namespace MyRESTfulApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly MyContext _context;
        public CountryController(MyContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Country> Get()
        {
            var list = _context.Countries.ToList();
            return list;
        }
    }
}