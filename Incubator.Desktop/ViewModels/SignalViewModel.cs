using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Incubator.Application.Interfaces;
using Incubator.Domain.Entities;
using System;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Windows;
using WpfApp = System.Windows.Application;

namespace Incubator.Desktop.ViewModels
{
    public partial class SignalViewModel : ObservableObject
    {
        private readonly IFrameStorageService _storageService;
        public SignalViewModel(IFrameStorageService storageService)
        {
            _storageService = storageService;
        }

        private SerialPort? _serialPort;
        private string _buffer = string.Empty;

        public ObservableCollection<IncubatorFrame> FrameHistory { get; } = new();

        [ObservableProperty]
        private string _selectedPort = string.Empty;

        [ObservableProperty]
        private string _connectionStatus = "Disconnected";

        public string[] AvailablePorts => SerialPort.GetPortNames();

        [RelayCommand]
        private void Connect()
        {
            try
            {
                _serialPort = new SerialPort(SelectedPort, 9600, Parity.None, 8, StopBits.One);
                _serialPort.DataReceived += OnDataReceived;
                _serialPort.Open();

                ConnectionStatus = $"Connected to {SelectedPort}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Connection error: {ex.Message}");
            }
        }

        private void OnDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (_serialPort == null) return;

            string incomingData = _serialPort.ReadExisting();
            _buffer += incomingData;

            // Procesar mientras encontremos el terminador \r\n
            while (_buffer.Contains("\r\n"))
            {
                int terminatorIndex = _buffer.IndexOf("\r\n");

                string completeFrame = _buffer.Substring(0, terminatorIndex + 2);
                _buffer = _buffer.Substring(terminatorIndex + 2);

                ProcessFrame(completeFrame);
            }
        }

        private async void ProcessFrame(string rawFrame)
        {
            try
            {
                var decodedFrame = FrameDecoder.Decode(rawFrame);

                // 1. Guardar la trama (El orquestador decide si va a BD o a TXT)
                await _storageService.SaveFrameAsync(decodedFrame);

                // 2. Mostrar en UI
                WpfApp.Current.Dispatcher.Invoke(() =>
                {
                    FrameHistory.Insert(0, decodedFrame);
                });
            }
            catch (Exception ex)
            {
                // Manejo de errores
            }
        }
    }
}
