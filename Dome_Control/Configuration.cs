using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.IO.Ports;
using System.Xml;
using System.Xml.XPath;
using System.Drawing;

namespace Dome_Control
{
    /// <summary>
    /// This 
    /// </summary>
    public partial class MainWindow //: Window
    {
        #region Members

        private string configXml;
        private string language;

        #endregion

        #region GUI Only Members

        private string AVR_COM_Name;
        private int AVR_COM_Baudrate;
        private int AVR_COM_Databits;
        private StopBits AVR_COM_Stopbits;
        private Parity AVR_COM_Parity;
        private Handshake AVR_COM_Handshake;

        public string ControlTabDef;
        public string ManualTabDef;
        public string DebugTabDef;
        public string Cnt_PortName_Label;
        public string Cnt_PortName_Grp_Def;
        private string Connected_Label;
        private string Disconnected_Label;
        private Image StatusBar_Image;        

        #endregion

        #region Methods

        /// <summary>
        /// Get the configration from the configuration file the configuration.
        /// </summary>
        void ReadConfiguration()
        {
            //
            //  Set-up the reading settings
            //
            XmlReaderSettings xmlSettings = new XmlReaderSettings();
            xmlSettings.IgnoreWhitespace = false;
            //
            //  Open the XML document
            //
            XPathDocument doc = new XPathDocument(configXml);
            XPathNavigator nav = doc.CreateNavigator();
            try
            {
                //----------------------------------------------------------------------
                //
                //  Read the AVR COM Port Configuration Settings
                //
                //----------------------------------------------------------------------
                //
                //  Get AVR Default COM
                //
                XPathNodeIterator node = nav.Select("config/AVR/COM");
                // needs that to access the xml sub-tree
                node.MoveNext();
                XPathNavigator target = node.Current;
                XPathNodeIterator nodes = target.SelectDescendants(XPathNodeType.Text, false);
                //  Iterate all the sub-tree to get the last node
                while (nodes.MoveNext())
                {
                    AVR_COM_Name = nodes.Current.Value;
                }
                // Check if the COM name is coherent
                if (!AVR_COM_Name.StartsWith("COM"))
                {
                    throw new Exception("Bad AVR COM Port Name");
                }
                //
                //  Get the COM Baudrate
                //
                node = nav.Select("config/AVR/Baudrate");
                //  Needs that to access the xml sub-tree
                node.MoveNext();
                target = node.Current;
                nodes = target.SelectDescendants(XPathNodeType.Text, false);
                //  Iterate all the sub-tree to get the last node
                while (nodes.MoveNext())
                {
                    try
                    {
                        int baud = nodes.Current.ValueAsInt;
                        AVR_COM_Baudrate = baud;
                        //if (((App)(Application.Current))._Dome_uC != null) ((App)(Application.Current))._Dome_uC._avr.setBaudrate(baud);
                        if (_dome != null) _dome._arduino._avr.setBaudrate(baud);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("AVR COM Bad Baudrate");
                    }
                }
                //
                //  Get the COM Databits
                //
                node = nav.Select("config/AVR/Databits");
                //  Needs that to access the xml sub-tree
                node.MoveNext();
                target = node.Current;
                nodes = target.SelectDescendants(XPathNodeType.Text, false);
                //  Iterate all the sub-tree to get the last node
                while (nodes.MoveNext())
                {
                    try
                    {
                        AVR_COM_Databits = nodes.Current.ValueAsInt;
                    }
                    catch
                    {
                        throw new Exception("AVR COM Bad Databits");
                    }
                }
                //
                //  Get the COM Stopbits
                //
                node = nav.Select("config/AVR/Stopbits");
                //  Needs that to access the xml sub-tree
                node.MoveNext();
                target = node.Current;
                nodes = target.SelectDescendants(XPathNodeType.Text, false);
                string res;
                //  Iterate all the sub-tree to get the last node
                while (nodes.MoveNext())
                {
                    res = nodes.Current.Value;
                    switch (res)
                    {
                        case "None":
                            {
                                AVR_COM_Stopbits = StopBits.None;
                                //if (((App)(Application.Current))._Dome_uC != null) ((App)(Application.Current))._Dome_uC._avr.setStopbits(StopBits.None);
                                if (_dome != null) _dome._arduino._avr.setStopbits(StopBits.None);
                                break;
                            }
                        case "One":
                            {
                                AVR_COM_Stopbits = StopBits.One;
                                //if (((App)(Application.Current))._Dome_uC != null) ((App)(Application.Current))._Dome_uC._avr.setStopbits(StopBits.One);
                                if (_dome != null) _dome._arduino._avr.setStopbits(StopBits.One);
                                break;
                            }
                        case "OnePointFive":
                            {
                                AVR_COM_Stopbits = StopBits.OnePointFive;
                                //if (((App)(Application.Current))._Dome_uC != null) ((App)(Application.Current))._Dome_uC._avr.setStopbits(StopBits.OnePointFive);
                                if (_dome != null) _dome._arduino._avr.setStopbits(StopBits.OnePointFive);
                                break;
                            }
                        case "Two":
                            {
                                AVR_COM_Stopbits = StopBits.Two;
                                //if (((App)(Application.Current))._Dome_uC != null) ((App)(Application.Current))._Dome_uC._avr.setStopbits(StopBits.Two);
                                if (_dome != null) _dome._arduino._avr.setStopbits(StopBits.Two);
                                break;
                            }
                        default: throw new Exception("AVR COM Bad Stopbit Format");
                    }
                }
                //
                //  Get the COM Parity
                //
                node = nav.Select("config/AVR/Parity");
                //  Needs that to access the xml sub-tree
                node.MoveNext();
                target = node.Current;
                nodes = target.SelectDescendants(XPathNodeType.Text, false);                
                //  Iterate all the sub-tree to get the last node
                while (nodes.MoveNext())
                {
                    res = nodes.Current.Value;
                    switch (res)
                    {
                        case "None":
                            {
                                AVR_COM_Parity = Parity.None;
                                //if (((App)(Application.Current))._Dome_uC != null) ((App)(Application.Current))._Dome_uC._avr.setParity(Parity.None);
                                if (_dome != null) _dome._arduino._avr.setParity(Parity.None);
                                break;
                            }
                        case "Even":
                            {
                                AVR_COM_Parity = Parity.Even;
                                //if (((App)(Application.Current))._Dome_uC != null) ((App)(Application.Current))._Dome_uC._avr.setParity(Parity.Even);
                                if (_dome != null) _dome._arduino._avr.setParity(Parity.Even);
                                break;
                            }
                        case "Mark":
                            {
                                AVR_COM_Parity = Parity.Mark;
                                //if (((App)(Application.Current))._Dome_uC != null) ((App)(Application.Current))._Dome_uC._avr.setParity(Parity.Mark);
                                if (_dome != null) _dome._arduino._avr.setParity(Parity.Mark);
                                break;
                            }
                        case "Odd":
                            {
                                AVR_COM_Parity = Parity.Odd;
                                //if (((App)(Application.Current))._Dome_uC != null) ((App)(Application.Current))._Dome_uC._avr.setParity(Parity.Odd);
                                if (_dome != null) _dome._arduino._avr.setParity(Parity.Odd);
                                break;
                            }
                        case "Space":
                            {
                                AVR_COM_Parity = Parity.Space;
                                //if (((App)(Application.Current))._Dome_uC != null) ((App)(Application.Current))._Dome_uC._avr.setParity(Parity.Space);
                                if (_dome != null) _dome._arduino._avr.setParity(Parity.Space);
                                break;
                            }
                        default: throw new Exception("AVR COM Bad Parity Format");
                    }
                }
                //
                //  Get the COM Handshake
                //
                node = nav.Select("config/AVR/Handshake");
                //  Needs that to access the xml sub-tree
                node.MoveNext();
                target = node.Current;
                nodes = target.SelectDescendants(XPathNodeType.Text, false);                
                //  Iterate all the sub-tree to get the last node
                while (nodes.MoveNext())
                {
                    res = nodes.Current.Value;
                    switch (res)
                    {
                        case "None":
                            {
                                AVR_COM_Handshake = Handshake.None;
                                //if (((App)(Application.Current))._Dome_uC != null) ((App)(Application.Current))._Dome_uC._avr.setHandshake(Handshake.None);
                                if (_dome != null) _dome._arduino._avr.setHandshake(Handshake.None);
                                break;
                            }
                        case "RTS":
                            {
                                AVR_COM_Handshake = Handshake.RequestToSend;
                                //if (((App)(Application.Current))._Dome_uC != null) ((App)(Application.Current))._Dome_uC._avr.setHandshake(Handshake.RequestToSend);
                                if (_dome != null) _dome._arduino._avr.setHandshake(Handshake.RequestToSend);
                                break;
                            }
                        case "XonXoff":
                            {
                                AVR_COM_Handshake = Handshake.XOnXOff;
                                //if (((App)(Application.Current))._Dome_uC != null) ((App)(Application.Current))._Dome_uC._avr.setHandshake(Handshake.XOnXOff);
                                if (_dome != null) _dome._arduino._avr.setHandshake(Handshake.XOnXOff);
                                break;
                            }
                        case "XonXoffRTS":
                            {
                                AVR_COM_Handshake = Handshake.RequestToSendXOnXOff;
                                //if (((App)(Application.Current))._Dome_uC != null) ((App)(Application.Current))._Dome_uC._avr.setHandshake(Handshake.RequestToSendXOnXOff);
                                if (_dome != null) _dome._arduino._avr.setHandshake(Handshake.RequestToSendXOnXOff);
                                break;
                            }
                        default: throw new Exception("AVR COM Bad Handshake Format");
                    }
                }
                //
                //  Get if it is an Arduino Device (need to properly configure the AVR_Device object)
                //
                node = nav.Select("config/AVR/Arduino");
                //  needs that to access the xml sub-tree
                node.MoveNext();
                target = node.Current;
                nodes = target.SelectDescendants(XPathNodeType.Text, false);
                //  Iterates all the sub-tree to get the last node
                while (nodes.MoveNext())
                {
                    res = nodes.Current.Value;
                    if (res.ToLower().Equals("yes")) isArduino = true;
                    else isArduino = false;
                }
                //
                //  Get if it is an Arduino Bootloader (need to properly configure AVR Dude)
                //
                node = nav.Select("config/AVR/Arduino_Bootloader");
                //  needs that to access the xml sub-tree
                node.MoveNext();
                target = node.Current;
                nodes = target.SelectDescendants(XPathNodeType.Text, false);
                //  Iterates all the sub-tree to get the last node
                while (nodes.MoveNext())
                {
                    res = nodes.Current.Value;
                    if (res.ToLower().Equals("yes")) isArduinoBootloader = true;
                    else isArduinoBootloader = false;
                }
                //
                //  Get AVR BootLoader COM
                //
                node = nav.Select("config/AVR/AVR_BootLoader_COM");
                // needs that to access the xml sub-tree
                node.MoveNext();
                target = node.Current;
                nodes = target.SelectDescendants(XPathNodeType.Text, false);
                //  Iterate all the sub-tree to get the last node
                while (nodes.MoveNext())
                {
                    AVRBootLoader_COM = nodes.Current.Value;
                }
                // Check if the COM name is coherent
                if (!AVRBootLoader_COM.StartsWith("COM"))
                {
                    throw new Exception("Bad AVR BootLoader COM Port Name");
                }
                //----------------------------------------------------------------------
                //
                //  Read Language Configuration Settings for the GUI language
                //
                //----------------------------------------------------------------------
                //
                //  Get English
                //
                node = nav.Select("config/Language");
                //  needs that to access the xml sub-tree
                node.MoveNext();
                target = node.Current;
                nodes = target.SelectDescendants(XPathNodeType.Text, false);
                //  Iterates all the sub-tree to get the last node
                while (nodes.MoveNext())
                {
                    res = nodes.Current.Value;
                    language = nodes.Current.Value + ".xml";
                }
                //----------------------------------------------------------------------
                //
                //  Read Encoder settings
                //
                //----------------------------------------------------------------------
                //
                //  Get Encoder Resolution
                //
                node = nav.Select("config/Encoder/resolution");
                //  needs that to access the xml sub-tree
                node.MoveNext();
                target = node.Current;
                nodes = target.SelectDescendants(XPathNodeType.Text, false);
                //  Iterates all the sub-tree to get the last node
                while (nodes.MoveNext())
                {
                    res = nodes.Current.Value;
//                    _enc_resolution = (uint)nodes.Current.ValueAsInt;
                    EncoderRes.Value = (uint)nodes.Current.ValueAsInt;
                }
                //
                //  Get Encoder Gear Ratio
                //
                node = nav.Select("config/Encoder/gear_ratio");
                //  needs that to access the xml sub-tree
                node.MoveNext();
                target = node.Current;
                nodes = target.SelectDescendants(XPathNodeType.Text, false);
                //  Iterates all the sub-tree to get the last node
                while (nodes.MoveNext())
                {
                    res = nodes.Current.Value;
//                    _gear_ratio = nodes.Current.ValueAsDouble;
                    GearRatio.Value = nodes.Current.ValueAsDouble;
                }
                //----------------------------------------------------------------------
                //
                //  Read Dome settings
                //
                //----------------------------------------------------------------------
                //
                //  Get Motor Accellearion time
                //
                node = nav.Select("config/Dome/accel_time");
                //  needs that to access the xml sub-tree
                node.MoveNext();
                target = node.Current;
                nodes = target.SelectDescendants(XPathNodeType.Text, false);
                //  Iterates all the sub-tree to get the last node
                while (nodes.MoveNext())
                {
                    res = nodes.Current.Value;
//                    _motor_accel_time = (uint)nodes.Current.ValueAsInt;
                    MotorAccelleration.Value = (uint)nodes.Current.ValueAsInt;
                }
                //
                //  Get Dome angular Speed
                //
                node = nav.Select("config/Dome/angular_speed_rpm");
                //  needs that to access the xml sub-tree
                node.MoveNext();
                target = node.Current;
                nodes = target.SelectDescendants(XPathNodeType.Text, false);
                //  Iterates all the sub-tree to get the last node
                while (nodes.MoveNext())
                {
                    res = nodes.Current.Value;
//                    _motor_rpm = (uint)nodes.Current.ValueAsInt;
                    MotorSpeed.Value = (uint)nodes.Current.ValueAsInt;
                }
                //
                //  Get Dome Threshold value
                //
                node = nav.Select("config/Dome/Threshold");
                //  needs that to access the xml sub-tree
                node.MoveNext();
                target = node.Current;
                nodes = target.SelectDescendants(XPathNodeType.Text, false);
                //  Iterates all the sub-tree to get the last node
                while (nodes.MoveNext())
                {
                    res = nodes.Current.Value;
//                    _threshold = (double)nodes.Current.ValueAsDouble;
                }
            }
            catch (Exception ex)
            {
                ErrDlg(Properties.Resources.Error_ReadConfFile, ex);
            }
        }

