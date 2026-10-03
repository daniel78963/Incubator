using Incubator.Application.Interfaces;
using Incubator.Domain.Entities;
using Incubator.Domain.Enums;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Incubator.Infrastructure.Services
{
    public class FrameStorageOrchestrator : IFrameStorageService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IAppConfigService _configService;

        public FrameStorageOrchestrator(IServiceProvider serviceProvider, IAppConfigService configService)
        {
            _serviceProvider = serviceProvider;
            _configService = configService;
        }

        public async Task SaveFrameAsync(IncubatorFrame frame)
        {
            var config = _configService.GetConfig();

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
