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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Reflection;
using System.Globalization;
using System.Threading;
using System.Windows.Threading;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Xml.Linq;
//using ASCOM_Telescope_ns;
using ASCOM.Arduino;
using Arduino.Dome;
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
        //private ASCOM_Telescope Telescope;
        public Dome _dome;
        private uint moveStep { get; set; }
        private string revisionString = "0.1";
        private bool isArduino;
        private bool isArduinoBootloader;
        private string AVRBootLoader_COM;
        private bool isAuto = false;
        private uint TelescopeCheckInterval_s = 1;
        private int SleewingSleepTime = 100;
        private uint _motor_accel_time = 5;
        private uint _motor_rpm = 1400;
        private uint _enc_resolution;
        private double _gear_ratio;
        private const uint ReadingSample = 5;
        private string[] HelpPaths;
        private const string DefaultChm = "Dome_Control_Help_en-US.chm";
        private string chmFullFileName;
        private DispatcherTimer mainTimer = new DispatcherTimer();
        private DebugWindow debugWnd = null;
        private enum Status
        {
            TURN_LEFT,
            TURN_RIGHT,
            NO_TURN
        };
        private Status _status;
        private double StartTime;
        private bool SingleLeftButtonPressed = false;
        private bool SingleRightButtonPressed = false;
        private string configFilename;

        #endregion

        /// <summary>
        /// Load a resource WPF-BitmapImage (png, bmp, ...) from embedded resource defined as 'Resource' not as 'Embedded resource'.
        /// </summary>
        /// <param name="pathInApplication">Path without starting slash</param>
        /// <param name="assembly">Usually 'Assembly.GetExecutingAssembly()'. If not mentionned, I will use the calling assembly</param>
        /// <returns></returns>
        public static BitmapImage LoadBitmapFromResource(string pathInApplication, Assembly assembly = null)
        {
            if (assembly == null)
            {
                assembly = Assembly.GetCallingAssembly();
            }

            if (pathInApplication[0] == '/')
            {
                pathInApplication = pathInApplication.Substring(1);
            }
            return new BitmapImage(new Uri(@"pack://application:,,,/" + assembly.GetName().Name + ";component/" + pathInApplication, UriKind.Absolute));
        }

        public MainWindow()
        {
            InitializeComponent();
            try
            {
                //Image1.Source = new BitmapImage(new Uri(@"././images/LeftArrow.png", UriKind.Relative));
                //Image2.Source = new BitmapImage(new Uri(@"././images/RightArrow.png", UriKind.Relative));
                //Image3.Source = new BitmapImage(new Uri(@"images/LeftArrow.png", UriKind.Relative));
                //Image4.Source = new BitmapImage(new Uri(@"./images/RightArrow.png", UriKind.Relative));
                //Image5.Source = new BitmapImage(new Uri(@"./images/Stop.png", UriKind.Relative));
                //Image1.Source = LoadBitmapFromResource("Images/LeftArrow.png");
                Uri uri = new Uri("pack://application:,,,/Images/LeftArrow.png");
                Image1.Source = new BitmapImage(uri);
                Image2.Source = new BitmapImage(new Uri("pack://application:,,,/Images/RightArrow.png"));
                Image4.Source = Image1.Source;
                Image5.Source = new BitmapImage(new Uri("pack://application:,,,,/Images/Stop.png"));
            }
            catch { }
            _status = Status.NO_TURN;
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
                configFilename = configXml;
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
                launchAVRDude();
                System.Threading.Thread.Sleep(3000);
            }