        ///// <summary>
        ///// Updates the GUI dialogs. Build up the configuration file path and launch the ReadGUIString method
        ///// </summary>
        //void UpdateDialogs()
        //{
        //    //	Configuration file might be in two spots, depending if it is
        //    //	development environment (Debug/bin) or in released environment
        //    //	therefore for flexibility both folders are checked!
        //    string fn = "../../" + language;
        //    if (System.IO.File.Exists(fn))
        //    {
        //        // Development version
        //        ReadGUIStrings(fn);
        //    }
        //    else
        //    {
        //        fn = language;
        //        if (System.IO.File.Exists(fn))
        //        {
        //            // Released Version
        //            ReadGUIStrings(fn);
        //        }
        //        else
        //        {
        //            ErrDlg("Configuration Language File Not Found", (Exception)null);
        //        }
        //    }

        //}

        ///// <summary>
        ///// Reads the GUI strings.
        ///// This methods is used to change the GUI language
        ///// </summary>
        ///// <param name="fn">The language definitions strings filename.</param>
        //void ReadGUIStrings(string fn)
        //{
        //    //  Setting up the XML parser configuration
        //    XmlReaderSettings settings = new XmlReaderSettings();
        //    settings.IgnoreWhitespace = false;

        //    //  Open the language xml file and create a xml tree navigator
        //    XPathDocument doc = new XPathDocument(fn);
        //    XPathNavigator nav = doc.CreateNavigator();
        //    //  Definition of variables used further
        //    XPathNodeIterator node;
        //    XPathNavigator target;
        //    XPathNodeIterator nodes;
        //    string res;

