using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace WebApplication9
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ILogger<ValuesController> logger;
        private readonly IConfiguration configuration;
        public ValuesController(ILogger<ValuesController> logger, IConfiguration configuration)
        {
            this.logger = logger;
            this.configuration = configuration;
        }

        [HttpGet("{id}")]
        public string Get(int id)
        {
            logger.LogInformation("Get value {id}", id);
            if (id == 10)
            {
                Environment.SetEnvironmentVariable("SCRIPT_ROOT", "D:\\temp\\config_test2");
                //// Reload only the custom JSON configuration provider
                CustomJsonConfigurationProvider.TriggerReload();
                logger.LogInformation("Custom JSON configuration provider reloaded after SCRIPT_ROOT change.");

            }

            return "value " + id;
        }
    }
}
