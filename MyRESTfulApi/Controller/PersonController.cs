using Microsoft.AspNetCore.Mvc;
using MyRESTfulApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyRESTfulApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new Person());
        }

        [HttpGet("first/{id}")]
        
        public IActionResult FindPerson(int id, string name)
        {
            return null;
        }
        [HttpPost("~/api/People")]
        public IActionResult Post(Person person)
        {
            return Ok();
        }

        [HttpDelete]
        public IActionResult Remove(int id)
        {
            return Ok();
        }

        [NonAction]
        public IActionResult GetTime()
        {
            return Ok(DateTime.Now);
        }
    }
}
