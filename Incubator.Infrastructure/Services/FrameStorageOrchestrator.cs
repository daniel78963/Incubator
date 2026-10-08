using Incubator.Application.Interfaces;
using Incubator.Domain.Entities;
using Incubator.Domain.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace Incubator.Infrastructure.Services
{
    public class FrameStorageOrchestrator : IFrameStorageService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IAppConfigService _configService;
        private readonly IEncryptionService _encryptionService;

        public FrameStorageOrchestrator(IServiceProvider serviceProvider, 
            IAppConfigService configService,
            IEncryptionService encryptionService)
        {
            _serviceProvider = serviceProvider;
            _configService = configService;
            _encryptionService = encryptionService;
        }

        //public async Task SaveFrameAsync(IncubatorFrame frame)
        //{
        //    var config = _configService.GetConfig();

        //    if (config.StorageType == StorageMethod.Database)
        //    {
        //        var dbService = _serviceProvider.GetRequiredService<DbFrameStorageService>();
        //        await dbService.SaveFrameAsync(frame);
        //    }
        //    else
        //    {
        //        var fileService = _serviceProvider.GetRequiredService<FileFrameStorageService>();
        //        await fileService.SaveFrameAsync(frame);
        //    }
        //}

        public async Task SaveFrameAsync(IncubatorFrame frame)
        {
            var config = _configService.GetConfig();

            // Si el usuario exige almacenamiento seguro, encriptamos la data sensible antes de guardar
            if (config.StoreEncrypted)
            {
                // Puedes encriptar propiedades individuales o la trama cruda
                frame.RawFrame = _encryptionService.Encrypt(frame.RawFrame);

                if (!string.IsNullOrEmpty(frame.QrData))
                {
                    frame.QrData = _encryptionService.Encrypt(frame.QrData);
                }
            }

            if (config.StorageType == StorageMethod.Database)
            {
                var dbService = _serviceProvider.GetRequiredService<DbFrameStorageService>();
                await dbService.SaveFrameAsync(frame);
            }
            else
            {
                var fileService = _serviceProvider.GetRequiredService<FileFrameStorageService>();
                await fileService.SaveFrameAsync(frame);
            }
        }
    }
}
