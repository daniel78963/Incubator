using Incubator.Desktop.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Incubator.Desktop.Views
{
    /// <summary>
    /// Interaction logic for CrearClienteView.xaml
    /// </summary>
    public partial class CrearClienteView : UserControl
    {
        public CrearClienteView(CrearClienteViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
        }
    }
}
