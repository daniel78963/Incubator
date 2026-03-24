using CommunityToolkit.Mvvm.ComponentModel;

namespace Incubator.Desktop.ViewModels
{
    // Heredamos de ObservableObject para notificar a la UI de cualquier cambio global
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _applicationTitle = "Incubator Desktop";

        // ¡Y eso es todo por ahora! 

        // Ya no necesitas inyectar ningún NavigationService aquí.
        // Tampoco necesitas comandos de navegación.

        // En el futuro, aquí podrías agregar:
        // - Información del usuario autenticado.
        // - Contadores de notificaciones globales.
        // - Estado de conexión general (ej. "Online" / "Offline").
    }
}