        //    try
        //    {
        //        //
        //        //  GUI Tabs
        //        //
        //        //  Get the Control Tab Label
        //        node = nav.Select("root/ControlTabDef");
        //        node.MoveNext();
        //        target = node.Current;
        //        nodes = target.SelectDescendants(XPathNodeType.Text, false);
        //        while (nodes.MoveNext())
        //        {
        //            ControlTab.Header = nodes.Current.Value;
        //        }
        //        //  Get the Manual Tab Label
        //        node = nav.Select("root/ManualTabDef");
        //        node.MoveNext();
        //        target = node.Current;
        //        nodes = target.SelectDescendants(XPathNodeType.Text, false);
        //        while (nodes.MoveNext())
        //        {
        //            ManualTab.Header = nodes.Current.Value;
        //        }
        //        //  Get the Debugl Tab Label
        //        node = nav.Select("root/DebugTabDef");
        //        node.MoveNext();
        //        target = node.Current;
        //        nodes = target.SelectDescendants(XPathNodeType.Text, false);
        //        while (nodes.MoveNext())
        //        {
        //            DebugTabDef = nodes.Current.Value;
        //        }
        //        //  Get the Manual Tab Label
        //        node = nav.Select("root/Cnt_PortName_Label");
        //        node.MoveNext();
        //        target = node.Current;
        //        nodes = target.SelectDescendants(XPathNodeType.Text, false);
        //        while (nodes.MoveNext())
        //        {
        //            COMPort_Label.Content = nodes.Current.Value;
        //        }
        //        //  Get the Connection Box Label
        //        node = nav.Select("root/Cnt_PortName_Grp_Def");
        //        node.MoveNext();
        //        target = node.Current;
        //        nodes = target.SelectDescendants(XPathNodeType.Text, false);
        //        while (nodes.MoveNext())
        //        {
        //            Connection_Box.Header = nodes.Current.Value;
        //        }
        //        //  Get the Connection Button Label
        //        node = nav.Select("root/Connection_Button_Label");
        //        node.MoveNext();
        //        target = node.Current;
        //        nodes = target.SelectDescendants(XPathNodeType.Text, false);
        //        while (nodes.MoveNext())
        //        {
        //            AVRConnectButton.Content = nodes.Current.Value;
        //            Connected_Label = nodes.Current.Value;
        //        }
        //        //  Get the Disconnection Button Label
        //        node = nav.Select("root/Disconnect_Button_Label");
        //        node.MoveNext();
        //        target = node.Current;
        //        nodes = target.SelectDescendants(XPathNodeType.Text, false);
        //        while (nodes.MoveNext())
        //        {
        //            //switch((System.Xml.XmlNodeType) nodes.Current.NodeType)
        //            //{
        //            //    case XmlNodeType.Text: Disconnected_Label = nodes.Current.Value; break;
        //            //}
        //            Disconnected_Label = nodes.Current.Value;
        //            //AVRConnectButton.Content = nodes.Current.Value;                    
        //        }
        //        //  Get the Control Mode Label
        //        node = nav.Select("root/CntMode_label");
        //        node.MoveNext();
        //        target = node.Current;
        //        nodes = target.SelectDescendants(XPathNodeType.Text, false);
        //        while (nodes.MoveNext())
        //        {
        //            CntMode_GroupBox.Header = nodes.Current.Value;
        //        }
        //        //  Get the Manual Control Mode Label
        //        node = nav.Select("root/ManualMode_Label");
        //        node.MoveNext();
        //        target = node.Current;
        //        nodes = target.SelectDescendants(XPathNodeType.Text, false);
        //        while (nodes.MoveNext())
        //        {
        //            manualRadioButton.Content = nodes.Current.Value;
        //        }
        //        //  Get the Automatic Control Mode Label
        //        node = nav.Select("root/AutomaticMode_Label");
        //        node.MoveNext();
        //        target = node.Current;
        //        nodes = target.SelectDescendants(XPathNodeType.Text, false);
        //        while (nodes.MoveNext())
        //        {
        //            autoRadioButton.Content = nodes.Current.Value;
        //        }
        //        //  Get the Temperature PWM Box Label
        //        node = nav.Select("root/Target_Temperature_Grp_Def");
        //        node.MoveNext();
        //        target = node.Current;
        //        nodes = target.SelectDescendants(XPathNodeType.Text, false);
        //        while (nodes.MoveNext())
        //        {
        //            autoGroupBox.Header = nodes.Current.Value;
        //        }
        //        //  Get the Temperatures Box Label
        //        node = nav.Select("root/Temperature_Grp_Def");
        //        node.MoveNext();
        //        target = node.Current;
        //        nodes = target.SelectDescendants(XPathNodeType.Text, false);
        //        while (nodes.MoveNext())
        //        {
        //            TemperaturesBox.Header = nodes.Current.Value;
        //        }
        //        //  Get the Peltier Temperature Label
        //        node = nav.Select("root/Peltier_Temperature_Label");
        //        node.MoveNext();
        //        target = node.Current;
        //        nodes = target.SelectDescendants(XPathNodeType.Text, false);
        //        while (nodes.MoveNext())
        //        {
        //            PeltierTemperatureLabel.Content = nodes.Current.Value;
        //        }
        //        //  Get the Room Temperature Label
        //        node = nav.Select("root/Room_Temperature_Label");
        //        node.MoveNext();
        //        target = node.Current;
        //        nodes = target.SelectDescendants(XPathNodeType.Text, false);
        //        while (nodes.MoveNext())
        //        {
        //            RoomTemperatureLabel.Content = nodes.Current.Value;
        //        }
        //        //  Get the Connection Box Label
        //        node = nav.Select("root/Elapsed");
        //        node.MoveNext();
        //        target = node.Current;
        //        nodes = target.SelectDescendants(XPathNodeType.Text, false);
        //        while (nodes.MoveNext())
        //        {
        //            elapsedTimeLabel.Content = nodes.Current.Value;
        //        }
        //        //
        //        //  Graphics Area GUI Definitions
        //        //
        //        //  Get Graphic Title
        //        node = nav.Select("root/Graph/Title");
        //        node.MoveNext();
        //        target = node.Current;
        //        nodes = target.SelectDescendants(XPathNodeType.Text, false);
        //        while (nodes.MoveNext())
        //        {
        //            Graph.TitleLabel = nodes.Current.Value;                    
        //        }                
        //        //  Get Graphic X axis Label
        //        node = nav.Select("root/Graph/X_Title");
        //        node.MoveNext();
        //        target = node.Current;
        //        nodes = target.SelectDescendants(XPathNodeType.Text, false);
        //        while (nodes.MoveNext())
        //        {
        //            Graph.XLabel = nodes.Current.Value;
        //        }
        //        //  Get Graphic Y axis Label
        //        node = nav.Select("root/Graph/Y_Title");
        //        node.MoveNext();
        //        target = node.Current;
        //        nodes = target.SelectDescendants(XPathNodeType.Text, false);
        //        while (nodes.MoveNext())
        //        {
        //            Graph.YLabel = nodes.Current.Value;
        //        }
        //        //  Get Graphic Peltier curve Label
        //        node = nav.Select("root/Graph/Peltier_Curve_Label");
        //        node.MoveNext();
        //        target = node.Current;
        //        nodes = target.SelectDescendants(XPathNodeType.Text, false);
        //        while (nodes.MoveNext())
        //        {
        //            Graph.PeltierCurveLabel = nodes.Current.Value;
        //        }
        //        //  Get Graphic room curve Label
        //        node = nav.Select("root/Graph/Room_Curve_Label");
        //        node.MoveNext();
        //        target = node.Current;
        //        nodes = target.SelectDescendants(XPathNodeType.Text, false);
        //        while (nodes.MoveNext())
        //        {
        //            Graph.RoomCurveLabel = nodes.Current.Value;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrDlg("Error Reading the Language Configuration", ex);
        //    }
        //}

