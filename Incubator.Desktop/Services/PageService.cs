using System;
using System.Windows;
using Wpf.Ui.Abstractions;

namespace Incubator.Desktop.Services
{
    // Implementamos la interfaz exacta que el error está pidiendo
    public class PageService : INavigationViewPageProvider
    {
        private readonly IServiceProvider _serviceProvider;

        // Inyectamos el contenedor de dependencias de Microsoft
        public PageService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        // WPF UI llamará a este método cada vez que hagas clic en un elemento del menú
        public object? GetPage(Type pageType)
        {
            // Verificamos por seguridad que lo que intentamos cargar sea un control visual de WPF
            if (!typeof(FrameworkElement).IsAssignableFrom(pageType))
            {
                throw new InvalidOperationException("La página debe ser un control de WPF.");
            }

            // Le pedimos al contenedor de dependencias que nos dé la instancia de la Vista
            // (Esto automáticamente resolverá el ViewModel asociado si lo configuramos bien)
            return _serviceProvider.GetService(pageType);
        }
    }
}