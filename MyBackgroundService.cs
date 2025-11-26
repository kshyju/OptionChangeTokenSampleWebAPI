
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;

namespace OptionChangeTokenDemo
{
    internal sealed class MyBackgroundService : BackgroundService
    {
        ILogger<MyBackgroundService> _logger;
        IOptionsMonitor<MyConfig> _optionsMonitor;
        IConfiguration _configuration;
        public MyBackgroundService(ILogger<MyBackgroundService> logger, IOptionsMonitor<MyConfig> optionsMonitor, IConfiguration configuration)
        {   
            this._configuration = configuration;
            this._logger = logger;
            this._optionsMonitor = optionsMonitor;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var thatEnvVar = _configuration["SCRIPT_ROOT"];

                var data = _optionsMonitor.CurrentValue.Name;
                _logger.LogWarning(".... {time} --- NAME: {name} . SCRIPT_ROOT:{env} ", DateTimeOffset.Now, data, thatEnvVar);
                await Task.Delay(TimeSpan.FromSeconds(2), stoppingToken);
            }

            return;

        }
    }
}
