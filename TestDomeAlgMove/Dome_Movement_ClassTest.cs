using Arduino.Dome;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TestDomeAlgMove
{
    
    
    /// <summary>
    ///This is a test class for Dome_Movement_ClassTest and is intended
    ///to contain all Dome_Movement_ClassTest Unit Tests
    ///</summary>
    [TestClass()]
    public class Dome_Movement_ClassTest
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
        ///A test for Dome_Movement_Class Constructor
        ///</summary>
        [TestMethod()]
        public void Dome_Movement_ClassConstructorTest()
        {
            double cur_pos = 50F; // TODO: Initialize to an appropriate value
            Dome_Movement_Class target = new Dome_Movement_Class(cur_pos);
//            Assert.Inconclusive("TODO: Implement code to verify target");//
            Assert.AreEqual(target._current_pos, cur_pos, 0.1, "Construtor passing element fails");
        }

        /// <summary>
        ///A test for find_rotation_sense
        ///</summary>
        [TestMethod()]
        public void find_rotation_senseTest()
        {
//            double cur_pos = 0F; // TODO: Initialize to an appropriate value
//            Dome_Movement_Class target = new Dome_Movement_Class(cur_pos); // TODO: Initialize to an appropriate value
//            double target1 = 0F; // TODO: Initialize to an appropriate value
//            Status expected = new Status(); // TODO: Initialize to an appropriate value
//            Status actual;
//            actual = target.find_rotation_sense(target1);
//            Assert.AreEqual(expected, actual);
//            Assert.Inconclusive("Verify the correctness of this test method.");			
			for (int i=0; i<9; i++)
			{
				Randon rnd = new Random();
				double cur_pos = rnd.NextDouble() * 360;
				double target_pos = rnd.NextDouble() * 360;
				Status actual;
				Status expected;
				Dome_Movement_Class target = new Dome_Movement_Class(cur_pos);
				actual = target.find_rotation_sense(target_pos);
				
				double theta = target_pos;
				int left = 0;
				int right = 0;
				
				while (Math.Abs(thetha - cur_pos) > 10.0)
				{
					if (theta > 350)
					{
						theta += 10.0 - 360;
					}
					else {
						theta += 10.0;
					}
					left++;
				}
				while (Math.Abs(cur_pos - theta) > 10.0)
				{
					if (theta < 10.0)
					{
						theta -= 360 - theta;
					}
					else {
						theta -= 10.0;
					}
					right++;
				}
				
				if(left == right) expected = Status.NO_TURN;
				else if (left > right) expected = Status.TURN_RIGHT;
				else expected = Status.TURN_LEFT;
				
				Assert.AreEqual(expected, actual);
			}
        }

        /// <summary>
        ///A test for _current_pos
        ///</summary>
        [TestMethod()]
        public void _current_posTest()
        {
            double cur_pos = 0F; // TODO: Initialize to an appropriate value
            Dome_Movement_Class target = new Dome_Movement_Class(cur_pos); // TODO: Initialize to an appropriate value
            double expected = 50F; // TODO: Initialize to an appropriate value
            double actual;
            target._current_pos = expected;
            actual = target._current_pos;
            Assert.AreEqual(expected, actual, 0.1,"Failed to assigned the current position");
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for _target_pos
        ///</summary>
        [TestMethod()]
        public void _target_posTest()
        {
            double cur_pos = 0F; // TODO: Initialize to an appropriate value
            Dome_Movement_Class target = new Dome_Movement_Class(cur_pos); // TODO: Initialize to an appropriate value
            double expected = 100F; // TODO: Initialize to an appropriate value
            double actual;
            target._target_pos = expected;
            actual = target._target_pos;
            Assert.AreEqual(expected, actual, 0.1, "Failed to assign the expected value");
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for _target_pos error
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void _target_pos_error_Test()
        {
            double cur_pos = 0F; // TODO: Initialize to an appropriate value
            Dome_Movement_Class target = new Dome_Movement_Class(cur_pos); // TODO: Initialize to an appropriate value
            double expected = 360F; // TODO: Initialize to an appropriate value
            double actual;
            target._target_pos = expected;
            actual = target._target_pos;
            Assert.AreEqual(expected, actual, 0.1, "Failed to assign the expected value");
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
