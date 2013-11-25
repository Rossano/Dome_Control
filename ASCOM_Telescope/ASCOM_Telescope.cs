using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASCOM.DeviceInterface;
using ASCOM.DriverAccess;
using ASCOM.Helper;

namespace ASCOM_Telescope_ns
{
    public class ASCOM_Telescope
    {
        #region Members

        private string progID;
        private Telescope _telescope;
        public Angle azimut { get; private set; }
        public Angle declination { get; private set; }
        ASCOM.Helper.Util U = new ASCOM.Helper.Util();

        #endregion

        #region Constructor

        public ASCOM_Telescope()
        {
            progID = Telescope.Choose("ScopeSim.Telescope");
            if (progID != "")
            {
                _telescope = new Telescope(progID);
                _telescope.Connected = true;                
            }
            else
            {
               
            }
        }

        #endregion

        #region Methods

        public Angle getAzimut ()
        {
            azimut = (Angle)_telescope.Azimuth;
            return azimut;
        }

        public Angle getDeclination()
        {
            declination = (Angle)_telescope.Declination;
            return declination;
        }

        public bool isConnected()
        {
            return _telescope.Connected;
        }

        #endregion
    }
}
