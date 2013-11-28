//tabs=4
// --------------------------------------------------------------------------------
// TODO fill in this information for your driver, then remove this line!
//
// ASCOM Dome driver for Arduino
//
// Description:    Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam 
//                nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam 
//                erat, sed diam voluptua. At vero eos et accusam et justo duo 
//                dolores et ea rebum. Stet clita kasd gubergren, no sea takimata 
//                sanctus est Lorem ipsum dolor sit amet.
//
// Implements:    ASCOM Dome interface version: <To be completed by driver developer>
// Author:        (XXX) Your N. Here <your@email.here>
//
// Edit Log:
//
// Date            Who    Vers    Description
// -----------    ---    -----    -------------------------------------------------------
// dd-mmm-yyyy    XXX    6.0.0    Initial edit, created from ASCOM driver template
// --------------------------------------------------------------------------------
//


// This is used to define code in the template that is specific to one class implementation
// unused code canbe deleted and this definition removed.
#define Dome

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Threading;

using ASCOM;
using ASCOM.Astrometry;
using ASCOM.Astrometry.AstroUtils;
using ASCOM.Utilities;
using ASCOM.DeviceInterface;
using System.Globalization;
using System.Collections;
using Arduino.Dome;
using ASCOM_Telescope_ns;

namespace ASCOM.Arduino
{
    //
    // Your driver's DeviceID is ASCOM.Arduino.Dome
    //
    // The Guid attribute sets the CLSID for ASCOM.Arduino.Dome
    // The ClassInterface/None addribute prevents an empty interface called
    // _Arduino from being created and used as the [default] interface
    //
    // TODO Replace the not implemented exceptions with code to implement the function or
    // throw the appropriate ASCOM exception.
    //

    /// <summary>
    /// ASCOM Dome Driver for Arduino.
    /// </summary>
    [Guid("d2e659c5-ee5d-4744-a9ff-ff74b7c7bfb0")]
    [ClassInterface(ClassInterfaceType.None)]
    public class Dome : IDomeV2
    {
        #region Constants

        /// <summary>
        /// ASCOM DeviceID (COM ProgID) for this driver.
        /// The DeviceID is used by ASCOM applications to load the driver at runtime.
        /// </summary>
        internal static string driverID = "ASCOM.Arduino.Dome";
        // TODO Change the descriptive string for your driver then remove this line
        /// <summary>
        /// Driver description that displays in the ASCOM Chooser.
        /// </summary>
        private static string driverDescription = "ASCOM Dome Driver for Arduino.";

        internal static string comPortProfileName = "COM Port"; // Constants used for Profile persistence
        internal static string comPortDefault = "COM3";
        internal static string traceStateProfileName = "Trace Level";
        internal static string traceStateDefault = "false";

        internal static string comPort; // Variables to hold the currrent device configuration
        internal static bool traceState;
        
        #endregion

        #region Private Members

        /// <summary>
        /// Private variable to hold the Aruidno Microcontroller driver
        /// </summary>
        private ArduinoDome _arduino;
        public ASCOM_Telescope _telescope;
        private double _position;
        private bool Parked;
        private double ParkPosition;
        private bool IsSlewing;
        private System.Windows.Threading.DispatcherTimer DomeTimer;
        /// <summary>
        /// Private variable to hold the connected state
        /// </summary>
        private bool connectedState;

        /// <summary>
        /// Private variable to hold an ASCOM Utilities object
        /// </summary>
        private Util utilities;

        /// <summary>
        /// Private variable to hold an ASCOM AstroUtilities object to provide the Range method
        /// </summary>
        private AstroUtils astroUtilities;

        /// <summary>
        /// Private variable to hold the trace logger object (creates a diagnostic log file with information that you specify)
        /// </summary>
        private TraceLogger tl;
        
        #endregion

        #region Public Properties

        public double Braking
        {
            get;
            set
            {
                Braking = value;
            }
        }

        public double Threshold { get; set; }

