using Microsoft.Extensions.Options;
using OptionChangeTokenDemo;

public sealed class ConfigureMyConfig : IConfigureOptions<MyConfig>
{
    private readonly IConfiguration _cfg;
    public ConfigureMyConfig(IConfiguration cfg) => _cfg = cfg;

    public void Configure(MyConfig options)
    {
        // Bind section or individual keys
        options.Name = _cfg["name"] ?? "default-name";
        options.City = _cfg["city"] ?? "default-city";

        // Custom rule
        if (options.City?.Equals("seattle", StringComparison.OrdinalIgnoreCase) == true)
            options.City = "Seattle, WA";
    }
}