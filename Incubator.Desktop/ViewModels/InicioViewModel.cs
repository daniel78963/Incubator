using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging; // Para el Messenger
using Incubator.Application.UseCases;
using Incubator.Desktop.Messages;
using Incubator.Desktop.Services;
using Incubator.Domain.Entities;
using System.Collections.ObjectModel;

namespace Incubator.Desktop.ViewModels
{
    public partial class InicioViewModel : ObservableObject, IRecipient<ClientCreatedMessage>
    {
        private readonly IGetClientsUseCase _obtenerClientesUseCase;
        private readonly IDialogService _dialogService;

        // ObservableCollection avisa a la vista automáticamente cuando se añaden o quitan elementos
        public ObservableCollection<Client> Clients { get; } = new();

        [ObservableProperty]
        private bool _isLoading;

        // Pedimos el caso de uso por constructor
        public InicioViewModel(IGetClientsUseCase obtenerClientesUseCase, IDialogService dialogService)
        {
            _obtenerClientesUseCase = obtenerClientesUseCase;
            _dialogService = dialogService;
            // 2. Nos registramos en el mensajero al construir el ViewModel
            // Esto le dice al Messenger: "Avísame cuando alguien envíe un ClienteCreadoMessage"
            WeakReferenceMessenger.Default.Register(this);
        }

        [RelayCommand]
        private async Task CargarDatosAsync()
        {
            IsLoading = true;
            Clients.Clear();

            try
            {
                // Ejecutamos la lógica de negocio puramente agnóstica de la UI
                var resultado = await _obtenerClientesUseCase.EjecutarAsync();

                foreach (var cliente in resultado)
                {
                    Clients.Add(cliente);
                }
            }
            catch (Exception ex)
            {
                // ¡Aquí usamos el servicio! Si la base de datos está caída, el usuario se entera.
                _dialogService.ShowMessage(
                    "Error de Conexión",
                    $"No se pudieron cargar los clientes. Detalle: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        // Ejemplo de un comando para eliminar que requiere confirmación
        [RelayCommand]
        private void DeleteClient(Client clienteSeleccionado)
        {
            if (clienteSeleccionado == null) return;

            // Pedimos confirmación sin tocar el UI desde el ViewModel
            bool confirmar = _dialogService.ConfirmationMessage(
                "Eliminar Cliente",
                $"¿Estás seguro de que deseas eliminar a {clienteSeleccionado.Name}?");

            if (confirmar)
            {
                // Aquí llamarías a tu caso de uso para eliminar:
                // await _eliminarClienteUseCase.EjecutarAsync(clienteSeleccionado.Id);

                Clients.Remove(clienteSeleccionado);
                _dialogService.ShowMessage("Éxito", "El cliente fue eliminado correctamente.");
            }
        }

        // 3. Implementamos el método obligatorio de la interfaz IRecipient
        public void Receive(ClientCreatedMessage message)
        {
            // El toolkit nos pasa el mensaje. La propiedad 'Value' contiene el Cliente que enviamos.
            Client nuevoCliente = message.Value;

            // Lo agregamos a nuestra lista. Como es una ObservableCollection, 
            // la tabla (DataGrid) en la UI se actualizará instantáneamente.
            Clients.Add(nuevoCliente);
        }
    }
}
