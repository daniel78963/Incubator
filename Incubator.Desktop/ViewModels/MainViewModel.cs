using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Incubator.Desktop.ViewModels
{
    // Heredar de ObservableObject nos da toda la plomería necesaria para notificar a la vista
    public partial class MainViewModel : ObservableObject
    {
        // El toolkit generará automáticamente una propiedad pública 'TituloVentana'
        // que avisa a la vista cuando su valor cambia.
        [ObservableProperty]
        private string _tituloVentana = "Mi Aplicación WPF Moderna";

        // Si tuvieras servicios (por ejemplo, para consultar la base de datos),
        // simplemente los pides aquí y el Host de App.xaml.cs te los entregará.
        // public MainViewModel(IMiServicioBaseDatos dbService) { ... }

        [ObservableProperty]
        private int _contadorClicks = 0;

        [ObservableProperty]
        private string _mensaje = "Esperando acción...";

        // 1. Comando Síncrono Normal
        // El toolkit genera automáticamente un ICommand público llamado 'SaludarCommand'
        // (toma el nombre de tu método y le agrega el sufijo 'Command').
        [RelayCommand]
        private void Saludar()
        {
            TituloVentana = "¡Hola desde el ViewModel!";
            ContadorClicks++;
        }

        // 2. Comando Asíncrono
        // El toolkit es lo suficientemente inteligente para manejar tareas asíncronas.
        // Esto generará un ICommand llamado 'CargarDatosCommand'.
        [RelayCommand]
        private async Task CargarDatosAsync()
        {
            TituloVentana = "Cargando datos...";

            // Simulamos una llamada a base de datos o API que toma 2 segundos
            await Task.Delay(2000);

            TituloVentana = "¡Datos cargados exitosamente!";
        }

        // Al agregar un parámetro (string nombre), el toolkit genera un 
        // comando que ESPERA recibir un string.
        [RelayCommand]
        private void SaludarUsuario(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                Mensaje = "Por favor, ingresa un nombre.";
                return;
            }

            Mensaje = $"¡Hola, {nombre}! Parámetro recibido correctamente.";
        }
    }
}
