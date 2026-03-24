using System;
using Wpf.Ui.Controls;
using Incubator.Desktop.ViewModels;

namespace Incubator.Desktop
{
    public partial class MainWindow : FluentWindow
    {
        public MainWindow(
            MainViewModel viewModel,
            Wpf.Ui.INavigationService navigationService)
        {
            InitializeComponent();
            DataContext = viewModel;

            // Le decimos al servicio de navegación global qué control visual va a gobernar
            navigationService.SetNavigationControl(RootNavigation);
        }
    }
}