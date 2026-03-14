using Microsoft.Extensions.Hosting;
using System.Configuration;
using System.Data;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;
//using Incubator.Desktop.Views;
using Incubator.Desktop.ViewModels;

namespace Incubator.Desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
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

                    // 1. Registrar Vistas (Windows)
                    // Usamos Singleton para la ventana principal porque solo habrá una en toda la app.
                    services.AddSingleton<MainWindow>();

                    // 2. Registrar ViewModels
                    // Usamos Transient para que cree una nueva instancia si abrimos múltiples vistas iguales.
                    services.AddTransient<MainViewModel>();

                    // 3. Registrar Servicios y configuración de otras capas (Infrastructure, Application)
                    // services.AddSingleton<IMiServicio, MiServicio>();

                    // Ejemplo de cómo leer una sección específica del appsettings.json si lo necesitas
                    // var miConfig = hostContext.Configuration.GetSection("MiConfiguracion").Get<MiConfiguracionObj>();
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