using System.Windows;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

// Importamos las capas de nuestra Arquitectura Limpia
using Incubator.Application.Interfaces;
using Incubator.Application.UseCases;
using Incubator.Infrastructure.Data;
using Incubator.Infrastructure.Repositories;
using Incubator.Infrastructure.Services;

// Importamos la capa de Presentación
using Incubator.Desktop.Services;
using Incubator.Desktop.ViewModels;
using Incubator.Desktop.Views;

namespace Incubator.Desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        // Exponemos el Host por si necesitamos acceder a los servicios globalmente
        public static IHost? AppHost { get; private set; }

        public App()
        {
            AppHost = Host.CreateDefaultBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    // ==========================================================
                    // 1. CONFIGURACIÓN DE BASE DE DATOS
                    // ==========================================================
                    string? dbConnection = hostContext.Configuration.GetConnectionString("MyDbConnection");
                    services.AddDbContext<MyDbContext>(options =>
                    {
                        options.UseSqlServer(dbConnection);
                    });

                    // ==========================================================
                    // 2. CAPA INFRASTRUCTURE (El detalle técnico)
                    // ==========================================================
                    services.AddTransient<IClientRepository, ClientRepository>();
                    services.AddTransient<ILegacyCalculatorService, LegacyCalculatorService>(); // Nuestro puente x86

                    // ==========================================================
                    // 3. CAPA APPLICATION (Reglas de negocio y casos de uso)
                    // ==========================================================
                    services.AddTransient<IGetClientsUseCase, GetClientsUseCase>();

                    // ==========================================================
                    // 4. SERVICIOS DE INTERFAZ DE USUARIO (Propios)
                    // ==========================================================
                    services.AddSingleton<IDialogService, DialogService>();

                    // ==========================================================
                    // 5. SERVICIOS DE INTERFAZ DE USUARIO (WPF UI - Lepo.co)
                    // ==========================================================
                    // 5.1. AGREGAMOS EL PROVEEDOR DE PÁGINAS AQUÍ:
                    services.AddSingleton<Wpf.Ui.Abstractions.INavigationViewPageProvider, PageService>();
                    // 5.2 Los demás servicios 
                    services.AddSingleton<Wpf.Ui.INavigationService, Wpf.Ui.NavigationService>();
                    services.AddSingleton<Wpf.Ui.ISnackbarService, Wpf.Ui.SnackbarService>();
                    services.AddSingleton<Wpf.Ui.IContentDialogService, Wpf.Ui.ContentDialogService>();

                    // ==========================================================
                    // 6. REGISTRO DE VENTANA PRINCIPAL (Singleton)
                    // ==========================================================
                    services.AddSingleton<MainWindow>();
                    services.AddSingleton<MainViewModel>();

                    // ==========================================================
                    // 7. REGISTRO DE VISTAS Y SUS VIEWMODELS (Transient)
                    // ==========================================================
                    // Usamos Transient para que cada vez que el usuario navegue a una página, 
                    // se cree una instancia fresca con el estado limpio.

                    services.AddTransient<InicioView>();
                    services.AddTransient<InicioViewModel>();

                    services.AddTransient<ConfiguracionView>();
                    services.AddTransient<ConfiguracionViewModel>();

                    // Si agregaste la vista de Crear Cliente, descomenta estas líneas:
                    // services.AddTransient<CrearClienteView>();
                    // services.AddTransient<CrearClienteViewModel>();
                })
                .Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Iniciamos el Host genérico
            await AppHost!.StartAsync();

            // Le pedimos al contenedor de dependencias que nos dé la ventana principal
            // El contenedor se encargará de inyectar automáticamente el MainViewModel, 
            // el INavigationService de WPF UI, y el IServiceProvider.
            var mainWindow = AppHost.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            // Detenemos el Host de forma limpia al cerrar la aplicación para liberar recursos
            await AppHost!.StopAsync();
            base.OnExit(e);
        }
    }
}