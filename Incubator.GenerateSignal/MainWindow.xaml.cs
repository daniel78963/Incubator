using System;
using System.IO.Ports;
using System.Windows;

namespace Incubator.GenerateSignal
{
    public partial class MainWindow : Window
    {
        private SerialPort? _serialPort;

        public MainWindow()
        {
            InitializeComponent();
            LoadConfiguration();
        }

        private void LoadConfiguration()
        {
            // Cargar los puertos COM instalados en el equipo
            string[] ports = SerialPort.GetPortNames();
            CmbPorts.ItemsSource = ports;
            if (ports.Length > 0) CmbPorts.SelectedIndex = 0;

            // Cargar los Baud Rates comunes
            CmbBaudRate.ItemsSource = new int[] { 9600, 19200, 38400, 115200 };
            CmbBaudRate.SelectedItem = 9600;
        }

        private void BtnConnect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_serialPort != null && _serialPort.IsOpen)
                {
                    // Lógica para desconectar
                    _serialPort.Close();
                    BtnConnect.Content = "Conectar";
                    BtnSendFrame.IsEnabled = false;
                    TxtStatus.Text = "Desconectado";
                    TxtStatus.Foreground = System.Windows.Media.Brushes.Gray;
                    return;
                }

                if (CmbPorts.SelectedItem == null)
                {
                    MessageBox.Show("Por favor selecciona un puerto COM.");
                    return;
                }

                // Configurar y abrir el puerto
                _serialPort = new SerialPort(CmbPorts.SelectedItem.ToString()!)
                {
                    BaudRate = (int)CmbBaudRate.SelectedItem,
                    Parity = Parity.None,
                    DataBits = 8,
                    StopBits = StopBits.One
                };

                _serialPort.Open();

                // Actualizar UI
                BtnConnect.Content = "Desconectar";
                BtnSendFrame.IsEnabled = true;
                TxtStatus.Text = $"Conectado a {_serialPort.PortName} a {_serialPort.BaudRate} baudios.";
                TxtStatus.Foreground = System.Windows.Media.Brushes.Green;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al conectar el puerto: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnSendFrame_Click(object sender, RoutedEventArgs e)
        {
            if (_serialPort != null && _serialPort.IsOpen)
            {
                try
                {
                    // Leer la trama del TextBox y asegurar el terminador \r\n al final[cite: 7]
                    string frameToSend = TxtFrame.Text.Trim() + "\r\n";

                    _serialPort.Write(frameToSend);

                    // Pequeña confirmación visual
                    TxtStatus.Text = $"Señal enviada a las {DateTime.Now:HH:mm:ss}";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al enviar la trama: {ex.Message}");
                }
            }
        }
    }
}