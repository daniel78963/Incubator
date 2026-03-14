using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Incubator.Desktop.ViewModels
{
    // Heredar de ObservableObject nos da toda la plomería necesaria para notificar a la vista
    public partial class MainViewModel : ObservableObject
    {
        // El toolkit generará automáticamente una propiedad pública 'TituloVentana'
        // que avisa a la vista cuando su valor cambia.
        [ObservableProperty]
        private string _tituloVentana = "Mi Aplicación WPF Moderna";

        // Si tuvieras servicios (por ejemplo, para consultar la base de datos),
        // simplemente los pides aquí y el Host de App.xaml.cs te los entregará.
        // public MainViewModel(IMiServicioBaseDatos dbService) { ... }
    }
}
