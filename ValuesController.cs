using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace WebApplication9
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private CustomOptionsChangeTokenSource _changeTokenSource;
        private readonly ILogger<ValuesController> logger;
        private IOptionsMonitor<MyAppSettings> _optionsMonitor;
        public ValuesController(ILogger<ValuesController> logger, IOptionsMonitor<MyAppSettings> optionsMonitor, CustomOptionsChangeTokenSource changeTokenSource)
        {
            this.logger = logger;
            _optionsMonitor = optionsMonitor;
            _changeTokenSource = changeTokenSource;

            // Add a change listener to verify that changes are detected
            _optionsMonitor.OnChange(options =>
            {
                Console.WriteLine("Options changed: " + options.PopulatedTime);
            });
        }


        [HttpGet("{id}")]
        public string Get(int id)
        {
            var options = _optionsMonitor.CurrentValue;

            logger.LogInformation("Get value {id}", id);
            if (id == 10)
            {
                // Refresh the MyAppSettings options.
                _changeTokenSource.TriggerChange();
                options = _optionsMonitor.CurrentValue;
            }

            return "value " + id + " PopulatedTime:" + options.PopulatedTime;
        }
    }

    public class CustomOptionsChangeTokenSource : IOptionsChangeTokenSource<MyAppSettings>
    {
        private CancellationTokenSource _cts = new CancellationTokenSource();
        public string Name => "Foo";
        public IChangeToken GetChangeToken()
        {
            var currentCts = _cts;
            return new CancellationChangeToken(currentCts.Token);
        }

        public void TriggerChange()
        {
            var newCts = new CancellationTokenSource();
            var previousCts = Interlocked.Exchange(ref _cts, newCts);
            previousCts.Cancel();
        }
    }

    public class ConfigureOptionsMyAppSettings : IConfigureOptions<MyAppSettings>
    {
        public void Configure(MyAppSettings options)
        {
            options.PopulatedTime = DateTime.UtcNow;
            Console.WriteLine("Options configured at: " + options.PopulatedTime); 
        }
    }
    public sealed class MyAppSettings
    {
        public DateTime PopulatedTime { get; set; }
    }
}
