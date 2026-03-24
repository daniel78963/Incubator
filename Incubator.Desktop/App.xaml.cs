using CommunityToolkit.Mvvm.ComponentModel;
using Incubator.Application.Interfaces;
using Incubator.Application.UseCases;
using Incubator.Desktop.Services;
//using Incubator.Desktop.Views;
using Incubator.Desktop.ViewModels;
using Incubator.Infrastructure.Data;
using Incubator.Infrastructure.Repositories;
using Incubator.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows;
using System.Windows.Media.Media3D;

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
                    // El CreateDefaultBuilder ya carga 'appsettings.json' y 'appsettings.Development.json' por defecto.
                    // La configuración está disponible en hostContext.Configuration

                    //Leemos la cadena de conexión del appsettings.json
                    string? dBConnection = hostContext.Configuration.GetConnectionString("MyDbConnection");
                    //Registramos el DbContext
                    services.AddDbContext<MyDbContext>(options =>
                    {
                        options.UseSqlServer(dBConnection);
                    });

                    // 1. Registrar Vistas (Windows)
                    // Usamos Singleton para la ventana principal porque solo habrá una en toda la app.
                    services.AddSingleton<MainWindow>();

                    // 2. Registrar ViewModels
                    // Usamos Transient para que cree una nueva instancia si abrimos múltiples vistas iguales.
                    services.AddTransient<MainViewModel>();

                    // 3. Registrar Servicios y configuración de otras capas (Infrastructure, Application)
                    // services.AddSingleton<IMiServicio, MiServicio>();
                    services.AddSingleton<IDialogService, DialogService>();

                    // Ejemplo de cómo leer una sección específica del appsettings.json si lo necesitas
                    // var miConfig = hostContext.Configuration.GetSection("MiConfiguracion").Get<MiConfiguracionObj>();

                    // 1. Capa Infrastructure: Registramos la implementación real del repositorio
                    services.AddTransient<IClientRepository, ClientRepository>();
                    // 2. Capa Application: Registramos los casos de uso
                    services.AddTransient<IGetClientsUseCase, GetClientsUseCase>();

                    // 1. Registramos el servicio de navegación como Singleton (solo uno en toda la app)
                    services.AddSingleton<INavigationService, NavigationService>(provider =>
                    {
                        // Le enseñamos al servicio cómo obtener los ViewModels del contenedor
                        return new NavigationService(viewModelType =>
                            (ObservableObject)provider.GetRequiredService(viewModelType));
                    });

                    // 2. Registramos TODOS nuestros ViewModels
                    services.AddTransient<InicioViewModel>();
                    services.AddTransient<ConfiguracionViewModel>();
                    services.AddTransient<CrearClienteViewModel>();
                    services.AddSingleton<MainViewModel>();

                    services.AddTransient<ILegacyCalculatorService, LegacyCalculatorService>();
                })
                .Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Iniciamos el Host
            await AppHost!.StartAsync();

            // Le pedimos al contenedor de dependencias que nos dé la ventana principal
            // Al instanciar MainWindow, el contenedor inyectará automáticamente el MainViewModel si lo necesita en su constructor.
            var mainWindow = AppHost.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            // Detenemos el Host de forma limpia al cerrar la app
            await AppHost!.StopAsync();
            base.OnExit(e);
        }
    }
}