        /// <summary>
        /// Updates the GUI Strings.
        /// </summary>
        void UpdateGUI()
        {
            try
            {
                //
                //  Menu Definitions
                //                           
//                MenuItemFileName.Header = Properties.Resources.MenuItemFileName;
//                NewFWLabel.Header = Properties.Resources.NewFWLabel;
//                DebugLabel.Header = Properties.Resources.DebugLabel;
//                MenuItemExitName.Header = Properties.Resources.MenuItemExitName;
//                MenuItemOptionsName.Header = Properties.Resources.MenuItemOptionsName;
                MenuItemFileName.Header = Dome_Control.Resources.Strings.MenuItemFileName;
                NewFWLabel.Header = Dome_Control.Resources.Strings.NewFWLabel;
                DebugLabel.Header = Dome_Control.Resources.Strings.DebugLabel;
                DebugMode.Header = Dome_Control.Resources.Strings.MenuItemDebugConsole;
                MenuItemExitName.Header = Dome_Control.Resources.Strings.MenuItemExitName;
                MenuItemOptionsName.Header = Dome_Control.Resources.Strings.MenuItemOptionsName;
                languageItem.Header = Dome_Control.Resources.Strings.LanguageUILabel;
                DebugMode.Header = Dome_Control.Resources.Strings.DebugMode;
                Options.Header = Dome_Control.Resources.Strings.MenuItemOptions;

                //COM_Options.Header = Properties.Resources.MenuItem_COM_Options;
                //GraphOptions.Header = Properties.Resources.MenuItem_Graph_Options;
                //PID_Options.Header = Properties.Resources.MenuItem_PID_Options;
//                HelpMenuItemName.Header = Properties.Resources.MenuItemHelpName;
//                HelpMenu.Header = Properties.Resources.MenuItemHelpName;
//                AboutItem.Header = Properties.Resources.MenuItemHelpAbout;
                HelpMenuItemName.Header = Dome_Control.Resources.Strings.MenuItemHelpName;
                HelpMenu.Header = Dome_Control.Resources.Strings.MenuItemHelpName;
                AboutItem.Header = Dome_Control.Resources.Strings.MenuItemHelpAbout;
                //
                //  GUI Tabs
                //                
                //  Set the Control Tab Label
//                ControlTab.Header = Properties.Resources.ControlTabDef;
                ControlTab.Header = Dome_Control.Resources.Strings.ControlTabDef;
                //  Set the Manual Tab Label
//                RunTab.Header = Properties.Resources.RunTabDef;
                RunTab.Header = Dome_Control.Resources.Strings.RunTabDef;
                //  Get the Debug Tab Label
//                DebugTabDef = Properties.Resources.DebugTabDef;
                DebugTab.Header = Dome_Control.Resources.Strings.DebugTabDef;
                //  Get the COM Port List Label
//                COMPort_Label.Content = Properties.Resources.ConnectionGroup_Label;
                COMPort_Label.Content = Dome_Control.Resources.Strings.ConnectionGroup_Label;
                //  Get the Connection Box Label
//                Connection_Box.Header = Properties.Resources.ConnectionGroup_Label;
                Connection_Box.Header = Dome_Control.Resources.Strings.ConnectionGroup_Label;
                //  Get the AVR Connection Button Label
//                AVRConnectButton.Content = Properties.Resources.Button_Connect;
//                Connected_Label = Properties.Resources.Button_Connect;
                Connected_Label = Dome_Control.Resources.Strings.Button_Connect;
                //  Get the AVR Disconnection Button Label
//                Disconnected_Label = Properties.Resources.Button_Disconnect;
                Disconnected_Label = Dome_Control.Resources.Strings.Button_Disconnect;
                //  Save/Load Config Button Labels
                LoadConfigButton.Content = Dome_Control.Resources.Strings.LoadConfigButtonLabel;
                SaveConfigButton.Content = Dome_Control.Resources.Strings.SaveConfigButtonLabel;
                //  Bootloader Elements
                BootloaderCOM_Label.Content = Dome_Control.Resources.Strings.BootloaderCOMLabel;
                //  Fill the ComboBox
                for (int i = 1; i < 100; i++)
                {
                    BootloaderCOM_Combo.Items.Add(string.Format("COM{0}", i));
                }

                    //  Get the Telescome Connection Button Label
                    //                ASCOMConnectButton.Content = Properties.Resources.Button_Connect;
                    ASCOMConnectButton.Content = Dome_Control.Resources.Strings.Button_Connect;
                //  Get Telescope/Dome Labels
//                DomePositionLabel.Content = Properties.Resources.DomePositionLabel;
//                TelescopeLabel.Content = Properties.Resources.TelescopeLabel;
//                AngleDiffLabel.Content = Properties.Resources.AngleDiffLabel;
//                SyncCheckBox.Content = Properties.Resources.DomeSyncLabel;
                DomePositionLabel.Content = Dome_Control.Resources.Strings.DomePositionLabel;
                TelescopeLabel.Content = Dome_Control.Resources.Strings.TelescopeLabel;
                AngleDiffLabel.Content = Dome_Control.Resources.Strings.AngleDiffLabel;
                SyncCheckBox.Content = Dome_Control.Resources.Strings.DomeSyncLabel;
                //  Get the Control Mode Label
                //CntMode_GroupBox.Header = Properties.Resources.ControlMode_Label;
                ////  Get the Manual Control Mode Label
                //manualRadioButton.Content = Properties.Resources.ManualTabDef;
                ////  Get the Automatic Control Mode Label
                //autoRadioButton.Content = Properties.Resources.AutomaticMode_Label;
                ////  Get the Temperature PWM Box Label
                //autoGroupBox.Header = Properties.Resources.TemperatureGroupBox_Label;
                ////  Get the Temperatures Box Label
                //TemperaturesBox.Header = Properties.Resources.TemperatureGroupBox_Label;
                ////  Get the Peltier Temperature Label
                //PeltierTemperatureLabel.Content = Properties.Resources.Peltier_Temperature_Label;
                ////  Get the Room Temperature Label
                //RoomTemperatureLabel.Content = Properties.Resources.Room_Temperature_Label;
                ////  Get the Connection Box Label
                //elapsedTimeLabel.Content = Properties.Resources.ElapseTime_Label;
                //
                //  Graphics Area GUI Definitions
                //
                //  Get Graphic Title
                //Graph.TitleLabel = Properties.Resources.GraphTitle_Label;
                ////  Get Graphic X axis Label
                //Graph.XLabel = Properties.Resources.GraphX_Label;
                ////  Get Graphic Y axis Label
                //Graph.YLabel = Properties.Resources.GraphY_Label;
                ////  Get Graphic Peltier curve Label
                //Graph.PeltierCurveLabel = Properties.Resources.Peltier_Temperature_Label;
                ////  Get Graphic room curve Label
                //Graph.RoomCurveLabel = Properties.Resources.Room_Temperature_Label;
            }
            catch (Exception ex)
            {
                ErrDlg(Properties.Resources.Error_UpdateGUI, ex);
            }
        }

