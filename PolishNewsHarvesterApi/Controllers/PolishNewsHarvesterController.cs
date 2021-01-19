using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PolishNewsHarvesterApi.Controllers
{
    [ApiController]
    [Route("api/status")]
    public class PolishNewsHarvesterController : ControllerBase
    {

        private readonly ILogger<PolishNewsHarvesterController> _logger;

        public PolishNewsHarvesterController(ILogger<PolishNewsHarvesterController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public string Get()
        {
            return "ok";
        }
    }
}