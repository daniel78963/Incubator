using Incubator.Application.Interfaces;
using Incubator.Infrastructure.Services;
using System.IO.Ports;
using System.Windows;

namespace Incubator.GenerateSignal
{
    public partial class MainWindow : Window
    {
        private SerialPort? _serialPort1;
        private SerialPort? _serialPort2;

        private readonly IEncryptionService _encryptionService = new AesEncryptionService();

        public MainWindow()
        {
            InitializeComponent();
            LoadConfiguration();
        }

        private void LoadConfiguration()
        {
            // Cargar los puertos COM instalados en el equipo
            string[] ports = SerialPort.GetPortNames();
            CmbPort1.ItemsSource = ports;
            if (ports.Length > 0) CmbPort1.SelectedIndex = 0;

            // Cargar los Baud Rates comunes
            CmbBaudRate1.ItemsSource = new int[] { 9600, 19200, 38400, 115200 };
            CmbBaudRate1.SelectedItem = 9600;

            CmbPort2.ItemsSource = ports;
            if (ports.Length > 0) CmbPort2.SelectedIndex = 0;

            // Cargar los Baud Rates comunes
            CmbBaudRate2.ItemsSource = new int[] { 9600, 19200, 38400, 115200 };
            CmbBaudRate2.SelectedItem = 9600;
        }

        private void BtnConnect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_serialPort1 != null && _serialPort1.IsOpen)
                {
                    // Lógica para desconectar
                    _serialPort1.Close();
                    BtnConnect.Content = "Conectar";
                    BtnSendFrame.IsEnabled = false;
                    TxtStatus.Text = "Desconectado";
                    TxtStatus.Foreground = System.Windows.Media.Brushes.Gray;
                    return;
                }

                if (CmbPort1.SelectedItem == null)
                {
                    MessageBox.Show("Por favor selecciona un puerto COM.");
                    return;
                }

                // Configurar y abrir el puerto
                _serialPort1 = new SerialPort(CmbPort1.SelectedItem.ToString()!)
                {
                    BaudRate = (int)CmbBaudRate1.SelectedItem,
                    Parity = Parity.None,
                    DataBits = 8,
                    StopBits = StopBits.One
                };

                _serialPort1.Open();

                if (CmbPort2.SelectedItem != null)
                {
                    _serialPort2 = new SerialPort(CmbPort2.SelectedItem.ToString()!, 9600, Parity.None, 8, StopBits.One);
                    _serialPort2.Open();
                }

                // Actualizar UI
                BtnConnect.Content = "Desconectar";
                BtnSendFrame.IsEnabled = true;
                BtnSendBoth.IsEnabled = true;
                //TxtStatus.Text = $"Conectado a {_serialPort.PortName} a {_serialPort.BaudRate} baudios.";
                TxtStatus.Text = "Conectados y listos para emitir.";
                TxtStatus.Foreground = System.Windows.Media.Brushes.Green;
            }
            catch (Exception ex)
            {
                //MessageBox.Show($"Error al conectar el puerto: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                MessageBox.Show($"Error al conectar puertos: {ex.Message}");
            }
        }

        private void BtnSendFrame_Click(object sender, RoutedEventArgs e)
        {
            if (_serialPort1 != null && _serialPort1.IsOpen)
            {
                try
                {
                    // Leer la trama del TextBox y asegurar el terminador \r\n al final[cite: 7]
                    string frameToSend = TxtFrame.Text.Trim() + "\r\n";

                    _serialPort1.Write(frameToSend);

                    // Pequeña confirmación visual
                    TxtStatus.Text = $"Señal enviada a las {DateTime.Now:HH:mm:ss}";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al enviar la trama: {ex.Message}");
                }
            }
        }

        private void BtnSendBoth_Click(object sender, RoutedEventArgs e)
        {
            string baseFrame = TxtFrame.Text.Trim(); // Ej: A1A462213082703230013

            //// Asignamos seriales distintos a la misma trama base para verlos en el receptor[cite: 6]
            //string framePort1 = baseFrame.Substring(0, 21) + "9999" + baseFrame.Substring(25) + "\r\n"; //[cite: 7]
            //string framePort2 = baseFrame.Substring(0, 21) + "5555" + baseFrame.Substring(25) + "\r\n"; //[cite: 7]

            //// Disparamos en paralelo simulando llegada simultánea
            //if (_serialPort1?.IsOpen == true) _serialPort1.Write(framePort1);
            //if (_serialPort2?.IsOpen == true) _serialPort2.Write(framePort2);

            // Seriales distintos para la simulación
            string rawFrame1 = baseFrame.Substring(0, 21) + "9999" + baseFrame.Substring(25);
            string rawFrame2 = baseFrame.Substring(0, 21) + "5555" + baseFrame.Substring(25);

            // Evaluamos individualmente si se encripta
            string framePort1 = ChkEncrypt1.IsChecked == true
                ? $"ENC|{_encryptionService.Encrypt(rawFrame1)}\r\n"
                : $"{rawFrame1}\r\n"; //[cite: 7]

            string framePort2 = ChkEncrypt2.IsChecked == true
                ? $"ENC|{_encryptionService.Encrypt(rawFrame2)}\r\n"
                : $"{rawFrame2}\r\n"; //[cite: 7]

            // Disparamos en paralelo simulando llegada simultánea
            if (_serialPort1?.IsOpen == true) _serialPort1.Write(framePort1);
            if (_serialPort2?.IsOpen == true) _serialPort2.Write(framePort2);

            TxtStatus.Text = $"Señales concurrentes enviadas a las {DateTime.Now:HH:mm:ss}";
        }
    }
}