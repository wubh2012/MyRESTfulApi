using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyRESTfulApi.Config;

namespace MyRESTfulApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private ILogger _logger;
        private FirstConfig _firstConfig;
        public TestController(ILogger<TestController> logger, IOptionsSnapshot<FirstConfig> firstConfig)
        {
            _logger = logger;
            _firstConfig = firstConfig.Value;

        }

        [HttpGet]
        public IActionResult Get()
        {
            _logger.LogInformation($"绑定至类 方式二 key1 = {_firstConfig.key1}");
            _logger.LogInformation($"绑定至类 方式二 key2 = {_firstConfig.key2}");
            _logger.LogInformation($"绑定至类 方式二 key3 = {_firstConfig.key3.childkey1}");

            var result = $"{_firstConfig.key1}, key2={_firstConfig.key2}";
            return Ok(result);
        }
    }
}