using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Incubator.Application.Interfaces;
using Incubator.Domain.Entities;
using Incubator.Domain.Enums;
using Incubator.Desktop.Services;
using System;

namespace Incubator.Desktop.ViewModels
{
    public partial class ConfigurationViewModel : ObservableObject
    {
        private readonly IAppConfigService _configService;
        private readonly IDialogService _dialogService;

        [ObservableProperty]
        private bool _isFlatFileSelected;

        [ObservableProperty]
        private bool _isDatabaseSelected;

        [ObservableProperty]
        private string _fileFormat = string.Empty;

        [ObservableProperty]
        private string _previewFileName = string.Empty;

        public ConfigurationViewModel(IAppConfigService configService, IDialogService dialogService)
        {
            _configService = configService;
            _dialogService = dialogService;
            LoadConfig();
        }

        private void LoadConfig()
        {
            var config = _configService.GetConfig();
            IsFlatFileSelected = config.StorageType == StorageMethod.FlatFile;
            IsDatabaseSelected = config.StorageType == StorageMethod.Database;
            FileFormat = config.FlatFileFormat;
            UpdatePreview();
        }

        partial void OnFileFormatChanged(string value)
        {
            UpdatePreview();
        }

        private void UpdatePreview()
        {
            try
            {
                PreviewFileName = $@"C:\SmartIncubator\Plots\{DateTime.Now.ToString(FileFormat)}.txt";
            }
            catch
            {
                PreviewFileName = "Formato inválido";
            }
        }

        [RelayCommand]
        private void SaveConfig()
        {
            var config = new AppConfiguration
            {
                StorageType = IsDatabaseSelected ? StorageMethod.Database : StorageMethod.FlatFile,
                FlatFileFormat = FileFormat
            };

            _configService.SaveConfig(config);
            _dialogService.ShowMessage("Éxito", "Configuración de almacenamiento guardada correctamente.");
        }
    }
}
