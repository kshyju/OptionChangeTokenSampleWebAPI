using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace WebApplication9
{
    public class CustomJsonConfigurationSource : IConfigurationSource
    {
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new CustomJsonConfigurationProvider();
        }
    }

    public class CustomJsonConfigurationProvider : ConfigurationProvider
    {
        private static CustomJsonConfigurationProvider? _instance;

        public static void TriggerReload()
        {
            _instance?.ReloadInternal();
        }

        private void ReloadInternal()
        {
            Load();
            OnReload(); // notify configuration root only about this provider's changes
        }

        public CustomJsonConfigurationProvider()
        {
            _instance = this; // capture singleton instance
        }
        public override void Load()
        {
            var filePath = Environment.GetEnvironmentVariable("SCRIPT_ROOT");

            if (string.IsNullOrWhiteSpace(filePath))
            {
                Console.WriteLine("SCRIPT_ROOT environment variable is not set.");
                return;
            }

            filePath = Path.Combine(filePath, "hello.json");
            if (!File.Exists(filePath))
            {
                Console.WriteLine("Configuration file not found at: " + filePath);
                return;

            }
            var json = File.ReadAllText(filePath);
            if (string.IsNullOrWhiteSpace(json))
            {
                return;
            }

            var data = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
            if (data != null)
            {
                Data = new Dictionary<string, string?>(data!, StringComparer.OrdinalIgnoreCase);
            }
        }
    }
}

