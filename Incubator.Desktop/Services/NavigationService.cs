using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Incubator.Desktop.Services
{
    public class NavigationService : INavigationService
    {
        private ObservableObject _vistaActual;
        private readonly Func<Type, ObservableObject> _viewModelFactory;

        public ObservableObject VistaActual
        {
            get => _vistaActual;
            private set
            {
                _vistaActual = value;
                StateChanged?.Invoke(); // Avisamos que hubo un cambio
            }
        }

        public event Action? StateChanged;

        // Recibimos una fábrica capaz de resolver dependencias
        public NavigationService(Func<Type, ObservableObject> viewModelFactory)
        {
            _viewModelFactory = viewModelFactory;
        }

        public void NavigateTo<TViewModel>() where TViewModel : ObservableObject
        {
            // Creamos el nuevo ViewModel pidiéndoselo a la fábrica (y al contenedor de DI)
            ObservableObject viewModel = _viewModelFactory.Invoke(typeof(TViewModel));
            VistaActual = viewModel;
        }
    }
}