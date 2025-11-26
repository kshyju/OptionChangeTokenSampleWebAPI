using Microsoft.Extensions.Options;
using OptionChangeTokenDemo;

namespace WebApplication9
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            Environment.SetEnvironmentVariable("SCRIPT_ROOT", "D:\\temp\\config_test3");
            // Add custom JSON config source (if file exists, it will load into IConfiguration)
            ((IConfigurationBuilder)builder.Configuration).Add(new CustomJsonConfigurationSource());

            builder.Configuration.AddEnvironmentVariables();

            builder.Services.AddSingleton<IConfigureOptions<MyConfig>, ConfigureMyConfig>();

            // Ensure options monitor rebinds when IConfiguration changes
            builder.Services.AddSingleton<IOptionsChangeTokenSource<MyConfig>>(sp =>
                new ConfigurationChangeTokenSource<MyConfig>(sp.GetRequiredService<IConfiguration>()));

            builder.Services.AddControllers();
            builder.Services.AddHostedService<MyBackgroundService>();
            var app = builder.Build();
            app.MapControllers();
            app.Run();
        }
    }
}
