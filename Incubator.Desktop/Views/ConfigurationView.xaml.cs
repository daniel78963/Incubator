using Incubator.Desktop.ViewModels;
using System.Windows.Controls;

namespace Incubator.Desktop.Views
{
    /// <summary>
    /// Interaction logic for ConfigurationView.xaml
    /// </summary>
    public partial class ConfigurationView : UserControl
    {
        public ConfigurationView(ConfigurationViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
        }
    }
}
