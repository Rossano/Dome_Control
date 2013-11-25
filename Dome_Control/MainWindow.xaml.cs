﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Reflection;
using System.Globalization;
using System.Threading;
using System.Windows.Threading;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using ASCOM_Telescope_ns;
using Xceed.Wpf;
using Xceed.Wpf.Toolkit;
using Xceed.Wpf.Toolkit.Core;

namespace Dome_Control
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        #region Members

        private Angle position { get; set; }
        private ASCOM_Telescope Telescope;
        private uint moveStep { get; set; }
        private string revisionString = "0.1";
        private bool isArduino;
        private bool isArduinoBootloader;
        private string AVRBootLoader_COM;
        private bool isAuto = false;
        private uint TelescopeCheckInterval_s = 1;
        private const uint ReadingSample = 5;
        private string[] HelpPaths;
        private const string DefaultChm = "Dome_Control_Help_en-US.chm";
        private string chmFullFileName;
        private DispatcherTimer mainTimer = new DispatcherTimer();
        private DebugWindow debugWnd = null;
        private enum _status
        {
            TURN_LEFT,
            TURN_RIGHT,
            NO_TURN
        };
        private double StartTime;

        #endregion

        public MainWindow()
        {
            InitializeComponent();
            try
            {
                //
                // Read the Configuration file
                //
                configXml = "../../config.xml";
                if (File.Exists(configXml))
                {
                    // Development Version
                    ReadConfiguration();
                }
                else
                {
                    configXml = "config.xml";
                    if (File.Exists(configXml))
                    {
                        // Released Version
                        ReadConfiguration();
                    }
                }
                //
                //  Using XML for language
                //
                //  UpdateDialogs();
                //
                //  Using Resources for language
                //
                UpdateGUI();
            }
            catch (Exception ex)
            {
                ErrDlg("Error Reading Configurations\n", ex);
            }
            //
            //  If it is AVR Bootloader help to get into the AVR user code via avrdude
            //
            if (!isArduinoBootloader)
            {
//                launchAVRDude();
//                System.Threading.Thread.Sleep(3000);
            }
//            this.Width = 820;
//            this.Height = 540;
  
            //autoGroupBox.IsEnabled = isAuto;
            //PWM_Value = (int)PWMSlider.Value;
            //autoGroupBox.Visibility = Visibility.Collapsed;
            //autoGroupBox.IsEnabled = false;
            //mainGraph = new ZedGraphControl();
            foreach (string s in System.IO.Ports.SerialPort.GetPortNames())
            {
                //ComPortComboBox.Items.Add(s);
                AVRCOMListBox.Items.Add(s);
            }
            mainTimer.Interval = TimeSpan.FromSeconds(TelescopeCheckInterval_s);
            mainTimer.IsEnabled = true;
            mainTimer.Tick += mainTimer_Tick;
            mainTimer.Stop();
            StartTime = Environment.TickCount;
            try
            {
                ConnectionImage.Source = new BitmapImage(new Uri(@"/images/DisconnectedImg.png", UriKind.Relative));
                Connected_Label = Properties.Resources.StatusBar_Disconnected;
                ConnectionStatusLabel.Content = Connected_Label;                
                //ConnectioStatusLabel.Content = Disconnected_Label;                
            }
            catch
            { }
            //  Get the Full help file name
            //bool found = false;
            //HelpPaths = new string[3] { "/Help/", "Help/", "../../Help/" };
            //foreach (string s in HelpPaths)
            //{
            //    if (File.Exists(s + DefaultChm))
            //    {
            //        found = true;
            //        break;
            //    }
            //}
            //if (!found)
            //{
            //    ErrDlg(Properties.Resources.Error_HelpFileNotFound, (Exception)null);
            //}
        }

        #region Event Handlers

        /// <summary>
        /// Main Timer Tick Event Handler.
        /// It Fires the new firing interval and, it display the connection elapsed time and it checks the connection
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void mainTimer_Tick(object sender, EventArgs e)
        {
            //  Gets the Telescope/Dome position and plots them
            try
            {
                DomePosition.Content = ((App)(System.Windows.Application.Current))._Dome_uC.GetDomePosition();
            }
            catch (Exception ex)
            {
                ErrDlg("Error getting Dome Position", ex);
            }
            if (Telescope.isConnected())
            {
                TelescopePos.Content = Telescope.getAzimut();
            }
            //  Checks up the AVR connection and plot an error if it is found disconnected
            if (!((App)(System.Windows.Application.Current))._Dome_uC.GetAck())
            {
                ConnectionStatusLabel.Content = Disconnected_Label;
                ErrDlg("Error: AVR disconnected", new Exception());
                mainTimer.Stop();                
            }
        }

        /// <summary>
        /// AVR Serial Port Selection Box Event Handler.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SelectionChangedEventArgs"/> instance containing the event data.</param>
        private void AVRBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //  Get the last selection for the List Box
            AVR_COM_Name = (string)AVRCOMListBox.SelectedItem;
        }

        /// <summary>
        /// Serial Port Baudrate Change Event Handler.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SelectionChangedEventArgs"/> instance containing the event data.</param>
        private void AVRBaudrate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                //
                //  Gets the selected Index of the Baudrate radiobox
                //  And decode the index to set the baudrate accordingly
                //
                int i = AVRBaudrateListBox.SelectedIndex;
                switch (i)
                {
                    case 0: ((App)(System.Windows.Application.Current))._Dome_uC._avr.setBaudrate(9600); break;
                    case 1: ((App)(System.Windows.Application.Current))._Dome_uC._avr.setBaudrate(19200); break;
                    case 2: ((App)(System.Windows.Application.Current))._Dome_uC._avr.setBaudrate(38400); break;
                    case 3: ((App)(System.Windows.Application.Current))._Dome_uC._avr.setBaudrate(57600); break;
                    case 4: ((App)(System.Windows.Application.Current))._Dome_uC._avr.setBaudrate(112200); break;
                    default: ((App)(System.Windows.Application.Current))._Dome_uC._avr.setBaudrate(9600); break;
                }

            }
            catch (Exception ex)
            {
                ErrDlg("Invalid Baudrate Selection", (Exception)null);
            }
        }

        /// <summary>
        /// Connection Button Click Event Handler.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void AVRConnectButton_Click(object sender, RoutedEventArgs e)
        {
            //  Initialize the PWM GUI element
//            PWM_Value = (int)PWMSlider.Value;
            //
            //  Check if the AVR is already connected
            //
            if (AVRConnectButton.Content.Equals(Properties.Resources.Button_Connect))
            {
                //  Connect the AVR

                //
                //  Initialize the AVR element
                //
                if (((App)(System.Windows.Application.Current))._Dome_uC == null)
                {
                    ((App)(System.Windows.Application.Current))._Dome_uC = new ArduinoDome_ns.ArduinoDome(AVR_COM_Name, isArduinoBootloader);
                }
                //
                //  Read the Firmware Versions and show it on a MessageBox
                //
                string ver = ((App)(System.Windows.Application.Current))._Dome_uC.GetVersion();
                //  Parse the received string to the the useful information only
                char[] delim = { ':', ' ', '\n' };
                string[] tokens = ver.Split(delim);
                int i = 0;
                foreach (string tok in tokens)
                {
                    if (tok.Equals("Firmware")) break;
                    i++;
                }
                //  Display the Firmware version
                ver = "";
                for (int j = 0; j < 7; j++)
                {
                    ver += tokens[i + j] + " ";
                }
                //  Show the AVR Firmware version into a message box
                System.Windows.MessageBox.Show(ver, "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                //
                //  Display the Control Tab into the GUI
                //
                MainTab.SelectedIndex++;
                //
                //  Start the Timers
                //
                mainTimer.Start();
//                graphTimer.Start();
                //  Initialize the Peltier 
                ((App)(System.Windows.Application.Current))._Dome_uC.PeltierInit();
                //  Change the StatusBar Icon
                ConnectionImage.Source = new BitmapImage(new Uri(@"/images/ConnectedImg.png", UriKind.Relative));
                //  Change the StatusBar labels
                ConnectionStatusLabel.Content = Properties.Resources.StatusBar_Connected;
                StatusBar_Version.Content = "FW Ver. : " + tokens[i + 1] + " " + tokens[i + 2];
                AVRConnectButton.Content = Properties.Resources.Button_Disconnect;
            }
            else
            {
                //
                //  Disconnect the AVR Device
                //
                ((App)(System.Windows.Application.Current))._Dome_uC.Disconnect();
                //  Stops the timers
                mainTimer.Stop();
//                graphTimer.Stop();
                //  Change the StatusBar Icon
                ConnectionImage.Source = new BitmapImage(new Uri(@"/images/DisconnectedImg.png", UriKind.Relative));
                //  Change the StatusBar labels
                ConnectionStatusLabel.Content = Properties.Resources.StatusBar_Disconnected;
                StatusBar_Version.Content = "FW Ver. : ";
                AVRConnectButton.Content = Properties.Resources.Button_Connect;
                //
                //  If it is AVR Bootloader help to get into the AVR user code via avrdude
                //
                if (!isArduinoBootloader)
                {
                    launchAVRDude();
                    System.Threading.Thread.Sleep(3000);
                }
            }
        }

        /// <summary>
        /// New FW Click event handler.
        /// This function show a new custom window to download a new FW into the microcontroler.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void NewFW_Click(object sender, RoutedEventArgs e)
        {
            NewFW_Download_Window wnd = new NewFW_Download_Window(isArduinoBootloader);
            wnd.ShowDialog();
        }

        private void Debug_Click(object sender, RoutedEventArgs e)
        {
            //if (debugWnd == null)
            {
                debugWnd = new DebugWindow(this);
            }
            debugWnd.Show();
        }

        private void AVRCOMListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void AVRBaudrateListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void AVRParity_None_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AVRParity_Even_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AVRParity_Mark_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AVRParity_Odd_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AVRParity_Space_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AVRStopbitNone_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AVRStopbit1_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AVRStopbit15_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AVRStopbit2_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AVRHandshake_None_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AVRHandshake_RTS_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AVRHandshake_RTSX_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AVRHandshake_XonXoff_Click(object sender, RoutedEventArgs e)
        {

        }

        #endregion
        
        #region Methods

        /// <summary>
        /// Window Close control Event Handler.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ExecutedRoutedEventArgs"/> instance containing the event data.</param>
        private void Close_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        /// <summary>
        /// Graphic Window size change Event Handler.
        /// It repaint the graphic windows adapting ZedGraph
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SizeChangedEventArgs"/> instance containing the event data.</param>
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
  //          Graph.SetSize((int)mainGraphHost.Child.Width, (int)mainGraphHost.Child.Height); //(int)dummy.ActualHeight);
        }


        /// <summary>
        /// Generic function to display an Error on a Message Box.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="ex">The ex.</param>
        private void ErrDlg(string str, Exception ex)
        {
            System.Windows.MessageBox.Show(str + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void ASCOMConnectButton_Click(object sender, RoutedEventArgs e)
        {
            Telescope = new ASCOM_Telescope();
            //ErrDlg("Telescope: " + Telescope.geetDeclination().ToString(), new Exception());
            ASCOMConnectButton.Content = Properties.Resources.Button_Disconnect;
        }

        /// <summary>
        /// Launches the avrdude.
        /// This is need to push the AR bootloader to end astarting the application code
        /// </summary>
        /// <exception cref="System.Exception">AVRDUDE fails, impossible to connect to AVR</exception>
        protected void launchAVRDude()
        {
            //  Setting up the the data structure to launch the external executable
            ProcessStartInfo avrdude = new ProcessStartInfo();

            avrdude.CreateNoWindow = false;
            avrdude.UseShellExecute = false;
            string BootLoader_Protocol;

            //  If the AVR has an Arduino bootloader set the protocol as arduino else as avr109
            if (isArduinoBootloader) BootLoader_Protocol = "arduino";
            else BootLoader_Protocol = "avr109";

            //  Defines the external executable name and process mode
            avrdude.WindowStyle = ProcessWindowStyle.Hidden;
            avrdude.FileName = "avrdude.exe";
            //  Defines the arguments
            avrdude.Arguments = string.Format("-c {0} -p m32u4 -P {1} -U lfuse:r:0xfc:m -U hfuse:r:0xd0:m -U efuse:r:0xf3:m", BootLoader_Protocol, AVRBootLoader_COM);
            //  Launch the executable and wait until has not ended
            try
            {
                using (Process exeProc = Process.Start(avrdude))
                {
                    exeProc.WaitForExit();
                }
            }
            catch(Exception ex)
            {
                //  In case of error report it to the user
               // throw new Exception("AVRDUDE fails, impossible to connect to AVR");
                ErrDlg(Properties.Resources.Error_AVRDUDE,ex);
            }
        }

        #endregion


        private void SingleDomeControl_Right_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void SingleDomeControl_Right_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void SingleDomeControl_Left_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void SingleDomeControl_Left_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void MultiDomeControl_Left_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MultiDomeControl_Right_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
