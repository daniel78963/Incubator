using Incubator.Application.Interfaces;
using Incubator.Domain.Entities;

namespace Incubator.Infrastructure.Services
{
    public class FileFrameStorageService
    {
        private readonly IAppConfigService _configService;
        private const string BaseDirectory = @"C:\SmartIncubator\Plots";

        public FileFrameStorageService(IAppConfigService configService)
        {
            _configService = configService;
        }

        public async Task SaveFrameAsync(IncubatorFrame frame)
        {
            var config = _configService.GetConfig();

            // Aseguramos que la carpeta exista
            if (!Directory.Exists(BaseDirectory))
            {
                Directory.CreateDirectory(BaseDirectory);
            }

            // Aplicamos el formato configurable para el nombre del archivo
            string fileName = $"{DateTime.Now.ToString(config.FlatFileFormat)}.txt";
            string filePath = Path.Combine(BaseDirectory, fileName);

            // Formato de línea en el archivo
            string line = $"[{DateTime.Now:HH:mm:ss}] SERIAL:{frame.SerialNumber} | RESULT:{frame.Result} | POS:{frame.Position} | RAW:{frame.RawFrame.Trim()}";

            await File.AppendAllTextAsync(filePath, line + Environment.NewLine);
        }
    }
}
