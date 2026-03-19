using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging; // Importante para el WeakReferenceMessenger
using Incubator.Desktop.Messages;
using Incubator.Desktop.Services;
using Incubator.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Incubator.Desktop.ViewModels
{
    // Heredamos de ObservableValidator para habilitar la validación
    public partial class CrearClienteViewModel : ObservableValidator
    {
        private readonly IDialogService _dialogService;

        // 1. Agregamos las reglas de validación usando DataAnnotations
        [ObservableProperty]
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [MinLength(3, ErrorMessage = "El nombre debe tener al menos 3 letras.")]
        private string _name = string.Empty;

        [ObservableProperty]
        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del correo no es válido.")]
        private string _email = string.Empty;

        public CrearClienteViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;
        }

        [RelayCommand]
        private void GuardarCliente()
        {
            // 2. Forzamos la validación de todas las propiedades al intentar guardar
            ValidateAllProperties();

            // 3. Verificamos si hay algún error
            if (HasErrors)
            {
                _dialogService.ShowMessage("Error de Validación", "Por favor, corrige los errores en el formulario antes de continuar.");
                return;
            }

            // Si llegamos aquí, los datos son válidos en la UI.
            // Aquí llamarías a tu caso de uso: await _crearClienteUseCase.EjecutarAsync(Nombre, Correo);
            // Simulamos la creación del cliente
            var newClient = new Client
            {
                Id = new Random().Next(100, 999), // ID simulado
                Name = this.Name,
                Email = this.Email
            };

            // ENVIAMOS EL MENSAJE
            // Usamos el mensajero por defecto del toolkit para emitir el evento a toda la app
            WeakReferenceMessenger.Default.Send(new ClientCreatedMessage(newClient));

            // Limpiamos el formulario o cerramos la vista...
            Name = string.Empty;
            Email = string.Empty;

            _dialogService.ShowMessage("Éxito", $"Cliente {Name} guardado correctamente.");
        }
    }
}
