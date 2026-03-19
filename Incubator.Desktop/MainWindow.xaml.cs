using Incubator.Desktop.ViewModels;
using System.Windows;

namespace Incubator.Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //public MainWindow()
        //{
        //    InitializeComponent();
        //}

        //Como registramos MainWindow y MainViewModel en nuestro App.xaml.cs, 
        //    el contenedor de DI es lo suficientemente inteligente para saber que cuando pides la ventana, 
        //    primero debe crear el ViewModel y pasárselo.

        // Pedimos el ViewModel a través del constructor
        public MainWindow(MainViewModel viewModel)
        {
            InitializeComponent();

            // Establecemos el ViewModel inyectado como el DataContext de esta ventana.
            // Esto le dice al XAML: "Busca tus datos aquí".
            this.DataContext = viewModel;
        }
    }
}