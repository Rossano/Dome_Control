using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO.Ports;

namespace Dome_Control
{
    /// <summary>
    /// Interaction logic for COM_Window.xaml
    /// </summary>
    public partial class COM_Window : Window
    {
        MainWindow parent;
        public SerialPort comPort { get; set; }
        public COM_Window(Window p)
        {
            parent = (MainWindow)p;
            InitializeComponent();
            comPort = new SerialPort();
            comPort.BaudRate = 19200;
            comPort.Handshake = Handshake.None;
            comPort.StopBits = StopBits.One;
            comPort.Parity = Parity.None;
            NoneParityRB.IsChecked = true;
            NoneHandshakeRB.IsChecked = true;
            baud19200RB.IsChecked = true;
            StopBit1RB.IsChecked = true;
        }

        private void EvenParityRB_Checked(object sender, RoutedEventArgs e)
        {
            comPort.Parity = Parity.Even;
        }

        private void OddParityRB_Checked(object sender, RoutedEventArgs e)
        {
            comPort.Parity = Parity.Odd;
        }

        private void NoneParityRB_Checked(object sender, RoutedEventArgs e)
        {
            comPort.Parity = Parity.None;
        }

        private void NoneHandshakeRB_Checked(object sender, RoutedEventArgs e)
        {
            comPort.Handshake = Handshake.None;
        }

        private void XONXOFFHandshakeRB_Checked(object sender, RoutedEventArgs e)
        {
            comPort.Handshake = Handshake.XOnXOff;
        }

        private void StopBitORB_Checked(object sender, RoutedEventArgs e)
        {
            comPort.StopBits = StopBits.None;
        }

        private void StopBit1RB_Checked(object sender, RoutedEventArgs e)
        {
            comPort.StopBits = StopBits.One;
        }

        private void StopBit1_5RB_Checked(object sender, RoutedEventArgs e)
        {
            comPort.StopBits = StopBits.OnePointFive;
        }

        private void StopBits2RB_Checked(object sender, RoutedEventArgs e)
        {
            comPort.StopBits = StopBits.Two;
        }

        private void baud9600RB_Checked(object sender, RoutedEventArgs e)
        {
            comPort.BaudRate = 9600;
        }

        private void baud14200RB_Checked(object sender, RoutedEventArgs e)
        {
            comPort.BaudRate = 14200;
        }

        private void baud19200RB_Checked(object sender, RoutedEventArgs e)
        {
            comPort.BaudRate = 19200;
        }

        private void baud28800RB_Checked(object sender, RoutedEventArgs e)
        {
            comPort.BaudRate = 28800;
        }

        private void baud57600RB_Checked(object sender, RoutedEventArgs e)
        {
            comPort.BaudRate = 57600;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            //parent._peltier._avr.setStopbits(comPort.StopBits);
            //parent._peltier._avr.setParity(comPort.Parity);
            //parent._peltier._avr.setHandshake(comPort.Handshake);
            //parent._peltier._avr.setBaudrate(comPort.BaudRate);
            ((App)(Application.Current))._peltier._avr.setStopbits(comPort.StopBits);
            ((App)(Application.Current))._peltier._avr.setParity(comPort.Parity);
            ((App)(Application.Current))._peltier._avr.setHandshake(comPort.Handshake);
            ((App)(Application.Current))._peltier._avr.setBaudrate(comPort.BaudRate);

            //parent.avr.stopBits = comPort.StopBits;
            //parent.avr.parity = comPort.Parity;
            //parent.avr.handshake = comPort.Handshake;
            //parent.avr.baudRate = comPort.BaudRate;
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