        XmlDocument createXmlConfig()
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml("<config></config>");
            doc.PrependChild(doc.CreateXmlDeclaration("1.0", "UTF-8", ""));

            //XmlNode config = doc.CreateElement("config");
            //doc.AppendChild(config);
            XmlNode config = doc.GetElementsByTagName("config")[0];

            XmlElement AVR = doc.CreateElement("AVR");
            config.AppendChild(AVR);
            XmlElement COM = doc.CreateElement("COM");
            AVR.AppendChild(COM);
            XmlElement baud = doc.CreateElement("Baudrate");
            AVR.AppendChild(baud);
            XmlElement databits = doc.CreateElement("Databits");
            AVR.AppendChild(databits);
            XmlElement stopbit = doc.CreateElement("Stopbits");
            AVR.AppendChild(stopbit);
            XmlElement parity = doc.CreateElement("Parity");
            AVR.AppendChild(parity);
            XmlElement handshake = doc.CreateElement("Handshake");
            AVR.AppendChild(handshake);
            XmlElement arduitoBootloader = doc.CreateElement("Arduino_Bootloader");
            AVR.AppendChild(arduitoBootloader);
            XmlElement bootloaderCOM = doc.CreateElement("Arduino_Bootloader_COM");
            AVR.AppendChild(bootloaderCOM);

