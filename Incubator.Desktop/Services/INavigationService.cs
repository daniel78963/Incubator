using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Incubator.Desktop.Services
{
    public interface INavigationService
    {
        // Propiedad que almacenará el ViewModel actual
        ObservableObject VistaActual { get; }

        // Evento para avisar que la vista cambió
        event Action StateChanged;

        // Método para cambiar la vista
        void NavigateTo<TViewModel>() where TViewModel : ObservableObject;
    }
}
