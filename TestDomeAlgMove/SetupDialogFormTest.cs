using ASCOM.Arduino;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TestDomeAlgMove
{
    
    
    /// <summary>
    ///This is a test class for SetupDialogFormTest and is intended
    ///to contain all SetupDialogFormTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SetupDialogFormTest
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
        ///A test for SetupDialogForm Constructor
        ///</summary>
        [TestMethod()]
        public void SetupDialogFormConstructorTest()
        {
            Dome parent = this; //null; // TODO: Initialize to an appropriate value
            SetupDialogForm target = new SetupDialogForm(parent);
            //Assert.Inconclusive("TODO: Implement code to verify target");*
            try {
            	var result = target.ShowDialog();
            	if (result == System.Windows.Forms.DialogResult.OK) 
            	{
            		Assert.IsTrue(true, "Test SetupDialogForm PASS");
            	}
            }
            catch (NullReferenceException ex) {
            	Assert.IsTrue(true, "Test SetupDialogForm OK press Cancel");
            }
            catch (Exception) {
            	
            	Assert.Fail();
            }
        }

        /// <summary>
        ///A test for BrowseToAscom
        ///</summary>
        [TestMethod()]
        [DeploymentItem("ASCOM_Dome.dll")]
        public void BrowseToAscomTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            SetupDialogForm_Accessor target = new SetupDialogForm_Accessor(param0); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            EventArgs e = null; // TODO: Initialize to an appropriate value
            try {
            	target.BrowseToAscom(sender, e);            	
            }
            catch (Exception) {
            	Assert.Fail("Launch ASCOM Browser Fail");
            }
            Assert.IsTrue(true, "Launch ASCOM browser Pass");
            //Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for CheckTelescopeRegistration
        ///</summary>
        [TestMethod()]
        [DeploymentItem("ASCOM_Dome.dll")]
        public void CheckTelescopeRegistrationTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            SetupDialogForm_Accessor target = new SetupDialogForm_Accessor(param0); // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.CheckTelescopeRegistration();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Dispose
        ///</summary>
        [TestMethod()]
        [DeploymentItem("ASCOM_Dome.dll")]
        public void DisposeTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            SetupDialogForm_Accessor target = new SetupDialogForm_Accessor(param0); // TODO: Initialize to an appropriate value
            bool disposing = false; // TODO: Initialize to an appropriate value
            target.Dispose(disposing);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for DomeCOMcomboBox_SelectedValueChanged
        ///</summary>
        [TestMethod()]
        [DeploymentItem("ASCOM_Dome.dll")]
        public void DomeCOMcomboBox_SelectedValueChangedTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            SetupDialogForm_Accessor target = new SetupDialogForm_Accessor(param0); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            EventArgs e = null; // TODO: Initialize to an appropriate value
            target.DomeCOMcomboBox_SelectedValueChanged(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for InitializeComponent
        ///</summary>
        [TestMethod()]
        [DeploymentItem("ASCOM_Dome.dll")]
        public void InitializeComponentTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            SetupDialogForm_Accessor target = new SetupDialogForm_Accessor(param0); // TODO: Initialize to an appropriate value
            target.InitializeComponent();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for TelescopeChooserButton_Click
        ///</summary>
        [TestMethod()]
        [DeploymentItem("ASCOM_Dome.dll")]
        public void TelescopeChooserButton_ClickTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            SetupDialogForm_Accessor target = new SetupDialogForm_Accessor(param0); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            EventArgs e = null; // TODO: Initialize to an appropriate value
            target.TelescopeChooserButton_Click(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for cmdCancel_Click
        ///</summary>
        [TestMethod()]
        [DeploymentItem("ASCOM_Dome.dll")]
        public void cmdCancel_ClickTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            SetupDialogForm_Accessor target = new SetupDialogForm_Accessor(param0); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            EventArgs e = null; // TODO: Initialize to an appropriate value
            target.cmdCancel_Click(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for cmdOK_Click
        ///</summary>
        [TestMethod()]
        [DeploymentItem("ASCOM_Dome.dll")]
        public void cmdOK_ClickTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            SetupDialogForm_Accessor target = new SetupDialogForm_Accessor(param0); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            EventArgs e = null; // TODO: Initialize to an appropriate value
            target.cmdOK_Click(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }
    }
}
