using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Text;

namespace Incubator.Desktop.ViewModels
{
    public partial class HomeViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool _isLoading;

        [ObservableProperty]
        private ObservableCollection<string> _availableSerialPorts = new ObservableCollection<string>();

        [ObservableProperty]
        private string _selectedSerialPort;


        public HomeViewModel()
        {
            RefreshSerialPorts();
        }

        private void RefreshSerialPorts()
        {
            _availableSerialPorts.Clear();
            var portNames = System.IO.Ports.SerialPort.GetPortNames();
            foreach (var port in portNames)
            {
                _availableSerialPorts.Add(port);
            }
        }
    }
}
