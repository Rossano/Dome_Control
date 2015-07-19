using Arduino.Dome;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TestDomeAlgMove
{
    
    
    /// <summary>
    ///This is a test class for AngleTest and is intended
    ///to contain all AngleTest Unit Tests
    ///</summary>
    [TestClass()]
    public class AngleTest
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
        ///A test for Angle Constructor
        ///</summary>
        [TestMethod()]
        public void AngleConstructorTest()
        {
            double _angle = 0F; // TODO: Initialize to an appropriate value
            Angle target = new Angle(_angle);
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for Decrement
        ///</summary>
        [TestMethod()]
        public void DecrementTest()
        {
            double _angle = 0F; // TODO: Initialize to an appropriate value
            Angle target = new Angle(_angle); // TODO: Initialize to an appropriate value
            double dec = 0F; // TODO: Initialize to an appropriate value
            target.Decrement(dec);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for GetHashCode
        ///</summary>
        [TestMethod()]
        public void GetHashCodeTest()
        {
            double _angle = 0F; // TODO: Initialize to an appropriate value
            Angle target = new Angle(_angle); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            actual = target.GetHashCode();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Increment
        ///</summary>
        [TestMethod()]
        public void IncrementTest()
        {
            double _angle = 0F; // TODO: Initialize to an appropriate value
            Angle target = new Angle(_angle); // TODO: Initialize to an appropriate value
            double inc = 0F; // TODO: Initialize to an appropriate value
            target.Increment(inc);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for ToString
        ///</summary>
        [TestMethod()]
        public void ToStringTest()
        {
            double _angle = 0F; // TODO: Initialize to an appropriate value
            Angle target = new Angle(_angle); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = target.ToString();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for op_Addition
        ///</summary>
        [TestMethod()]
        public void op_AdditionTest()
        {
            Angle lhs = null; // TODO: Initialize to an appropriate value
            Angle rhs = null; // TODO: Initialize to an appropriate value
            Angle expected = null; // TODO: Initialize to an appropriate value
            Angle actual;
            actual = (lhs + rhs);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for op_GreaterThan
        ///</summary>
        [TestMethod()]
        public void op_GreaterThanTest()
        {
            Angle lhs = null; // TODO: Initialize to an appropriate value
            Angle rhs = null; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = (lhs > rhs);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for op_GreaterThanOrEqual
        ///</summary>
        [TestMethod()]
        public void op_GreaterThanOrEqualTest()
        {
            Angle lhs = null; // TODO: Initialize to an appropriate value
            Angle rhs = null; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = (lhs >= rhs);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for op_Implicit
        ///</summary>
        [TestMethod()]
        public void op_ImplicitTest()
        {
            Angle angleObj = null; // TODO: Initialize to an appropriate value
            double expected = 0F; // TODO: Initialize to an appropriate value
            double actual;
            actual = angleObj;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for op_Implicit
        ///</summary>
        [TestMethod()]
        public void op_ImplicitTest1()
        {
            double _angle = 0F; // TODO: Initialize to an appropriate value
            Angle expected = null; // TODO: Initialize to an appropriate value
            Angle actual;
            actual = _angle;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for op_LessThan
        ///</summary>
        [TestMethod()]
        public void op_LessThanTest()
        {
            Angle lhs = null; // TODO: Initialize to an appropriate value
            Angle rhs = null; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = (lhs < rhs);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for op_LessThanOrEqual
        ///</summary>
        [TestMethod()]
        public void op_LessThanOrEqualTest()
        {
            Angle lhs = null; // TODO: Initialize to an appropriate value
            Angle rhs = null; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = (lhs <= rhs);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for op_Subtraction
        ///</summary>
        [TestMethod()]
        public void op_SubtractionTest()
        {
            Angle lhs = null; // TODO: Initialize to an appropriate value
            Angle rhs = null; // TODO: Initialize to an appropriate value
            Angle expected = null; // TODO: Initialize to an appropriate value
            Angle actual;
            actual = (lhs - rhs);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
