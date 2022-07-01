using BinaryTree;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections;
using StressTestResult;

namespace TestBinaryTree
{


    /// <summary>
    ///This is a test class for TreeTest and is intended
    ///to contain all TreeTest Unit Tests
    ///</summary>
    [TestClass()]
    public class TreeTest
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
        /// Creates a tree of TestResults (Deflection value shown in diagram).
        ///             190
        ///         /        \
        ///     114             266
        ///    /   \           /   \
        ///   0    114       190   342
        ///     \                  /
        ///     76               304
        ///     /
        ///    38
        /// </summary>
        /// <returns>Tree of TestResults.</returns>
        private static Tree<TestResult> CreateATreeOfTestResults()
        {
            Tree<TestResult> tree = new Tree<TestResult>(new TestResult() { Deflection = 190, AppliedStress = 60, Temperature = 200, TestDate = DateTime.Now });
            tree.Add(new TestResult() { Deflection = 114, AppliedStress = 40, Temperature = 200, TestDate = DateTime.Now });
            tree.Add(new TestResult() { Deflection = 266, AppliedStress = 80, Temperature = 200, TestDate = DateTime.Now });
            tree.Add(new TestResult() { Deflection = 114, AppliedStress = 50, Temperature = 200, TestDate = DateTime.Now });
            tree.Add(new TestResult() { Deflection = 0, AppliedStress = 10, Temperature = 200, TestDate = DateTime.Now });
            tree.Add(new TestResult() { Deflection = 342, AppliedStress = 100, Temperature = 200, TestDate = DateTime.Now });
            tree.Add(new TestResult() { Deflection = 76, AppliedStress = 30, Temperature = 200, TestDate = DateTime.Now });
            tree.Add(new TestResult() { Deflection = 304, AppliedStress = 90, Temperature = 200, TestDate = DateTime.Now });
            tree.Add(new TestResult() { Deflection = 38, AppliedStress = 20, Temperature = 200, TestDate = DateTime.Now });
            tree.Add(new TestResult() { Deflection = 190, AppliedStress = 70, Temperature = 200, TestDate = DateTime.Now });
            return tree;
        }

        // TestResult objects
        TestResult deflection0 = new TestResult() { Deflection = 0, AppliedStress = 10, Temperature = 200, TestDate = DateTime.Now };
        TestResult deflection38 = new TestResult() { Deflection = 38, AppliedStress = 20, Temperature = 200, TestDate = DateTime.Now };
        TestResult deflection76 = new TestResult() { Deflection = 76, AppliedStress = 30, Temperature = 200, TestDate = DateTime.Now };
        TestResult deflection114a = new TestResult() { Deflection = 114, AppliedStress = 40, Temperature = 200, TestDate = DateTime.Now };
        TestResult deflection114b = new TestResult() { Deflection = 114, AppliedStress = 50, Temperature = 200, TestDate = DateTime.Now };
        TestResult deflection190a = new TestResult() { Deflection = 190, AppliedStress = 60, Temperature = 200, TestDate = DateTime.Now };
        TestResult deflection190b = new TestResult() { Deflection = 190, AppliedStress = 70, Temperature = 200, TestDate = DateTime.Now };
        TestResult deflection266 = new TestResult() { Deflection = 266, AppliedStress = 80, Temperature = 200, TestDate = DateTime.Now };
        TestResult deflection304 = new TestResult() { Deflection = 304, AppliedStress = 90, Temperature = 200, TestDate = DateTime.Now };
        TestResult deflection342 = new TestResult() { Deflection = 342, AppliedStress = 100, Temperature = 200, TestDate = DateTime.Now };
        TestResult deflection999 = new TestResult() { Deflection = 999, AppliedStress = 999, Temperature = 200, TestDate = DateTime.Now };


