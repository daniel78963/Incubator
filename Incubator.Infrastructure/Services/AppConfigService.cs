using Incubator.Application.Interfaces;
using Incubator.Domain.Entities;
using System.Text.Json;

namespace Incubator.Infrastructure.Services
{
    public class AppConfigService : IAppConfigService
    {
        private readonly string _configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "user_settings.json");

        public AppConfiguration GetConfig()
        {
            if (File.Exists(_configPath))
            {
                string json = File.ReadAllText(_configPath);
                return JsonSerializer.Deserialize<AppConfiguration>(json) ?? new AppConfiguration();
            }
            return new AppConfiguration(); // Devuelve los valores por defecto (FlatFile, yyyyMMdd)
        }

        public void SaveConfig(AppConfiguration config)
        {
            string json = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_configPath, json);
        }
    }
}
