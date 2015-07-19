using ASCOM.Arduino;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.ComponentModel;
using ASCOM.DeviceInterface;
using System.Collections;

namespace TestDomeAlgMove
{
    
    
    /// <summary>
    ///This is a test class for DomeTest and is intended
    ///to contain all DomeTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DomeTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for Dome Constructor
        ///</summary>
        [TestMethod()]
        public void DomeConstructorTest()
        {
            Dome target = new Dome();
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for AbortSlew
        ///</summary>
        [TestMethod()]
        public void AbortSlewTest()
        {
            Dome target = new Dome(); // TODO: Initialize to an appropriate value
            target.AbortSlew();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for Action
        ///</summary>
        [TestMethod()]
        public void ActionTest()
        {
            Dome target = new Dome(); // TODO: Initialize to an appropriate value
            string actionName = string.Empty; // TODO: Initialize to an appropriate value
            string actionParameters = string.Empty; // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = target.Action(actionName, actionParameters);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for CheckConnected
        ///</summary>
        [TestMethod()]
        [DeploymentItem("ASCOM_Dome.dll")]
        public void CheckConnectedTest()
        {
            Dome_Accessor target = new Dome_Accessor(); // TODO: Initialize to an appropriate value
            string message = string.Empty; // TODO: Initialize to an appropriate value
            target.CheckConnected(message);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for CloseShutter
        ///</summary>
        [TestMethod()]
        public void CloseShutterTest()
        {
            Dome target = new Dome(); // TODO: Initialize to an appropriate value
            target.CloseShutter();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for CommandBlind
        ///</summary>
        [TestMethod()]
        public void CommandBlindTest()
        {
            Dome target = new Dome(); // TODO: Initialize to an appropriate value
            string command = string.Empty; // TODO: Initialize to an appropriate value
            bool raw = false; // TODO: Initialize to an appropriate value
            target.CommandBlind(command, raw);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for CommandBool
        ///</summary>
        [TestMethod()]
        public void CommandBoolTest()
        {
            Dome target = new Dome(); // TODO: Initialize to an appropriate value
            string command = string.Empty; // TODO: Initialize to an appropriate value
            bool raw = false; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.CommandBool(command, raw);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for CommandString
        ///</summary>
        [TestMethod()]
        public void CommandStringTest()
        {
            Dome target = new Dome(); // TODO: Initialize to an appropriate value
            string command = string.Empty; // TODO: Initialize to an appropriate value
            bool raw = false; // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = target.CommandString(command, raw);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Dispose
        ///</summary>
        [TestMethod()]
        public void DisposeTest()
        {
            Dome target = new Dome(); // TODO: Initialize to an appropriate value
            target.Dispose();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for DomeTimer_Tick
        ///</summary>
        [TestMethod()]
        [DeploymentItem("ASCOM_Dome.dll")]
        public void DomeTimer_TickTest()
        {
            Dome_Accessor target = new Dome_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            EventArgs e = null; // TODO: Initialize to an appropriate value
            target.DomeTimer_Tick(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for FindHome
        ///</summary>
        [TestMethod()]
        public void FindHomeTest()
        {
            Dome target = new Dome(); // TODO: Initialize to an appropriate value
            target.FindHome();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for OpenShutter
        ///</summary>
        [TestMethod()]
        public void OpenShutterTest()
        {
            Dome target = new Dome(); // TODO: Initialize to an appropriate value
            target.OpenShutter();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for Park
        ///</summary>
        [TestMethod()]
        public void ParkTest()
        {
            Dome target = new Dome(); // TODO: Initialize to an appropriate value
            target.Park();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for ReadProfile
        ///</summary>
        [TestMethod()]
        public void ReadProfileTest()
        {
            Dome target = new Dome(); // TODO: Initialize to an appropriate value
            target.ReadProfile();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for RegUnregASCOM
        ///</summary>
        [TestMethod()]
        [DeploymentItem("ASCOM_Dome.dll")]
        public void RegUnregASCOMTest()
        {
            bool bRegister = false; // TODO: Initialize to an appropriate value
            Dome_Accessor.RegUnregASCOM(bRegister);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for RegisterASCOM
        ///</summary>
        [TestMethod()]
        public void RegisterASCOMTest()
        {
            Type t = null; // TODO: Initialize to an appropriate value
            Dome.RegisterASCOM(t);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for SetPark
        ///</summary>
        [TestMethod()]
        public void SetParkTest()
        {
            Dome target = new Dome(); // TODO: Initialize to an appropriate value
            target.SetPark();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for SetupDialog
        ///</summary>
        [TestMethod()]
        public void SetupDialogTest()
        {
            Dome target = new Dome(); // TODO: Initialize to an appropriate value
            target.SetupDialog();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for SlewToAltitude
        ///</summary>
        [TestMethod()]
        public void SlewToAltitudeTest()
        {
            Dome target = new Dome(); // TODO: Initialize to an appropriate value
            double Altitude = 0F; // TODO: Initialize to an appropriate value
            target.SlewToAltitude(Altitude);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for SlewToAzimuth
        ///</summary>
        [TestMethod()]
        public void SlewToAzimuthTest()
        {
            Dome target = new Dome(); // TODO: Initialize to an appropriate value
            double Azimuth = 0F; // TODO: Initialize to an appropriate value
            target.SlewToAzimuth(Azimuth);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for Stop
        ///</summary>
        [TestMethod()]
        public void StopTest()
        {
            Dome target = new Dome(); // TODO: Initialize to an appropriate value
            target.Stop();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for SyncToAzimuth
        ///</summary>
        [TestMethod()]
        public void SyncToAzimuthTest()
        {
            Dome target = new Dome(); // TODO: Initialize to an appropriate value
            double Azimuth = 0F; // TODO: Initialize to an appropriate value
            target.SyncToAzimuth(Azimuth);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for TurnLeft
        ///</summary>
        [TestMethod()]
        public void TurnLeftTest()
        {
            Dome target = new Dome(); // TODO: Initialize to an appropriate value
            target.TurnLeft();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for TurnRight
        ///</summary>
        [TestMethod()]
        public void TurnRightTest()
        {
            Dome target = new Dome(); // TODO: Initialize to an appropriate value
            target.TurnRight();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for UnregisterASCOM
        ///</summary>
        [TestMethod()]
        public void UnregisterASCOMTest()
        {
            Type t = null; // TODO: Initialize to an appropriate value
            Dome.UnregisterASCOM(t);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for UnsyncToAzimuth
        ///</summary>
        [TestMethod()]
        public void UnsyncToAzimuthTest()
        {
            Dome target = new Dome(); // TODO: Initialize to an appropriate value
            target.UnsyncToAzimuth();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for WriteProfile
        ///</summary>
        [TestMethod()]
        public void WriteProfileTest()
        {
            Dome target = new Dome(); // TODO: Initialize to an appropriate value
            target.WriteProfile();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for configureFirmware
        ///</summary>
        [TestMethod()]
        public void configureFirmwareTest()
        {
            Dome target = new Dome(); // TODO: Initialize to an appropriate value
            target.configureFirmware();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for getArduinoPortName
        ///</summary>
        [TestMethod()]
        public void getArduinoPortNameTest()
        {
            Dome target = new Dome(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = target.getArduinoPortName();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for slewThread_Body
        ///</summary>
        [TestMethod()]
        [DeploymentItem("ASCOM_Dome.dll")]
        public void slewThread_BodyTest()
        {
            Dome_Accessor target = new Dome_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            DoWorkEventArgs e = null; // TODO: Initialize to an appropriate value
            target.slewThread_Body(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for slewThread_Completed
        ///</summary>
        [TestMethod()]
        [DeploymentItem("ASCOM_Dome.dll")]
        public void slewThread_CompletedTest()
        {
            Dome_Accessor target = new Dome_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            RunWorkerCompletedEventArgs e = null; // TODO: Initialize to an appropriate value
            target.slewThread_Completed(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for Altitude
        ///</summary>
        [TestMethod()]
        public void AltitudeTest()
        {
            Dome target = new Dome(); // TODO: Initialize to an appropriate value
            double actual;
            actual = target.Altitude;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for AtHome
        ///</summary>
        [TestMethod()]
        public void AtHomeTest()
        {
            Dome target = new Dome(); // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.AtHome;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for AtPark
        ///</summary>
        [TestMethod()]
        public void AtParkTest()
        {
            Dome target = new Dome(); // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.AtPark;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Azimuth
        ///</summary>
        [TestMethod()]
        public void AzimuthTest()
        {
            Dome target = new Dome(); // TODO: Initialize to an appropriate value
            double actual;
            actual = target.Azimuth;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for CanFindHome
        ///</summary>
        [TestMethod()]
        public void CanFindHomeTest()
        {
            Dome target = new Dome(); // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.CanFindHome;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for CanPark
        ///</summary>
        [TestMethod()]
        public void CanParkTest()
        {
            Dome target = new Dome(); // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.CanPark;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for CanSetAltitude
        ///</summary>
        [TestMethod()]
        public void CanSetAltitudeTest()
        {
            Dome target = new Dome(); // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.CanSetAltitude;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for CanSetAzimuth
        ///</summary>
        [TestMethod()]
        public void CanSetAzimuthTest()
        {
            Dome target = new Dome(); // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.CanSetAzimuth;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for CanSetPark
        ///</summary>
        [TestMethod()]
        public void CanSetParkTest()
        {
            Dome target = new Dome(); // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.CanSetPark;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for CanSetShutter
        ///</summary>
        [TestMethod()]
        public void CanSetShutterTest()
        {
            Dome target = new Dome(); // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.CanSetShutter;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for CanSlave
        ///</summary>
        [TestMethod()]
        public void CanSlaveTest()
        {
            Dome target = new Dome(); // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.CanSlave;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for CanSyncAzimuth
        ///</summary>
        [TestMethod()]
        public void CanSyncAzimuthTest()
        {
            Dome target = new Dome(); // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.CanSyncAzimuth;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Connected
        ///</summary>
        [TestMethod()]
        public void ConnectedTest()
        {
            Dome target = new Dome(); // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            target.Connected = expected;
            actual = target.Connected;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Description
        ///</summary>
        [TestMethod()]
        public void DescriptionTest()
        {
            Dome target = new Dome(); // TODO: Initialize to an appropriate value
            string actual;
            actual = target.Description;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for DriverInfo
        ///</summary>
        [TestMethod()]
        public void DriverInfoTest()
        {
            Dome target = new Dome(); // TODO: Initialize to an appropriate value
            string actual;
            actual = target.DriverInfo;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for DriverVersion
        ///</summary>
        [TestMethod()]
        public void DriverVersionTest()
        {
            Dome target = new Dome(); // TODO: Initialize to an appropriate value
            string actual;
            actual = target.DriverVersion;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for InterfaceVersion
        ///</summary>
        [TestMethod()]
        public void InterfaceVersionTest()
        {
            Dome target = new Dome(); // TODO: Initialize to an appropriate value
            short actual;
            actual = target.InterfaceVersion;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for IsConnected
        ///</summary>
        [TestMethod()]
        [DeploymentItem("ASCOM_Dome.dll")]
        public void IsConnectedTest()
        {
            Dome_Accessor target = new Dome_Accessor(); // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.IsConnected;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Name
        ///</summary>
        [TestMethod()]
        public void NameTest()
        {
            Dome target = new Dome(); // TODO: Initialize to an appropriate value
            string actual;
            actual = target.Name;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for ShutterStatus
        ///</summary>
        [TestMethod()]
        public void ShutterStatusTest()
        {
            Dome target = new Dome(); // TODO: Initialize to an appropriate value
            ShutterState actual;
            actual = target.ShutterStatus;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Slaved
        ///</summary>
        [TestMethod()]
        public void SlavedTest()
        {
            Dome target = new Dome(); // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            target.Slaved = expected;
            actual = target.Slaved;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for SleewingSleepTime
        ///</summary>
        [TestMethod()]
        public void SleewingSleepTimeTest()
        {
            Dome target = new Dome(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.SleewingSleepTime = expected;
            actual = target.SleewingSleepTime;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Slewing
        ///</summary>
        [TestMethod()]
        public void SlewingTest()
        {
            Dome target = new Dome(); // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.Slewing;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for SupportedActions
        ///</summary>
        [TestMethod()]
        public void SupportedActionsTest()
        {
            Dome target = new Dome(); // TODO: Initialize to an appropriate value
            ArrayList actual;
            actual = target.SupportedActions;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Synced
        ///</summary>
        [TestMethod()]
        public void SyncedTest()
        {
            Dome target = new Dome(); // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            target.Synced = expected;
            actual = target.Synced;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Threshold
        ///</summary>
        [TestMethod()]
        public void ThresholdTest()
        {
            Dome target = new Dome(); // TODO: Initialize to an appropriate value
            double expected = 0F; // TODO: Initialize to an appropriate value
            double actual;
            target.Threshold = expected;
            actual = target.Threshold;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for dome_angular_speed
        ///</summary>
        [TestMethod()]
        public void dome_angular_speedTest()
        {
            Dome target = new Dome(); // TODO: Initialize to an appropriate value
            double expected = 0F; // TODO: Initialize to an appropriate value
            double actual;
            target.dome_angular_speed = expected;
            actual = target.dome_angular_speed;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for dome_gear_ratio
        ///</summary>
        [TestMethod()]
        public void dome_gear_ratioTest()
        {
            Dome target = new Dome(); // TODO: Initialize to an appropriate value
            double expected = 0F; // TODO: Initialize to an appropriate value
            double actual;
            target.dome_gear_ratio = expected;
            actual = target.dome_gear_ratio;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for encoder_resolution
        ///</summary>
        [TestMethod()]
        public void encoder_resolutionTest()
        {
            Dome target = new Dome(); // TODO: Initialize to an appropriate value
            uint expected = 0; // TODO: Initialize to an appropriate value
            uint actual;
            target.encoder_resolution = expected;
            actual = target.encoder_resolution;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for motor_accelleration_time
        ///</summary>
        [TestMethod()]
        public void motor_accelleration_timeTest()
        {
            Dome target = new Dome(); // TODO: Initialize to an appropriate value
            uint expected = 0; // TODO: Initialize to an appropriate value
            uint actual;
            target.motor_accelleration_time = expected;
            actual = target.motor_accelleration_time;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
