using System;
using System.Collections.Generic;
using System.Collections;
using System.Globalization;
using System.Runtime.InteropServices;
using ASCOM.DeviceInterface;
using ASCOM.Utilities;
using ASCOM;
using Arduino.Dome;
using System.Linq;
using System.Text;

namespace Arduino.Dome
{
    [Guid ("B827B6C0-DBCF-4F17-97FE-7FCD538469B6")]
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    public class Dome: IDomeV2
    {
        #region Constants

        internal const string driverID = "ASCOM Arduino Dome Driver";
        private const string driverDescription = "ASCOM Arduino Dome Driver API";
        private const string driverVer = "0.1";

        #endregion

        #region Members

        private ArduinoDome _arduino;
        private Util HC = new Util();
        private Config _config = new Config();
        private Angle Braking = 0.0;

        #endregion

        #region ASCOM Registration

        /// <summary>
        /// Register/Unregister the ASCOM Dome Driver. This is harmless if already
        /// register or unregistered.
        /// </summary>
        /// <param name="bRegister">if set to <c>true</c> [b register].</param>
        private static void RegUnregASCOM(bool bRegister)
        {            
            using (var p = new Profile())
            {
                p.DeviceType = "Dome";
                if (bRegister) p.Register(driverID, driverDescription);
                else p.Unregister(driverID);
                try
                {
                    Marshal.ReleaseComObject(p);
                }
                catch
                {

                }
            }
            //p = null;
        }

        /// <summary>
        /// Registers the ASCOM Dome Driver.
        /// </summary>
        /// <param name="T">The T.</param>
        [ComRegisterFunction]
        public static void RegisterASCOM(Type T)
        {
            RegUnregASCOM(true);
        }

        /// <summary>
        /// Unregisters the ASCOM Dome Driver.
        /// </summary>
        /// <param name="T">The T.</param>
        [ComUnregisterFunction]
        public static void UnregisterASCOM(Type T)
        {
            RegUnregASCOM(false);
        }

        #endregion        

        #region Properties

        public Angle _position;
        public double AngleResulution { get; set; }
       

        #endregion

        #region Implementation of IDomeV2

        #region IDomeV2 Properties Implementation

        public double Altitude
        {
            get { throw new PropertyNotImplementedException("Altitude not implemented", false); }
        }

        public bool AtHome
        {
            get
            {
                if (Math.Abs(_position) < AngleResulution) return true;
                else return false;
            } 
        }

        public bool AtPark
        {
            get { return _config.Parked; }
        }

        public Angle Azimouth
        {
            get { return (Angle)_config.Azimuth; }
        }

        public bool CanFindHome
        {
            get { return false; }
        }

        public bool CanPark
        {
            get { return true; }
        }

        public bool CanSetAltitude
        {
            get { return false; }
        }

        public bool CanSetAzimuth
        {
            get { return true; }
        }

        public bool CanSetPark
        {
            get { return true; }
        }

        public bool CanSetShutter
        {
            get { return false; }
        }

        public bool CanSyncAzimuth
        {
            get { return true; }
        }

        public string Description
        {
            get { return driverDescription; }
        }

        public string DriverInfo
        {
            get { return driverVer; }
        }

        public bool Connected
        {
            get { return _config.Link; }
            set
            {
                switch(value)
                {
                    case true: _config.Link = _arduino.Connect(); break;
                    case false: _config.Link = _arduino.Disconnect(); break;
                }
            }
        }

        public short InterfaceVersion
        {
            get { return 1; }
        }

        public string Name
        {
            get { return "Arduino Dome"; }
        }

        public ShutterState ShutterStatus
        {
            get { throw new PropertyNotImplementedException("ShutterStatus not implemented"); }
        }

        public bool Slaved
        {
            get { return _config.Slaved; }
            set { _config.Slaved = value; }
        }

        public bool Slewing
        {
            get { return _config.IsSlewing; }
        }

        #endregion

        #region IDomeV2 Method Implementation

        public void AbortSlew()
        {
            _arduino.Stop();
        }


        public void CloseShutter()
        {
            throw new MethodNotImplementedException("CloseShutter Method not implemented");
        }

        public void CommandBind(string cmd)
        {
            throw new MethodNotImplementedException("CommandBind Method not implemented");
        }

        public void CommandBool(string cmd)
        {
            throw new MethodNotImplementedException("CommandBool not implemented");
        }

        public void CommandString(string cmd)
        {
            throw new MethodNotImplementedException("CommandString not implemented");
        }

        public void Dispose()
        {
            throw new MethodNotImplementedException("Dispose not implemented");
        }

        public void FindHome()
        {
            throw new MethodNotImplementedException("FindHome not implemented");
        }        

        public void OpenShutter()
        {
            throw new MethodNotImplementedException("OpenShutter not implemented");
        }

        public void Park()
        {
            //  Lock on which direction the Dome has to tuen, HP angle is 0..360°
            if (_position > 180)
            {
                _arduino.TurnLeft();
                //  Should be placed on backgnd thread
                while (!_config.Parked) HC.WaitForMilliseconds(100);
            }
            else if (_position < 180)
            {
                _arduino.TurnRight();
                //  Should be placed on backgnd thread
                while (!_config.Parked) HC.WaitForMilliseconds(100);
            }
            else if (_position == 0)
            {
                _arduino.Stop();
            }
            _arduino.Stop();
        }

        public void SetPark()
        {
            _config.ParkPosition = _config.Azimuth;
        }

        public void SetupDialog()
        {
            using (var f = new SetupDialogForm())
            {
                f.ShowDialog();
            }
        }

        public void SlewToAltitude(double Altitude)
        {
            throw new MethodNotImplementedException("SlewToAltitude not implemented");
        }

        public void SlewToAzimuth(Angle Azimuth)
        {
            if (Azimouth > 360 || Azimouth < 0)
            {
                throw new Exception("Angle out of range");
            }
            _config.IsSlewing = true;
            //Angle _azim = Azimouth;
            if (_position < Azimouth)
            {
                _arduino.TurnLeft();
                //  Put this on backgnd thread
                while (Math.Abs(_position - Azimouth) > Braking)
                {
                    HC.WaitForMilliseconds(100);
                }
                _arduino.Stop();
            }
            else if (_position > Azimouth)
            {
                _arduino.TurnRight();
                //  Put this on backgnd thread
                while (Math.Abs(_position - Azimouth) > Braking)
                {
                    HC.WaitForMilliseconds(100);
                }
                _arduino.Stop();
            }
            else if (Math.Abs(_position - Azimouth) <= Braking) { }
            _config.IsSlewing = false;
        }

        #endregion

        #endregion

        #region Constructors
        #endregion

        #region Methods
        #endregion
    }
}
