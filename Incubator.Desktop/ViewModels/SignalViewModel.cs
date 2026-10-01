using System;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Incubator.Domain.Entities;

namespace Incubator.Desktop.ViewModels
{
    public partial class SignalViewModel : ObservableObject
    {
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

        private void ProcessFrame(string rawFrame)
        {
            try
            {
                var decodedFrame = FrameDecoder.Decode(rawFrame);

                // Despachar a la UI ya que SerialPort usa un hilo secundario
                global::System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    FrameHistory.Insert(0, decodedFrame);
                });
            }
            catch (Exception ex)
            {
                // Manejar error de decodificación o trama corrupta
            }
        }
    }
}
