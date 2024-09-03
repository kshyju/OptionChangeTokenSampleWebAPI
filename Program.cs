using Microsoft.Extensions.Options;

namespace WebApplication9
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddSingleton<CustomOptionsChangeTokenSource>(); 
            builder.Services.AddSingleton<IConfigureOptions<MyAppSettings>, ConfigureOptionsMyAppSettings>();
            builder.Services.AddSingleton<IOptionsChangeTokenSource<MyAppSettings>, CustomOptionsChangeTokenSource>();

            builder.Services.AddControllers();
            var app = builder.Build();
            app.MapControllers();
            app.Run();
        }
    }
}