//            this.Width = 820;
//            this.Height = 540;
  
            //autoGroupBox.IsEnabled = isAuto;
            //PWM_Value = (int)PWMSlider.Value;
            //autoGroupBox.Visibility = Visibility.Collapsed;
            //autoGroupBox.IsEnabled = false;
            //mainGraph = new ZedGraphControl();
            //foreach (string s in System.IO.Ports.SerialPort.GetPortNames())
            //{
            //    //ComPortComboBox.Items.Add(s);
            //    AVRCOMListBox.Items.Add(s);
            //}
            mainTimer.Interval = TimeSpan.FromSeconds(TelescopeCheckInterval_s);
            mainTimer.IsEnabled = true;
            mainTimer.Tick += mainTimer_Tick;
            mainTimer.Stop();
            StartTime = Environment.TickCount;
            try
            {
                ConnectionImage.Source = new BitmapImage(new Uri(@"./images/DisconnectedImg.png", UriKind.Relative));
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
            long pos;
            double foo = 0;
            //  Gets the Telescope/Dome position and plots them
            try
            {
                foo = _dome.Azimuth;
                pos = Convert.ToInt64(foo);
                DomePosition.Content = (360 * foo / _enc_resolution / _gear_ratio).ToString("F6");
                writeLastDomePosition(pos);
            }
            catch (Exception ex)
            {
                ErrDlg("Error getting Dome Position", ex);
            }
            //if (_dome._telescope.isConnected())
            if(_dome._telescope.Connected)
            {
                //TelescopePos.Content = _dome._telescope.getAzimut();
                TelescopePos.Content = _dome._telescope.Azimuth;
                AngleDiff.Content = (360 * foo / _enc_resolution / _gear_ratio - _dome._telescope.Azimuth).ToString("F6");
            }
            //  Checks up the AVR connection and plot an error if it is found disconnected
            if(!_dome.Connected)
            //if (!((App)(System.Windows.Application.Current))._Dome_uC.GetAck())
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
                    //case 0: ((App)(System.Windows.Application.Current))._Dome_uC._avr.setBaudrate(9600); break;
                    //case 1: ((App)(System.Windows.Application.Current))._Dome_uC._avr.setBaudrate(19200); break;
                    //case 2: ((App)(System.Windows.Application.Current))._Dome_uC._avr.setBaudrate(38400); break;
                    //case 3: ((App)(System.Windows.Application.Current))._Dome_uC._avr.setBaudrate(57600); break;
                    //case 4: ((App)(System.Windows.Application.Current))._Dome_uC._avr.setBaudrate(112200); break;
                    //default: ((App)(System.Windows.Application.Current))._Dome_uC._avr.setBaudrate(9600); break;
                }

            }
            catch (Exception ex)
            {
                ErrDlg("Invalid Baudrate Selection", ex);
            }
        }

//        /// <summary>
//        /// Connection Button Click Event Handler.
//        /// </summary>
//        /// <param name="sender">The source of the event.</param>
//        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
//        private void AVRConnectButton_Click(object sender, RoutedEventArgs e)
//        {
//            //  Initialize the PWM GUI element
////            PWM_Value = (int)PWMSlider.Value;
//            //
//            //  Check if the AVR is already connected
//            //
//            if (AVRConnectButton.Content.Equals(Properties.Resources.Button_Connect))
//            {
//                //  Connect the AVR

//                //
//                //  Initialize the AVR element
//                //
//                if (_dome == null)
//                //if (((App)(System.Windows.Application.Current))._Dome_uC == null)
//                {
//                    //((App)(System.Windows.Application.Current))._Dome_uC = new Arduino.Dome.ArduinoDome(AVR_COM_Name, isArduinoBootloader);
//                    _dome = new Dome();
//                    //_dome = new Dome(AVR_COM_Name, isArduinoBootloader);
//                }
//                //
//                //  Read the Firmware Versions and show it on a MessageBox
//                //
//                string ver = _dome.DriverVersion;// ((App)(System.Windows.Application.Current))._Dome_uC.GetVersion();
//                //  Parse the received string to the the useful information only
//                char[] delim = { ':', ' ', '\n' };
//                string[] tokens = ver.Split(delim);
//                int i = 0;
//                foreach (string tok in tokens)
//                {
//                    if (tok.Equals("Firmware")) break;
//                    i++;
//                }
//                //  Display the Firmware version
//                ver = "";
//                for (int j = 0; j < 7; j++)
//                {
//                    ver += tokens[i + j] + " ";
//                }
//                //  Show the AVR Firmware version into a message box
//                System.Windows.MessageBox.Show(ver, "Info", MessageBoxButton.OK, MessageBoxImage.Information);
//                //
//                //  Display the Control Tab into the GUI
//                //
//                MainTab.SelectedIndex++;
//                //
//                //  Start the Timers
//                //
//                mainTimer.Start();
////                graphTimer.Start();
                
