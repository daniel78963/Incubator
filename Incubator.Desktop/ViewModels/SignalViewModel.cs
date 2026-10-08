using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Incubator.Application.Interfaces;
using Incubator.Domain.Entities;
using System;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Threading.Channels;
using System.Windows;
using WpfApp = System.Windows.Application;

namespace Incubator.Desktop.ViewModels
{
    public partial class SignalViewModel : ObservableObject
    {
        private readonly IFrameStorageService _storageService;

        // El Canal (Cola concurrente de alto rendimiento)
        private readonly Channel<string> _frameQueue;

        // Conexión 1 (COM Clásico)
        private SerialPort? _serialPort1;
        private string _bufferPort1 = string.Empty;

        // Conexión 2 (USB Virtual COM)
        private SerialPort? _serialPort2;
        private string _bufferPort2 = string.Empty;

        public ObservableCollection<IncubatorFrame> FrameHistory { get; } = new();

        [ObservableProperty] private string _selectedPort1 = string.Empty;
        [ObservableProperty] private string _selectedPort2 = string.Empty;
        [ObservableProperty] private string _connectionStatus = "Desconectados";

        public string[] AvailablePorts => SerialPort.GetPortNames();

        public SignalViewModel(IFrameStorageService storageService)
        {
            _storageService = storageService;

            // Creamos un canal sin límite de capacidad
            _frameQueue = Channel.CreateUnbounded<string>();

            // Iniciamos el consumidor en segundo plano
            _ = ProcessQueueAsync();
        }

        [RelayCommand]
        private void Connect()
        {
            try
            {
                // Conectar Dispositivo 1
                if (!string.IsNullOrEmpty(SelectedPort1))
                {
                    _serialPort1 = new SerialPort(SelectedPort1, 9600, Parity.None, 8, StopBits.One);
                    _serialPort1.DataReceived += (s, e) => OnDataReceived(_serialPort1, ref _bufferPort1);
                    _serialPort1.Open();
                }

                // Conectar Dispositivo 2 (USB)
                if (!string.IsNullOrEmpty(SelectedPort2))
                {
                    _serialPort2 = new SerialPort(SelectedPort2, 9600, Parity.None, 8, StopBits.One);
                    _serialPort2.DataReceived += (s, e) => OnDataReceived(_serialPort2, ref _bufferPort2);
                    _serialPort2.Open();
                }

                ConnectionStatus = "Conectados a canales seleccionados";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error de conexión: {ex.Message}");
            }
        }

        // Método genérico para escuchar cualquier puerto y escribir en el canal
        private void OnDataReceived(SerialPort port, ref string buffer)
        {
            if (port == null || !port.IsOpen) return;

            string incomingData = port.ReadExisting();
            buffer += incomingData;

            // Procesar mientras exista el terminador de trama
            while (buffer.Contains("\r\n")) //[cite: 7]
            {
                int terminatorIndex = buffer.IndexOf("\r\n"); //[cite: 7]
                string completeFrame = buffer.Substring(0, terminatorIndex + 2);
                buffer = buffer.Substring(terminatorIndex + 2);

                // ¡PRODUCCIÓN! Enviamos a la cola de forma segura y rápida
                _frameQueue.Writer.TryWrite(completeFrame);
            }
        }

        // EL CONSUMIDOR: Procesa una a una las tramas encoladas
        private async Task ProcessQueueAsync()
        {
            // Este bucle se mantiene vivo siempre y se despierta cuando entra una trama al canal
            await foreach (var rawFrame in _frameQueue.Reader.ReadAllAsync())
            {
                try
                {
                    // 1. Armado y Decodificación
                    var decodedFrame = FrameDecoder.Decode(rawFrame);

                    // 2. Almacenamiento (BD o TXT según configuración)
                    await _storageService.SaveFrameAsync(decodedFrame);

                    // 3. Mostrar en UI
                    WpfApp.Current.Dispatcher.Invoke(() =>
                    {
                        FrameHistory.Insert(0, decodedFrame);
                    });
                }
                catch (Exception ex)
                {
                    // Log: Trama corrupta o error de BD
                }
            }
        }
    }
}