            XmlElement lang = doc.CreateElement("Language");
            config.AppendChild(lang);

            XmlElement dome = doc.CreateElement("Dome");
            config.AppendChild(dome);
            XmlElement accel = doc.CreateElement("accel_time");
            dome.AppendChild(accel);
            XmlElement speed = doc.CreateElement("angular_speed_rpm");
            dome.AppendChild(speed);
            XmlElement thsld = doc.CreateElement("Threshold");
            dome.AppendChild(thsld);

            XmlElement encoder = doc.CreateElement("Encoder");
            config.AppendChild(encoder);
            XmlElement type = doc.CreateElement("type");
            encoder.AppendChild(type);
            XmlElement res = doc.CreateElement("resolution");
            encoder.AppendChild(res);
            XmlElement gearRatio = doc.CreateElement("gear_ratio");
            encoder.AppendChild(gearRatio);
            XmlElement pos = doc.CreateElement("LastPosition");
            encoder.AppendChild(pos);

            COM.InnerText = AVR_COM_Name;
            baud.InnerText = AVR_COM_Baudrate.ToString();
            databits.InnerText = AVR_COM_Databits.ToString();
            stopbit.InnerText = AVR_COM_Stopbits.ToString();
            parity.InnerText = AVR_COM_Parity.ToString();
            handshake.InnerText = AVR_COM_Handshake.ToString();
            arduitoBootloader.InnerText = isArduinoBootloader.ToString();
            bootloaderCOM.InnerText = AVRBootLoader_COM;

            lang.InnerText = language;

            type.InnerText = "Incremental";
            res.InnerText = this.EncoderRes.Value.ToString();
            gearRatio.InnerText = this.GearRatio.Value.ToString();
            pos.InnerText = _dome.Azimuth.ToString();
            accel.InnerText = MotorAccelleration.Value.ToString();
            speed.InnerText = MotorSpeed.Value.ToString();
            thsld.InnerText = this.Threshold.Value.ToString();

            return doc;
        }
        
		void LogCheckBox_Click(object sender, RoutedEventArgs e)
		{
			throw new NotImplementedException();
		}

        #endregion

    }
}
