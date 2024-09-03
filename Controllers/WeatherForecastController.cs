using Microsoft.AspNetCore.Mvc;

namespace WebApplication9.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public string Get() => "Try /api/values/5 and wait a few seconds and then try /api/values/10.";
    }
}
