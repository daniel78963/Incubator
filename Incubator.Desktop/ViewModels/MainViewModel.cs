using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Incubator.Desktop.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Navigation;

namespace Incubator.Desktop.ViewModels
{
    // Heredar de ObservableObject nos da toda la plomería necesaria para notificar a la vista
    public partial class MainViewModel : ObservableObject
    {
        private readonly INavigationService _navigationService;

        //// Esta propiedad guarda el ViewModel que está activo actualmente.
        //// Usamos 'ObservableObject' como tipo base para que acepte cualquier ViewModel.
        //[ObservableProperty]
        //private ObservableObject _vistaActual;

        // Exponemos la vista actual para que el XAML haga el Binding
        public ObservableObject VistaActual => _navigationService.VistaActual;

        //public MainViewModel()
        // Inyectamos el servicio por constructor
        public MainViewModel(INavigationService navigationService)
        {
            //// Vista por defecto al arrancar
            //VistaActual = new InicioViewModel();

            ////(Nota: En un escenario real con Inyección de Dependencias, en lugar de hacer new, le pediríamos estos ViewModels al contenedor, pero esto ilustra el concepto base).
            
            _navigationService = navigationService;

            // Nos suscribimos al evento para que cuando el servicio cambie la vista, 
            // el MainViewModel avise a la UI que la propiedad 'VistaActual' cambió.
            _navigationService.StateChanged += () => OnPropertyChanged(nameof(VistaActual));

            // Vista inicial
            _navigationService.NavigateTo<InicioViewModel>();
        }

        //[RelayCommand]
        //private void NavegarAInicio() => VistaActual = new InicioViewModel();

        //[RelayCommand]
        //private void NavegarAConfiguracion() => VistaActual = new ConfiguracionViewModel();
        // Los comandos ahora simplemente llaman al servicio
        [RelayCommand]
        private void NavegarAInicio() => _navigationService.NavigateTo<InicioViewModel>();

        [RelayCommand]
        private void NavegarAConfiguracion() => _navigationService.NavigateTo<ConfiguracionViewModel>();

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