        /// <summary>
        ///A test for System.Collections.Generic.ICollection<TItem>.Add
        ///</summary>
        [TestMethod()]
        [DeploymentItem("BinaryTree.dll")]
        public void AddTest()
        {
            Tree<TestResult> tree = new Tree<TestResult>(new TestResult() { Deflection = 190, AppliedStress = 60, Temperature = 200, TestDate = DateTime.Now });
            ICollection<TestResult> target = (ICollection<TestResult>)tree;
            target.Add(new TestResult() { Deflection = 114, AppliedStress = 40, Temperature = 200, TestDate = DateTime.Now });
            target.Add(new TestResult() { Deflection = 266, AppliedStress = 80, Temperature = 200, TestDate = DateTime.Now });
            target.Add(new TestResult() { Deflection = 114, AppliedStress = 50, Temperature = 200, TestDate = DateTime.Now });
            target.Add(new TestResult() { Deflection = 0, AppliedStress = 10, Temperature = 200, TestDate = DateTime.Now });
            target.Add(new TestResult() { Deflection = 342, AppliedStress = 100, Temperature = 200, TestDate = DateTime.Now });
            target.Add(new TestResult() { Deflection = 76, AppliedStress = 30, Temperature = 200, TestDate = DateTime.Now });
            target.Add(new TestResult() { Deflection = 304, AppliedStress = 90, Temperature = 200, TestDate = DateTime.Now });
            target.Add(new TestResult() { Deflection = 38, AppliedStress = 20, Temperature = 200, TestDate = DateTime.Now });
            target.Add(new TestResult() { Deflection = 190, AppliedStress = 70, Temperature = 200, TestDate = DateTime.Now });
            int expected = 10;
            int actual = target.Count;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Clear
        ///</summary>
        [TestMethod()]
        public void ClearTest()
        {
            Tree<TestResult> target = CreateATreeOfTestResults();
            target.Clear();
            int expected = 1;
            int actual = target.Count;
            TestResult expectedValue = default(TestResult);
            TestResult actualValue = target.NodeData;
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(expectedValue, actualValue);
        }


        /// <summary>
        ///A test for Contains where the tree contains the value
        ///</summary>
        [TestMethod()]
        public void ContainsTest1()
        {
            Tree<TestResult> target = CreateATreeOfTestResults();
            bool actual = target.Contains(deflection190a);
            Assert.IsTrue(actual);
        }

        /// <summary>
        ///A test for Contains where the tree does not contain the value
        ///</summary>
        [TestMethod()]
        public void ContainsTest2()
        {
            Tree<TestResult> target = CreateATreeOfTestResults();
            bool actual = target.Contains(deflection999);
            Assert.IsFalse(actual);
        }

        /// <summary>
        ///A test for CopyTo - Not Implemented
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(NotImplementedException))]
        public void CopyToTest()
        {
            Tree<TestResult> target = CreateATreeOfTestResults();
            TestResult[] array = new TestResult[10];
            target.CopyTo(array, 0);
        }


        /// <summary>
        ///A test for IndexOf where item exists
        ///</summary>
        [TestMethod()]
        public void IndexOfTest()
        {
            Tree<TestResult> target = CreateATreeOfTestResults();
            int expected = 3;
            int actual = target.IndexOf(deflection114a);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Insert - Not implemented
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(NotImplementedException))]
        public void InsertTest()
        {
            Tree<TestResult> target = CreateATreeOfTestResults();
            target.Insert(3, deflection999);
        }


        /// <summary>
        ///A test for System.Collections.Generic.ICollection<TItem>.Remove
        ///Remove the root node of the tree where root has no children
        ///</summary>
        [TestMethod()]
        [DeploymentItem("BinaryTree.dll")]
        public void RemoveRootNoChildren()
        {
            Tree<TestResult> tree = new Tree<TestResult>(new TestResult() { Deflection = 190, AppliedStress = 60, Temperature = 200, TestDate = DateTime.Now });
            ICollection<TestResult> target = (ICollection<TestResult>)tree;
            Assert.IsTrue(target.Remove(deflection190a));
            Assert.AreEqual(1, target.Count);
            Assert.AreEqual(deflection190a.Deflection, tree.NodeData.Deflection);
        }

        /// <summary>
        ///A test for System.Collections.Generic.ICollection<TItem>.Remove
        ///Remove the root node of the tree where root has one child
        ///</summary>
        [TestMethod()]
        [DeploymentItem("BinaryTree.dll")]
        public void RemoveRootOneChild()
        {
            Tree<TestResult> tree = new Tree<TestResult>(new TestResult() { Deflection = 190, AppliedStress = 60, Temperature = 200, TestDate = DateTime.Now });
            tree.Add(new TestResult() { Deflection = 114, AppliedStress = 40, Temperature = 200, TestDate = DateTime.Now });
            tree.Add(new TestResult() { Deflection = 114, AppliedStress = 50, Temperature = 200, TestDate = DateTime.Now });
            tree.Add(new TestResult() { Deflection = 0, AppliedStress = 10, Temperature = 200, TestDate = DateTime.Now });
            tree.Add(new TestResult() { Deflection = 76, AppliedStress = 30, Temperature = 200, TestDate = DateTime.Now });
            tree.Add(new TestResult() { Deflection = 38, AppliedStress = 20, Temperature = 200, TestDate = DateTime.Now });
            ICollection<TestResult> target = (ICollection<TestResult>)tree;
            Assert.IsTrue(target.Remove(deflection190a));
            Assert.AreEqual(5, target.Count);
            Assert.AreEqual(deflection114a.Deflection, tree.NodeData.Deflection);
            Assert.AreEqual(deflection0.Deflection, tree[0].Deflection);
            Assert.AreEqual(deflection38.Deflection, tree[1].Deflection);
            Assert.AreEqual(deflection76.Deflection, tree[2].Deflection);
            Assert.AreEqual(deflection114a.Deflection, tree[3].Deflection);
            Assert.AreEqual(deflection114b.Deflection, tree[4].Deflection);
        }

        /// <summary>
        ///A test for System.Collections.Generic.ICollection<TItem>.Remove
        ///Remove the root node of the tree where root has two children
        ///</summary>
        [TestMethod()]
        [DeploymentItem("BinaryTree.dll")]
        public void RemoveRootTwoChildren()
        {
            Tree<TestResult> tree = CreateATreeOfTestResults();
            ICollection<TestResult> target = (ICollection<TestResult>)tree;
            Assert.IsTrue(target.Remove(deflection190a));
            Assert.AreEqual(9, target.Count);
            Assert.AreEqual(deflection190b.Deflection, tree.NodeData.Deflection);
            Assert.AreEqual(deflection0.Deflection, tree[0].Deflection);
            Assert.AreEqual(deflection38.Deflection, tree[1].Deflection);
            Assert.AreEqual(deflection76.Deflection, tree[2].Deflection);
            Assert.AreEqual(deflection114a.Deflection, tree[3].Deflection);
            Assert.AreEqual(deflection114b.Deflection, tree[4].Deflection);
            Assert.AreEqual(deflection190b.Deflection, tree[5].Deflection);
            Assert.AreEqual(deflection266.Deflection, tree[6].Deflection);
            Assert.AreEqual(deflection304.Deflection, tree[7].Deflection);
            Assert.AreEqual(deflection342.Deflection, tree[8].Deflection);
        }

        /// <summary>
        ///A test for System.Collections.Generic.ICollection<TItem>.Remove
        ///Remove a node of the tree where the node has no children
        ///</summary>
        [TestMethod()]
        [DeploymentItem("BinaryTree.dll")]
        public void RemoveNodeNoChildren()
        {
            Tree<TestResult> tree = CreateATreeOfTestResults();
            ICollection<TestResult> target = (ICollection<TestResult>)tree;
            Assert.IsTrue(target.Remove(deflection304));
            Assert.AreEqual(9, target.Count);
            Assert.AreEqual(deflection190a.Deflection, tree.NodeData.Deflection);
            Assert.AreEqual(deflection0.Deflection, tree[0].Deflection);
            Assert.AreEqual(deflection38.Deflection, tree[1].Deflection);
            Assert.AreEqual(deflection76.Deflection, tree[2].Deflection);
            Assert.AreEqual(deflection114a.Deflection, tree[3].Deflection);
            Assert.AreEqual(deflection114b.Deflection, tree[4].Deflection);
            Assert.AreEqual(deflection190a.Deflection, tree[5].Deflection);
            Assert.AreEqual(deflection190b.Deflection, tree[6].Deflection);
            Assert.AreEqual(deflection266.Deflection, tree[7].Deflection);
            Assert.AreEqual(deflection342.Deflection, tree[8].Deflection);
        }

        /// <summary>
        ///A test for System.Collections.Generic.ICollection<TItem>.Remove
        ///Remove a node of the tree where the node has one child
        ///</summary>
        [TestMethod()]
        [DeploymentItem("BinaryTree.dll")]
        public void RemoveNodeOneChild()
        {
            Tree<TestResult> tree = CreateATreeOfTestResults();
            ICollection<TestResult> target = (ICollection<TestResult>)tree;
            Assert.IsTrue(target.Remove(deflection76));
            Assert.AreEqual(9, target.Count);
            Assert.AreEqual(deflection190a.Deflection, tree.NodeData.Deflection);
            Assert.AreEqual(deflection0.Deflection, tree[0].Deflection);
            Assert.AreEqual(deflection38.Deflection, tree[1].Deflection);
            Assert.AreEqual(deflection114a.Deflection, tree[2].Deflection);
            Assert.AreEqual(deflection114b.Deflection, tree[3].Deflection);
            Assert.AreEqual(deflection190a.Deflection, tree[4].Deflection);
            Assert.AreEqual(deflection190b.Deflection, tree[5].Deflection);
            Assert.AreEqual(deflection266.Deflection, tree[6].Deflection);
            Assert.AreEqual(deflection304.Deflection, tree[7].Deflection);
            Assert.AreEqual(deflection342.Deflection, tree[8].Deflection);
        }

        /// <summary>
        ///A test for System.Collections.Generic.ICollection<TItem>.Remove
        ///Remove a node of the tree where the node has two children
        ///</summary>
        [TestMethod()]
        [DeploymentItem("BinaryTree.dll")]
        public void RemoveNodeTwoChildren()
        {
            Tree<TestResult> tree = CreateATreeOfTestResults();
            ICollection<TestResult> target = (ICollection<TestResult>)tree;
            Assert.IsTrue(target.Remove(deflection266));
            Assert.AreEqual(9, target.Count);
            Assert.AreEqual(deflection190a.Deflection, tree.NodeData.Deflection);
            Assert.AreEqual(deflection0.Deflection, tree[0].Deflection);
            Assert.AreEqual(deflection38.Deflection, tree[1].Deflection);
            Assert.AreEqual(deflection76.Deflection, tree[2].Deflection);
            Assert.AreEqual(deflection114a.Deflection, tree[3].Deflection);
            Assert.AreEqual(deflection114b.Deflection, tree[4].Deflection);
            Assert.AreEqual(deflection190a.Deflection, tree[5].Deflection);
            Assert.AreEqual(deflection190b.Deflection, tree[6].Deflection);
            Assert.AreEqual(deflection304.Deflection, tree[7].Deflection);
            Assert.AreEqual(deflection342.Deflection, tree[8].Deflection);
        }

        /// <summary>
        ///A test for System.Collections.Generic.ICollection<TItem>.Remove
        ///Try to remove a nonexistent node from the tree.
        ///</summary>
        [TestMethod()]
        [DeploymentItem("BinaryTree.dll")]
        public void RemoveNonexistentNode()
        {
            Tree<TestResult> tree = CreateATreeOfTestResults();
            ICollection<TestResult> target = (ICollection<TestResult>)tree;
            Assert.IsFalse(target.Remove(deflection999));
            Assert.AreEqual(10, target.Count);
            Assert.AreEqual(deflection190a.Deflection, tree.NodeData.Deflection);
            Assert.AreEqual(deflection0.Deflection, tree[0].Deflection);
            Assert.AreEqual(deflection38.Deflection, tree[1].Deflection);
            Assert.AreEqual(deflection76.Deflection, tree[2].Deflection);
            Assert.AreEqual(deflection114a.Deflection, tree[3].Deflection);
            Assert.AreEqual(deflection114b.Deflection, tree[4].Deflection);
            Assert.AreEqual(deflection190a.Deflection, tree[5].Deflection);
            Assert.AreEqual(deflection190b.Deflection, tree[6].Deflection);
            Assert.AreEqual(deflection266.Deflection, tree[7].Deflection);
            Assert.AreEqual(deflection304.Deflection, tree[8].Deflection);
            Assert.AreEqual(deflection342.Deflection, tree[9].Deflection);
        }

        /// <summary>
        ///A test for RemoveAt with a valid index
        ///</summary>
        [TestMethod()]
        public void RemoveAtTest1()
        {
            Tree<TestResult> target = CreateATreeOfTestResults();
            target.RemoveAt(4);
            int actual = target.Count;
            Assert.AreEqual(deflection0.Deflection, target[0].Deflection);
            Assert.AreEqual(deflection38.Deflection, target[1].Deflection);
            Assert.AreEqual(deflection76.Deflection, target[2].Deflection);
            Assert.AreEqual(deflection114a.Deflection, target[3].Deflection);
            Assert.AreEqual(deflection190a.Deflection, target[4].Deflection);
            Assert.AreEqual(deflection190b.Deflection, target[5].Deflection);
            Assert.AreEqual(deflection266.Deflection, target[6].Deflection);
            Assert.AreEqual(deflection304.Deflection, target[7].Deflection);
            Assert.AreEqual(deflection342.Deflection, target[8].Deflection);
            Assert.AreEqual(9, actual);
        }

        /// <summary>
        ///A test for RemoveAt with an invalid index
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void RemoveAtTest2()
        {
            Tree<TestResult> target = CreateATreeOfTestResults();
            target.RemoveAt(12);
        }


        /// <summary>
        ///A test for Count
        ///</summary>
        [TestMethod()]
        public void CountTest()
        {
            Tree<TestResult> target = CreateATreeOfTestResults();
            int expected = 10;
            int actual = target.Count;
            Assert.AreEqual(expected, actual);
        }


        /// <summary>
        ///A test for IsReadOnly
        ///</summary>
        [TestMethod()]
        public void IsReadOnlyTest()
        {
            Tree<TestResult> target = CreateATreeOfTestResults();
            Assert.IsFalse(target.IsReadOnly);
        }


        /// <summary>
        ///A test for Item (indexer) with valid indicies
        ///</summary>
        [TestMethod()]
        public void ItemTest1()
        {
            Tree<TestResult> target = CreateATreeOfTestResults();
            Assert.AreEqual(deflection0.Deflection, target[0].Deflection);
            Assert.AreEqual(deflection38.Deflection, target[1].Deflection);
            Assert.AreEqual(deflection76.Deflection, target[2].Deflection);
            Assert.AreEqual(deflection114a.Deflection, target[3].Deflection);
            Assert.AreEqual(deflection114b.Deflection, target[4].Deflection);
            Assert.AreEqual(deflection190a.Deflection, target[5].Deflection);
            Assert.AreEqual(deflection190b.Deflection, target[6].Deflection);
            Assert.AreEqual(deflection266.Deflection, target[7].Deflection);
            Assert.AreEqual(deflection304.Deflection, target[8].Deflection);
            Assert.AreEqual(deflection342.Deflection, target[9].Deflection);
        }

        /// <summary>
        ///A test for Item (indexer) with invalid indicies
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ItemTest2()
        {
            Tree<TestResult> target = CreateATreeOfTestResults();
            TestResult actual = target[-1];
            actual = target[11];
        }
    }
}
