using Incubator.Desktop.ViewModels;
using System.Windows.Controls;

namespace Incubator.Desktop.Views
{
    /// <summary>
    /// Interaction logic for SignalView.xaml
    /// </summary>
    public partial class SignalView : UserControl
    {
        public SignalView(SignalViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
        }
    }
}