        public bool Synced { get; set; }
                
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Arduino"/> class.
        /// Must be public for COM registration.
        /// </summary>
        public Dome()
        {
            ReadProfile(); // Read device configuration from the ASCOM Profile store

            tl = new TraceLogger("", "Arduino");
            tl.Enabled = traceState;
            tl.LogMessage("Dome", "Starting initialisation");

            connectedState = false; // Initialise connected to false
            utilities = new Util(); //Initialise util object
            astroUtilities = new AstroUtils(); // Initialise astro utilities object
            //TODO: Implement your additional construction here
            _position = 0.0;
            Parked = true;
            ParkPosition = 0.0;
            IsSlewing = false;
            connectedState = false;
            Braking = 0.0;
            DomeTimer = new System.Windows.Threading.DispatcherTimer();
            DomeTimer.Interval = TimeSpan.FromSeconds(3);//new TimeSpan(0, 0, 3);
            DomeTimer.Tick += DomeTimer_Tick;
            DomeTimer.IsEnabled = true;
            DomeTimer.Stop();

            tl.LogMessage("Dome", "Completed initialisation");
        }

        #endregion


        //
        // PUBLIC COM INTERFACE IDomeV2 IMPLEMENTATION
        //

        #region Common properties and methods.

        /// <summary>
        /// Displays the Setup Dialog form.
        /// If the user clicks the OK button to dismiss the form, then
        /// the new settings are saved, otherwise the old values are reloaded.
        /// THIS IS THE ONLY PLACE WHERE SHOWING USER INTERFACE IS ALLOWED!
        /// </summary>
        public void SetupDialog()
        {
            // consider only showing the setup dialog if not connected
            // or call a different dialog if connected
            if (IsConnected)
                System.Windows.Forms.MessageBox.Show("Already connected, just press OK");

            using (SetupDialogForm F = new SetupDialogForm())
            {
                var result = F.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    WriteProfile(); // Persist device configuration values to the ASCOM Profile store
                }
            }
        }

        public ArrayList SupportedActions
        {
            get
            {
                tl.LogMessage("SupportedActions Get", "Returning empty arraylist");
                return new ArrayList();
            }
        }

        public string Action(string actionName, string actionParameters)
        {
            throw new ASCOM.ActionNotImplementedException("Action " + actionName + " is not implemented by this driver");
        }

        public void CommandBlind(string command, bool raw)
        {
            CheckConnected("CommandBlind");
            // Call CommandString and return as soon as it finishes
            this.CommandString(command, raw);
            // or
            throw new ASCOM.MethodNotImplementedException("CommandBlind not implemented");
        }

        public bool CommandBool(string command, bool raw)
        {
            CheckConnected("CommandBool");
            string ret = CommandString(command, raw);
            // TODO decode the return string and return true or false
            // or
            throw new ASCOM.MethodNotImplementedException("CommandBool not implemented");
        }

        public string CommandString(string command, bool raw)
        {
            CheckConnected("CommandString");
            // it's a good idea to put all the low level communication with the device here,
            // then all communication calls this function
            // you need something to ensure that only one command is in progress at a time

            throw new ASCOM.MethodNotImplementedException("CommandString not implemented");
        }

        public void Dispose()
        {
            // Clean up the tracelogger and util objects
            tl.Enabled = false;
            tl.Dispose();
            tl = null;
            utilities.Dispose();
            utilities = null;
            astroUtilities.Dispose();
            astroUtilities = null;
        }

        public bool Connected
        {
            get
            {
                tl.LogMessage("Connected Get", IsConnected.ToString());
                return IsConnected;
            }
            set
            {
                tl.LogMessage("Connected Set", value.ToString());
                if (value == IsConnected)
                    return;

                if (value)
                {
                    //connectedState = true;
                    tl.LogMessage("Connected Set", "Connecting to port " + comPort);
                    // TODO connect to the device
                    connectedState = _arduino.Connect();
                }
                else
                {
                    connectedState = false;
                    tl.LogMessage("Connected Set", "Disconnecting from port " + comPort);
                    // TODO disconnect from the device
                    connectedState = _arduino.Disconnect();
                }                
            }
        }

        public string Description
        {
            // TODO customise this device description
            get
            {
                tl.LogMessage("Description Get", driverDescription);
                return driverDescription;
            }
        }

        public string DriverInfo
        {
            get
            {
                Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                // TODO customise this driver description
                string driverInfo = "Information about the driver itself. Version: " + String.Format(CultureInfo.InvariantCulture, "{0}.{1}", version.Major, version.Minor);
                tl.LogMessage("DriverInfo Get", driverInfo);
                return driverInfo;
            }
        }

