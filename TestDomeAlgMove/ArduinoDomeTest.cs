using Arduino.Dome;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TestDomeAlgMove
{
    
    
    /// <summary>
    ///This is a test class for ArduinoDomeTest and is intended
    ///to contain all ArduinoDomeTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ArduinoDomeTest
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
        ///A test for ArduinoDome Constructor
        ///</summary>
        [TestMethod()]
        public void ArduinoDomeConstructorTest()
        {
            string com = string.Empty; // TODO: Initialize to an appropriate value
            bool flag = false; // TODO: Initialize to an appropriate value
            ArduinoDome target = new ArduinoDome(com, flag);
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for ArduinoDome Constructor
        ///</summary>
        [TestMethod()]
        public void ArduinoDomeConstructorTest1()
        {
            string com = string.Empty; // TODO: Initialize to an appropriate value
            ArduinoDome target = new ArduinoDome(com);
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for ArduinoDome Constructor
        ///</summary>
        [TestMethod()]
        public void ArduinoDomeConstructorTest2()
        {
            ArduinoDome target = new ArduinoDome();
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for BuildArduinoCommand
        ///</summary>
        [TestMethod()]
        public void BuildArduinoCommandTest()
        {
            ArduinoDome target = new ArduinoDome(); // TODO: Initialize to an appropriate value
            string cmd = string.Empty; // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = target.BuildArduinoCommand(cmd);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for BuildArduinoCommand
        ///</summary>
        [TestMethod()]
        public void BuildArduinoCommandTest1()
        {
            ArduinoDome target = new ArduinoDome(); // TODO: Initialize to an appropriate value
            string cmd = string.Empty; // TODO: Initialize to an appropriate value
            string args = string.Empty; // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = target.BuildArduinoCommand(cmd, args);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Connect
        ///</summary>
        [TestMethod()]
        public void ConnectTest()
        {
            ArduinoDome target = new ArduinoDome(); // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.Connect();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Disconnect
        ///</summary>
        [TestMethod()]
        public void DisconnectTest()
        {
            ArduinoDome target = new ArduinoDome(); // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.Disconnect();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Dispose
        ///</summary>
        [TestMethod()]
        public void DisposeTest()
        {
            ArduinoDome target = new ArduinoDome(); // TODO: Initialize to an appropriate value
            target.Dispose();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for GearConfig
        ///</summary>
        [TestMethod()]
        public void GearConfigTest()
        {
            ArduinoDome target = new ArduinoDome(); // TODO: Initialize to an appropriate value
            uint enc = 0; // TODO: Initialize to an appropriate value
            double gear = 0F; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.GearConfig(enc, gear);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetAck
        ///</summary>
        [TestMethod()]
        public void GetAckTest()
        {
            ArduinoDome target = new ArduinoDome(); // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.GetAck();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetVersion
        ///</summary>
        [TestMethod()]
        public void GetVersionTest()
        {
            ArduinoDome target = new ArduinoDome(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = target.GetVersion();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Init
        ///</summary>
        [TestMethod()]
        public void InitTest()
        {
            ArduinoDome target = new ArduinoDome(); // TODO: Initialize to an appropriate value
            target.Init();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for SendCommand
        ///</summary>
        [TestMethod()]
        [DeploymentItem("ArduinoDome.dll")]
        public void SendCommandTest()
        {
            ArduinoDome_Accessor target = new ArduinoDome_Accessor(); // TODO: Initialize to an appropriate value
            MessageData _msg = new MessageData(); // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.SendCommand(_msg);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for SendCommand
        ///</summary>
        [TestMethod()]
        public void SendCommandTest1()
        {
            ArduinoDome target = new ArduinoDome(); // TODO: Initialize to an appropriate value
            string cmd = string.Empty; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.SendCommand(cmd);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Slew
        ///</summary>
        [TestMethod()]
        public void SlewTest()
        {
            ArduinoDome target = new ArduinoDome(); // TODO: Initialize to an appropriate value
            Direction dir = new Direction(); // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.Slew(dir);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Stop
        ///</summary>
        [TestMethod()]
        public void StopTest()
        {
            ArduinoDome target = new ArduinoDome(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = target.Stop();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for clearArduinoDebugMode
        ///</summary>
        [TestMethod()]
        public void clearArduinoDebugModeTest()
        {
            ArduinoDome target = new ArduinoDome(); // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.clearArduinoDebugMode();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for getArduinoPortName
        ///</summary>
        [TestMethod()]
        public void getArduinoPortNameTest()
        {
            ArduinoDome target = new ArduinoDome(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = target.getArduinoPortName();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for get_debug_mode
        ///</summary>
        [TestMethod()]
        public void get_debug_modeTest()
        {
            ArduinoDome target = new ArduinoDome(); // TODO: Initialize to an appropriate value
            DebugMode expected = new DebugMode(); // TODO: Initialize to an appropriate value
            DebugMode actual;
            actual = target.get_debug_mode();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for get_encoder_pol
        ///</summary>
        [TestMethod()]
        public void get_encoder_polTest()
        {
            ArduinoDome target = new ArduinoDome(); // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.get_encoder_pol();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for setArduinoDebugMode
        ///</summary>
        [TestMethod()]
        public void setArduinoDebugModeTest()
        {
            ArduinoDome target = new ArduinoDome(); // TODO: Initialize to an appropriate value
            string mode = string.Empty; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.setArduinoDebugMode(mode);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for set_debug_mode
        ///</summary>
        [TestMethod()]
        public void set_debug_modeTest()
        {
            ArduinoDome target = new ArduinoDome(); // TODO: Initialize to an appropriate value
            DebugMode mode = new DebugMode(); // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.set_debug_mode(mode);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for set_encoder_pol
        ///</summary>
        [TestMethod()]
        public void set_encoder_polTest()
        {
            ArduinoDome target = new ArduinoDome(); // TODO: Initialize to an appropriate value
            uint pol = 0; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.set_encoder_pol(pol);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for DomePosition
        ///</summary>
        [TestMethod()]
        public void DomePositionTest()
        {
            ArduinoDome target = new ArduinoDome(); // TODO: Initialize to an appropriate value
            double expected = 0F; // TODO: Initialize to an appropriate value
            double actual;
            target.DomePosition = expected;
            actual = target.DomePosition;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for SlewDirection
        ///</summary>
        [TestMethod()]
        public void SlewDirectionTest()
        {
            ArduinoDome target = new ArduinoDome(); // TODO: Initialize to an appropriate value
            Direction expected = new Direction(); // TODO: Initialize to an appropriate value
            Direction actual;
            target.SlewDirection = expected;
            actual = target.SlewDirection;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Version
        ///</summary>
        [TestMethod()]
        public void VersionTest()
        {
            ArduinoDome target = new ArduinoDome(); // TODO: Initialize to an appropriate value
            string actual;
            actual = target.Version;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
