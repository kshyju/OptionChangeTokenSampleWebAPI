using Microsoft.Extensions.Options;
using OptionChangeTokenDemo;

namespace WebApplication9
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            Environment.SetEnvironmentVariable("SCRIPT_ROOT", "D:\\temp\\config_test");
            // Add custom JSON config source (if file exists, it will load into IConfiguration)
            ((IConfigurationBuilder)builder.Configuration).Add(new CustomJsonConfigurationSource());

            // Add environment variables to IConfiguration
            builder.Configuration.AddEnvironmentVariables();

            // Bind to MyConfig POCO Options.
            builder.Services.Configure<MyConfig>(builder.Configuration);

            // set minimum logging level to Information
            builder.Logging.SetMinimumLevel(LogLevel.Information);

            builder.Services.AddControllers();
            builder.Services.AddHostedService<MyBackgroundService>();
            var app = builder.Build();
            app.MapControllers();
            app.Run();
        }
    }
}
