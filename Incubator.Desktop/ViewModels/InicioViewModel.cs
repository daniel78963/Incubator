using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Incubator.Domain.Entities;
using Incubator.Application.UseCases;

namespace Incubator.Desktop.ViewModels
{
    public partial class InicioViewModel : ObservableObject
    {
        private readonly IGetClientsUseCase _obtenerClientesUseCase;

        // ObservableCollection avisa a la vista automáticamente cuando se añaden o quitan elementos
        public ObservableCollection<Client> Clientes { get; } = new();

        [ObservableProperty]
        private bool _isLoading;

        // Pedimos el caso de uso por constructor
        public InicioViewModel(IGetClientsUseCase obtenerClientesUseCase)
        {
            _obtenerClientesUseCase = obtenerClientesUseCase;
        }

        [RelayCommand]
        private async Task CargarDatosAsync()
        {
            IsLoading = true;
            Clientes.Clear();

            // Ejecutamos la lógica de negocio puramente agnóstica de la UI
            var resultado = await _obtenerClientesUseCase.EjecutarAsync();

            foreach (var cliente in resultado)
            {
                Clientes.Add(cliente);
            }

            IsLoading = false;
        }
    }
}
