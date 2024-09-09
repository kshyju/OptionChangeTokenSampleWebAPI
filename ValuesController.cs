using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace WebApplication9
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly CustomOptionsChangeTokenSource _changeTokenSource;
        private readonly ILogger<ValuesController> logger;
        private readonly IOptionsMonitor<MyAppSettings> _optionsMonitor;
        public ValuesController(ILogger<ValuesController> logger, IOptionsMonitor<MyAppSettings> optionsMonitor, CustomOptionsChangeTokenSource changeTokenSource)
        {
            this.logger = logger;
            _optionsMonitor = optionsMonitor;
            _changeTokenSource = changeTokenSource;
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

    public sealed class CustomOptionsChangeTokenSource : IOptionsChangeTokenSource<MyAppSettings>, IDisposable
    {
        private CancellationTokenSource _cts = new();

        public string Name => Options.DefaultName;

        public IChangeToken GetChangeToken() => new CancellationChangeToken(_cts.Token);

        public void TriggerChange()
        {
            var previousCts = Interlocked.Exchange(ref _cts, new CancellationTokenSource());
            previousCts.Cancel();
            previousCts.Dispose();
        }

        public void Dispose() => _cts.Dispose();
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