        public string DriverVersion
        {
            get
            {
                Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                string driverVersion = String.Format(CultureInfo.InvariantCulture, "{0}.{1}", version.Major, version.Minor);
                tl.LogMessage("DriverVersion Get", driverVersion);
                return driverVersion;
            }
        }

        public short InterfaceVersion
        {
            // set by the driver wizard
            get
            {
                tl.LogMessage("InterfaceVersion Get", "2");
                return Convert.ToInt16("2");
            }
        }

        public string Name
        {
            get
            {
                string name = "ASCOM Arduino Dome - Driver for an ASCOM Dome based on Arduino microcontroller";
                tl.LogMessage("Name Get", name);
                return name;
            }
        }

        #endregion

        #region IDome Implementation

        private bool domeShutterState = false; // Variable to hold the open/closed status of the shutter, true = Open

        public void AbortSlew()
        {
            _arduino.Stop();
            // This is a mandatory parameter but we have no action to take in this simple driver
            tl.LogMessage("AbortSlew", "Completed");
        }

        public double Altitude
        {
            get
            {
                tl.LogMessage("Altitude Get", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("Altitude not implemented", false);
            }
        }

        public bool AtHome
        {
            get
            {
                bool result;
                if (Math.Abs(_position) < Braking) result = true;
                else result= false;
                tl.LogMessage("AtHome Get:", result.ToString());
                return result;
                //throw new ASCOM.PropertyNotImplementedException("AtHome", false);
            }
        }

        public bool AtPark
        {
            get
            {
                tl.LogMessage("AtPark Get:", Parked.ToString());
                return Parked;
                //throw new ASCOM.PropertyNotImplementedException("AtPark", false);
            }
        }

        public double Azimuth
        {
            get
            {
                tl.LogMessage("Azimuth Get", _position.ToString());
                return _position;
                //throw new ASCOM.PropertyNotImplementedException("Azimuth", false);
            }
        }

        public bool CanFindHome
        {
            get
            {
                tl.LogMessage("CanFindHome Get", false.ToString());
                return false;
            }
        }

        public bool CanPark
        {
            get
            {
                tl.LogMessage("CanPark Get", true.ToString());
                return true;
            }
        }

        public bool CanSetAltitude
        {
            get
            {
                tl.LogMessage("CanSetAltitude Get", false.ToString());
                return false;
            }
        }

        public bool CanSetAzimuth
        {
            get
            {
                tl.LogMessage("CanSetAzimuth Get", true.ToString());
                return true;
            }
        }

        public bool CanSetPark
        {
            get
            {
                tl.LogMessage("CanSetPark Get", true.ToString());
                return true;
            }
        }

        public bool CanSetShutter
        {
            get
            {
                tl.LogMessage("CanSetShutter Get", false.ToString());
                return false;
            }
        }

        public bool CanSlave
        {
            get
            {
                tl.LogMessage("CanSlave Get", false.ToString());
                return false;
            }
        }

        public bool CanSyncAzimuth
        {
            get
            {
                tl.LogMessage("CanSyncAzimuth Get", true.ToString());
                return true;
            }
        }

        public void CloseShutter()
        {
            tl.LogMessage("CloseShutter", "Method not implemented");
            //domeShutterState = false;
            throw new ASCOM.MethodNotImplementedException("CloseShutter not implemented");
        }

        public void FindHome()
        {
            tl.LogMessage("FindHome", "Not implemented");
            throw new ASCOM.MethodNotImplementedException("FindHome not implemented");
        }

        public void OpenShutter()
        {
            tl.LogMessage("OpenShutter", "Shutter has been opened");
            //domeShutterState = true;
            throw new ASCOM.MethodNotImplementedException("OpenShutter not implemented");
        }

        public void Park()
        {
            tl.LogMessage("Park", "Start Parking");
            //throw new ASCOM.MethodNotImplementedException("Park");

            //  Check on which direction is shorter to park
            if (_position > 180.0)
            {
                _arduino.Slew(Direction.LEFT);
                while (!Parked) utilities.WaitForMilliseconds(100);
            }
            else if (_position < 180.0)
            {
                _arduino.Slew(Direction.RIGHT);
                while (!Parked) utilities.WaitForMilliseconds(100);
            }
            else if (_position == 0.0)
            {
                
            }
            _arduino.Stop();
        }

        public void SetPark()
        {
            tl.LogMessage("SetPark:", string.Format("New Park Position is {0}",Azimuth));
            //throw new ASCOM.MethodNotImplementedException("SetPark");
            ParkPosition = Azimuth;
        }

        public ShutterState ShutterStatus
        {
            get
            {
                //tl.LogMessage("CanSyncAzimuth Get", false.ToString());
                //if (domeShutterState)
                //{
                //    tl.LogMessage("ShutterStatus", ShutterState.shutterOpen.ToString());
                //    return ShutterState.shutterOpen;
                //}
                //else
                //{
                //    tl.LogMessage("ShutterStatus", ShutterState.shutterClosed.ToString());
                //    return ShutterState.shutterClosed;
                //}
                tl.LogMessage("ShutterStatus:", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("ShutterStatus not implemented");                
            }
        }

        public bool Slaved
        {
            get
            {
                tl.LogMessage("Slaved Get", false.ToString());
                return false;
            }
            set
            {
                tl.LogMessage("Slaved Set", "not implemented");
                throw new ASCOM.PropertyNotImplementedException("Slaved", true);
            }
        }

        public void SlewToAltitude(double Altitude)
        {
            tl.LogMessage("SlewToAltitude", "Not Implemented"); 
            throw new ASCOM.MethodNotImplementedException("SlewToAltitude");            
        }

        public void SlewToAzimuth(double Azimuth)
        {
            tl.LogMessage("SlewToAzimuth", Azimuth.ToString());
            //throw new ASCOM.MethodNotImplementedException("SlewToAzimuth");
            if (Azimuth > 360 || Azimuth < 0)
            {
                throw new ASCOM.InvalidValueException("Angle Out of Range");
            }
            IsSlewing = true;
            if (_position < Azimuth)
            {
                _arduino.Slew(Direction.ANTICLOCWISE);
                while (Math.Abs(_position - Azimuth) > Braking)
                {
                    utilities.WaitForMilliseconds(100);
                }
                _arduino.Stop();
            }
            else if (_position > Azimuth)
            {
                _arduino.Slew(Direction.CLOCKWISE);
                while (Math.Abs(_position - Azimuth) > Braking)
                {
                    utilities.WaitForMilliseconds(100);
                }
                _arduino.Stop();
            }
            else if (Math.Abs(_position - Azimuth) < Braking)
            {

            }
            IsSlewing = false;
        }

        public bool Slewing
        {
            get
            {
                tl.LogMessage("Slewing Get", IsSlewing.ToString());
                return IsSlewing;
            }
        }

        public void SyncToAzimuth(double Azimuth)
        {
            tl.LogMessage("SyncToAzimuth", "Starting Synchronization to Azimuth");
            DomeTimer.Start();
            Synced = true;
            //throw new ASCOM.MethodNotImplementedException("SyncToAzimuth");
        }

        public void UnsyncToAzimuth()
        {
            tl.LogMessage("UnsyncToAzimuth", "Stopping Synchroniziation to Azimouth");
            DomeTimer.Stop();
            Synced = false;
        }

        #endregion

        #region Event Handlers

        private void DomeTimer_Tick(object sender, EventArgs e)
        {
            //  Check if the connected telescope has moved enough to need a new dome slew
            if (Math.Abs(_telescope.azimut - _position) > Threshold)
            {
                //  If yes check in which direction the dome has to turn and perform the slewing
                if (_telescope.azimut > _position)
                {
                    SlewToAzimuth(_position + Threshold);
                }
                else
                {
                    SlewToAzimuth(_position - Threshold);
                }
            }
        }

        #endregion 
        #region Private properties and methods
        // here are some useful properties and methods that can be used as required
        // to help with driver development

        #region ASCOM Registration

        // Register or unregister driver for ASCOM. This is harmless if already
        // registered or unregistered. 
        //
        /// <summary>
        /// Register or unregister the driver with the ASCOM Platform.
        /// This is harmless if the driver is already registered/unregistered.
        /// </summary>
        /// <param name="bRegister">If <c>true</c>, registers the driver, otherwise unregisters it.</param>
        private static void RegUnregASCOM(bool bRegister)
        {
            using (var P = new ASCOM.Utilities.Profile())
            {
                P.DeviceType = "Dome";
                if (bRegister)
                {
                    P.Register(driverID, driverDescription);
                }
                else
                {
                    P.Unregister(driverID);
                }
                try
                {
                    Marshal.ReleaseComObject(P);
                }
                catch { }
            }
            P = null;
        }

        /// <summary>
        /// This function registers the driver with the ASCOM Chooser and
        /// is called automatically whenever this class is registered for COM Interop.
        /// </summary>
        /// <param name="t">Type of the class being registered, not used.</param>
        /// <remarks>
        /// This method typically runs in two distinct situations:
        /// <list type="numbered">
        /// <item>
        /// In Visual Studio, when the project is successfully built.
        /// For this to work correctly, the option <c>Register for COM Interop</c>
        /// must be enabled in the project settings.
        /// </item>
        /// <item>During setup, when the installer registers the assembly for COM Interop.</item>
        /// </list>
        /// This technique should mean that it is never necessary to manually register a driver with ASCOM.
        /// </remarks>
        [ComRegisterFunction]
        public static void RegisterASCOM(Type t)
        {
            RegUnregASCOM(true);
        }

        /// <summary>
        /// This function unregisters the driver from the ASCOM Chooser and
        /// is called automatically whenever this class is unregistered from COM Interop.
        /// </summary>
        /// <param name="t">Type of the class being registered, not used.</param>
        /// <remarks>
        /// This method typically runs in two distinct situations:
        /// <list type="numbered">
        /// <item>
        /// In Visual Studio, when the project is cleaned or prior to rebuilding.
        /// For this to work correctly, the option <c>Register for COM Interop</c>
        /// must be enabled in the project settings.
        /// </item>
        /// <item>During uninstall, when the installer unregisters the assembly from COM Interop.</item>
        /// </list>
        /// This technique should mean that it is never necessary to manually unregister a driver from ASCOM.
        /// </remarks>
        [ComUnregisterFunction]
        public static void UnregisterASCOM(Type t)
        {
            RegUnregASCOM(false);
        }

        #endregion

        /// <summary>
        /// Returns true if there is a valid connection to the driver hardware
        /// </summary>
        private bool IsConnected
        {
            get
            {
                for (int i = 0; i < 10; i++)
                {
                    if (_arduino.getACK())
                    {
                        connectedState = true;
                        return true;
                    }
                    else
                    {
                        utilities.WaitForMilliseconds(10);
                    }
                }
                connectedState = false;
                // TODO check that the driver hardware connection exists and is connected to the hardware
                return connectedState;
            }
        }

        /// <summary>
        /// Use this function to throw an exception if we aren't connected to the hardware
        /// </summary>
        /// <param name="message"></param>
        private void CheckConnected(string message)
        {
            if (!IsConnected)
            {
                throw new ASCOM.NotConnectedException(message);
            }
        }

        /// <summary>
        /// Read the device configuration from the ASCOM Profile store
        /// </summary>
        internal void ReadProfile()
        {
            using (Profile driverProfile = new Profile())
            {
                driverProfile.DeviceType = "Dome";
                traceState = Convert.ToBoolean(driverProfile.GetValue(driverID, traceStateProfileName, string.Empty, traceStateDefault));
                comPort = driverProfile.GetValue(driverID, comPortProfileName, string.Empty, comPortDefault);
            }
        }

        /// <summary>
        /// Write the device configuration to the  ASCOM  Profile store
        /// </summary>
        internal void WriteProfile()
        {
            using (Profile driverProfile = new Profile())
            {
                driverProfile.DeviceType = "Dome";
                driverProfile.WriteValue(driverID, traceStateProfileName, traceState.ToString());
                driverProfile.WriteValue(driverID, comPortProfileName, comPort.ToString());
            }

        }

        #endregion

        #region Public Methods

        public void TurnLeft()
        {
            //  Tell Arduino Driver to turn left and update slewing property
            _arduino.Slew(Direction.ANTICLOCWISE);  
            IsSlewing = true;
        }

        public void TurnRight()
        {
            //  Tell Arduino Driver to turn right and update slewing property
            _arduino.Slew(Direction.CLOCKWISE);
            IsSlewing = true;
        }

        public void Stop()
        {
            //  If is slewing, tells Arduino Driver to stop turning and update slewing property
            if (IsSlewing)
            {
                _arduino.Stop();
                IsSlewing = false;
            }
        }

        #endregion
    }
}

