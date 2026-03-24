using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Incubator.Application.Interfaces;
using Incubator.Desktop.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Incubator.Desktop.ViewModels
{
    public partial class ConfiguracionViewModel : ObservableObject
    {
        private readonly ILegacyCalculatorService _legacyService;
        private readonly IDialogService _dialogService;

        [ObservableProperty]
        private int _inputValue = 5; // Valor por defecto 

        [ObservableProperty]
        private string _resultText = "Esperando cálculo...";

        [ObservableProperty]
        private bool _isProcessing;

        public ConfiguracionViewModel(ILegacyCalculatorService legacyService, IDialogService dialogService)
        {
            _legacyService = legacyService;
            _dialogService = dialogService;
        }

        [RelayCommand]
        private async Task CalcularLegacyAsync()
        {
            IsProcessing = true;
            ResultText = "Procesando en entorno x86...";

            // Configuramos un timeout profesional de 5 segundos
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

            try
            {
                // Llamamos a la infraestructura
                int resultado = await _legacyService.CalculateComplexValueAsync(InputValue, cts.Token);
                ResultText = $"Resultado de la DLL vieja: {resultado}";
            }
            catch (TimeoutException ex)
            {
                ResultText = "Fallo por Timeout";
                _dialogService.ShowMessage("Timeout", ex.Message);
            }
            catch (Exception ex)
            {
                ResultText = "Error en ejecución";
                _dialogService.ShowMessage("Error Legacy", ex.Message);
            }
            finally
            {
                IsProcessing = false;
            }
        }
    }
}