//                //  Change the StatusBar Icon
//                ConnectionImage.Source = new BitmapImage(new Uri(@"/images/ConnectedImg.png", UriKind.Relative));
//                //  Change the StatusBar labels
//                ConnectionStatusLabel.Content = Properties.Resources.StatusBar_Connected;
//                StatusBar_Version.Content = "FW Ver. : " + tokens[i + 1] + " " + tokens[i + 2];
//                AVRConnectButton.Content = Properties.Resources.Button_Disconnect;
//            }
//            else
//            {
//                //
//                //  Disconnect the AVR Device
//                //
//                _dome.Connected = false;
//                //((App)(System.Windows.Application.Current))._Dome_uC.Disconnect();
//                //  Stops the timers
//                mainTimer.Stop();
////                graphTimer.Stop();
//                //  Change the StatusBar Icon
//                ConnectionImage.Source = new BitmapImage(new Uri(@"/images/DisconnectedImg.png", UriKind.Relative));
//                //  Change the StatusBar labels
//                ConnectionStatusLabel.Content = Properties.Resources.StatusBar_Disconnected;
//                StatusBar_Version.Content = "FW Ver. : ";
//                AVRConnectButton.Content = Properties.Resources.Button_Connect;
//                //
//                //  If it is AVR Bootloader help to get into the AVR user code via avrdude
//                //
//                if (!isArduinoBootloader)
//                {
//                    launchAVRDude();
//                    System.Threading.Thread.Sleep(3000);
//                }
//            }
//        }

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

        /// <summary>
        /// SingleDomeControl_Left control Click event Handler.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void SingleDomeControl_Left_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                /// Checks if the button has already been pressed, if yes stops the dome, else
                /// turn on the left
                if (SingleLeftButtonPressed)
                {
                    /// Checks if the dome is already turning, if yes stop it else do nothing
                    if (_status == Status.TURN_LEFT)
                    {
                        _dome.Stop();
                        _status = Status.NO_TURN;
                        SingleLeftButtonPressed = false;
                    }
                }
                else
                {
                    /// Checks if the dome is stopped, if yes make it turn on left else do nothing
                    if (_status == Status.NO_TURN)
                    {
                        _dome.TurnLeft();
                        _status = Status.TURN_LEFT;
                        SingleLeftButtonPressed = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrDlg("Error slewing/stopping on left ", ex);
            }
        }

        /// <summary>
        /// SingleDomeControl_Right control Click event Handler.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void SingleDomeControl_Right_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                /// Checks if the button has already been pressed, if yes stops the dome, else
                /// turn on the right.
                if (SingleRightButtonPressed)
                {
                    /// Checks if the dome is already turning, if yes stop it else do nothing
                    if (_status == Status.TURN_RIGHT)
                    {
                        _dome.Stop();
                        _status = Status.NO_TURN;
                        SingleRightButtonPressed = false;
                    }
                }
                else
                {
                    /// Checks if the dome is stopped, if yes maje it turns on right else do nothing
                    if (_status == Status.NO_TURN)
                    {
                        _dome.TurnRight();
                        _status = Status.TURN_RIGHT;
                        SingleRightButtonPressed = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrDlg("Error slewing/stopping on right ", ex);
            }
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
            try
            {
                //
                //  Check if Dome is already connected
                //
                if (ASCOMConnectButton.Content.Equals(Properties.Resources.Button_Connect))
                {
                    //  Connect the AVR
                    //
                    //  Initialize the AVR element
                    //
                    if (_dome == null)
                    {
                        _dome = new Dome();
                    }
                    _dome.motor_accelleration_time = _motor_accel_time;
                    _dome.dome_gear_ratio = _gear_ratio;
                    _dome.dome_angular_speed = 2 * Math.PI * _motor_rpm / _gear_ratio / 60;
                    _dome.encoder_resolution = _enc_resolution;
                    _dome.configureFirmware();
                    //
                    //  Gets connection information to the Arduino and the telescope
                    //
                    //_dome.SetupDialog();
                    //
                    //  Connect the Arduino
                    //
                    //                _dome._arduino.Connect();
                    //
                    //  Read the Firmware Versions and show it on a MessageBox
                    //
                    string ver = _dome._arduino.GetVersion();//DriverVersion;// ((App)(System.Windows.Application.Current))._Dome_uC.GetVersion();
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
                    //                MainTab.SelectedIndex++;
                    //
                    //  Start the Timers
                    //
                    mainTimer.Start();
                    //                graphTimer.Start();

                    //  Change the StatusBar Icon
                    ConnectionImage.Source = new BitmapImage(new Uri(@"./images/ConnectedImg.png", UriKind.Relative));
                    //  Change the StatusBar labels
                    ConnectionStatusLabel.Content = Properties.Resources.StatusBar_Connected;
                    StatusBar_Version.Content = "FW Ver. : " + tokens[i + 1] + " " + tokens[i + 2];
                    ASCOMConnectButton.Content = Properties.Resources.Button_Disconnect;
                }
                else
                {
                    //
                    //  Disconnect the AVR Device
                    //
                    _dome.Connected = false;
                    //((App)(System.Windows.Application.Current))._Dome_uC.Disconnect();
                    //  Stops the timers
                    mainTimer.Stop();
                    //                graphTimer.Stop();
                    //  Change the StatusBar Icon
                    ConnectionImage.Source = new BitmapImage(new Uri(@"./images/DisconnectedImg.png", UriKind.Relative));
                    //  Change the StatusBar labels
                    ConnectionStatusLabel.Content = Properties.Resources.StatusBar_Disconnected;
                    StatusBar_Version.Content = "FW Ver. : ";
                    ASCOMConnectButton.Content = Properties.Resources.Button_Connect;
                    //
                    //  If it is AVR Bootloader help to get into the AVR user code via avrdude
                    //
                    if (!isArduinoBootloader)
                    {
                        launchAVRDude();
                        System.Threading.Thread.Sleep(3000);
                    }
                }

                //_telescope = new ASCOM_Telescope();
                //ErrDlg("Telescope: " + Telescope.geetDeclination().ToString(), new Exception());
                ASCOMConnectButton.Content = Properties.Resources.Button_Disconnect;
            }
            catch (Exception ex)
            {
                ErrDlg("Error Opening ASCOM or Arduino", ex);
            }
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
            //if (_status == Status.NO_TURN)
            //{
            //    try
            //    {
            //        _dome.TurnRight();
            //        _status = Status.TURN_RIGHT;
            //    }
            //    catch (Exception ex)
            //    {
            //        ErrDlg("Error Slewing Right the Dome", ex);
            //    }
            //}
            try
            {
                /// Checks if the button has already been pressed, if yes stops the dome, else
                /// turn on the left
                if (SingleRightButtonPressed)
                {
                    /// Checks if the dome is already turning, if yes stop it else do nothing
                    if (_status == Status.TURN_RIGHT)
                    {
                        _dome.Stop();
                        _status = Status.NO_TURN;
                    }
                }
                else
                {
                    /// Checks if the dome is stopped, if yes make it turn on left else do nothing
                    if (_status == Status.NO_TURN)
                    {
                        _dome.TurnRight();
                        _status = Status.TURN_RIGHT;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrDlg("Error slewing/stopping on left ", ex);
            }
        }

        private void SingleDomeControl_Right_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_status == Status.TURN_RIGHT)
            {
                try
                {
                    _dome.Stop();
                    _status = Status.NO_TURN;
                }
                catch (Exception ex)
                {
                    ErrDlg("Error Stopping slewing the Dome", ex);
                }
            }
        }

        private void SingleDomeControl_Left_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //if (_status == Status.NO_TURN)
            //{
            //    try
            //    {
            //        _dome.TurnLeft();
            //        _status = Status.TURN_LEFT;
            //    }
            //    catch (Exception ex)
            //    {
            //        ErrDlg("Error slewing Left the Dome", ex);
            //    }
            //}
            try
            {
                /// Checks if the button has already been pressed, if yes stops the dome, else
                /// turn on the left
                if (SingleLeftButtonPressed)
                {
                    /// Checks if the dome is already turning, if yes stop it else do nothing
                    if (_status == Status.TURN_LEFT)
                    {
                        _dome.Stop();
                        _status = Status.NO_TURN;
                    }
                }
                else
                {
                    /// Checks if the dome is stopped, if yes make it turn on left else do nothing
                    if (_status == Status.NO_TURN)
                    {
                        _dome.TurnLeft();
                        _status = Status.TURN_LEFT;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrDlg("Error slewing/stopping on left ", ex);
            }
        }

        private void SingleDomeControl_Left_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_status == Status.TURN_LEFT)
            {
                try
                {
                    _dome.Stop();
                    _status = Status.NO_TURN;
                }
                catch (Exception ex)
                {
                    ErrDlg("Error stopping slweing the Dome", ex);
                }
            }
        }

        private void MultiDomeControl_Left_Click(object sender, RoutedEventArgs e)
        {
            if (_status == Status.NO_TURN)
            {
                try
                {
                    _status = Status.TURN_LEFT;
                    _dome.SlewToAzimuth(_dome.Azimuth - 2.0);
                    _status = Status.NO_TURN;
                }
                catch (Exception ex)
                {
                    ErrDlg("Error slewing multiple step left", ex);
                }
            }
        }

        private void MultiDomeControl_Right_Click(object sender, RoutedEventArgs e)
        {
            if (_status == Status.NO_TURN)
            {
                try
                {
                    _status = Status.TURN_RIGHT;
                    _dome.SlewToAzimuth(_dome.Azimuth + 2.0);
                    _status = Status.NO_TURN;
                }
                catch (Exception ex)
                {
                    ErrDlg("Error slewing multiple step right", ex);
                }
            }
        }
        
        private void writeLastDomePosition(long pos)
        {
            try
            {
                XDocument config = XDocument.Load(configXml);

                var nodes = from node in config.Descendants("Encoder")
                           // where node.Element("LastPosition").Value == "LastPosition"
                            select node;
                foreach (XElement el in nodes)
                {
                    el.SetElementValue("LastPosition", pos.ToString());
                }
                config.Save(configXml);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_status == Status.TURN_LEFT || _status == Status.TURN_RIGHT)
                {
                    _dome.Stop();
                    _status = Status.NO_TURN;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SyncCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            ///
            /// To maintein compatibility with ASCOM dome driver it is passed as parameter the 
            /// telescope position, although this information is already available into the 
            /// Dome driver
            /// 
            //_dome.Synced = (bool)SyncCheckBox.IsChecked;
            if ((bool)SyncCheckBox.IsChecked) _dome.SyncToAzimuth(_dome._telescope.Azimuth);
            else _dome.UnsyncToAzimuth();
        }
    }
}
