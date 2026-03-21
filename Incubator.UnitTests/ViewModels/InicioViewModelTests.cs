using Incubator.Application.UseCases;
using Incubator.Desktop.Services;
using Incubator.Desktop.ViewModels;
using Incubator.Domain.Entities;
using Moq; // Debes instalar el paquete NuGet 'Moq'
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;
using Xunit;

namespace Incubator.UnitTests.ViewModels
{
    public class InicioViewModelTests
    {
        [Fact]
        public async Task CargarDatosCommand_DebeLlenarLaLista_Y_ManejarElEstadoIsLoading()
        {
            // 1. ARRANGE (Preparar el escenario)

            // Creamos clientes falsos para la prueba
            var clientesFalsos = new List<Client>
            {
                new Client { Id = 1, Name = "Cliente Prueba 1" },
                new Client { Id = 2, Name = "Cliente Prueba 2" }
            };

            // "Mockeamos" (simulamos) el caso de uso para que devuelva nuestra lista
            // falsa sin tocar la base de datos de Entity Framework.
            var mockUseCase = new Mock<IGetClientsUseCase>();
            mockUseCase
                .Setup(u => u.EjecutarAsync())
                .ReturnsAsync(clientesFalsos);

            // Mockeamos el servicio de diálogos (no queremos que salten ventanas en los tests)
            var mockDialogService = new Mock<IDialogService>();

            // Instanciamos el ViewModel inyectándole nuestros simuladores
            var viewModel = new InicioViewModel(mockUseCase.Object, mockDialogService.Object);

            // Verificamos el estado inicial antes de ejecutar nada
            Assert.Empty(viewModel.Clients);
            Assert.False(viewModel.IsLoading);

            // 2. ACT (Ejecutar la acción que queremos probar)

            // Ejecutamos el comando asíncrono
            await viewModel.CargarDatosCommand.ExecuteAsync(null);

            // 3. ASSERT (Verificar que los resultados son los esperados)

            // Verificamos que la lista ahora tiene 2 elementos
            Assert.Equal(2, viewModel.Clients.Count);

            // Verificamos que el primer elemento es el correcto
            Assert.Equal("Cliente Prueba 1", viewModel.Clients[0].Name);

            // Verificamos que el IsLoading se apagó al terminar
            Assert.False(viewModel.IsLoading);

            // Opcional: Verificar que el caso de uso fue llamado exactamente 1 vez
            mockUseCase.Verify(u => u.EjecutarAsync(), Times.Once);
        }

        [Fact]
        public async Task CargarDatosCommand_SiHayError_DebeMostrarMensajeYApagarIsLoading()
        {
            // ARRANGE
            var mockUseCase = new Mock<IGetClientsUseCase>();

            // Forzamos a que el caso de uso lance una excepción (simulando que se cayó la BD)
            mockUseCase
                .Setup(u => u.EjecutarAsync())
                .ThrowsAsync(new System.Exception("Base de datos desconectada"));

            var mockDialogService = new Mock<IDialogService>();

            var viewModel = new InicioViewModel(mockUseCase.Object, mockDialogService.Object);

            // ACT
            await viewModel.CargarDatosCommand.ExecuteAsync(null);

            // ASSERT
            // Verificamos que la lista sigue vacía
            Assert.Empty(viewModel.Clients);

            // Verificamos que se apagó el estado de carga en el bloque finally
            Assert.False(viewModel.IsLoading);

            // Verificamos que el servicio de diálogos FUE llamado para mostrar el error
            mockDialogService.Verify(d => d.ShowMessage("Error de Conexión", It.IsAny<string>()), Times.Once);
        }
    }
}