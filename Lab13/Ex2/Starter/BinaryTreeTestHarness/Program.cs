using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BinaryTree;
using StressTestResult;

namespace BinaryTreeTestHarness
{
    class Program
    {
        static void Main(string[] args)
        {
            // Integer Tests
            TestIntegerTree();
            TestDeleteRootNodeInteger();
            //TestDeleteNodeNoChildrenInteger();
            //TestDeleteNodeOneChildInteger();
            //TestDeleteNodeTwoChildrenInteger();
            TestIteratorsIntegers();

            // String Tests
            TestStringTree();
            TestDeleteRootNodeString();
            //TestDeleteNodeNoChildrenString();
            //TestDeleteNodeOneChildString();
            //TestDeleteNodeTwoChildrenString();
            TestIteratorsStrings();

            // TestResult Tests
            TestTestResultTree();
            TestDeleteRootNodeTestResults();
            //TestDeleteNodeNoChildrenTestResult();
            //TestDeleteNodeOneChildTestResult();
            //TestDeleteNodeTwoChildrenTestResult();
            TestIteratorsTestResults();

        }

        #region Integer Tests
        private static void TestIntegerTree()
        {
            Console.WriteLine("TestIntegerTree()");
            Tree<int> tree = CreateATreeOfIntegers();
            Console.WriteLine("\nWalkTree()");
            tree.WalkTree();
            Console.WriteLine("\nCount: {0}", tree.Count);
            Console.WriteLine("\nRemove(11)");
            ICollection<int> colTree = tree;
            colTree.Remove(11);
            Console.WriteLine("\nCount: {0}", tree.Count);
            Console.WriteLine("\nContains(11): {0}", tree.Contains(11));
            Console.WriteLine("\nContains(-12): {0}", tree.Contains(-12));
            Console.WriteLine("\nIndexOf(5): {0}", tree.IndexOf(5));
            Console.WriteLine("\ntree[3]: {0}", tree[3]);
            Console.ReadLine();
            Console.Clear();
        }

        private static void TestIteratorsIntegers()
        {
            Console.WriteLine("TestIteratorsIntegers()");
            Tree<int> tree = CreateATreeOfIntegers();
            Console.WriteLine("\nIn ascending order");
            foreach (var item in tree)
            {
                Console.WriteLine(item.ToString());
            }

            Console.WriteLine("\nIn descending order");
            try
            {
                foreach (var item in tree.Reverse())
                {
                    Console.WriteLine(item.ToString());
                }
            }
            catch (NotImplementedException)
            {
                Console.WriteLine("Not Implemented. You will implement this functionality in Exercise 3");
            }
            Console.ReadLine();
            Console.Clear();
        }
        private static void TestDeleteRootNodeInteger()
        {
            Console.WriteLine("TestDeleteRootNodeInteger()");
            Tree<int> tree = CreateATreeOfIntegers();
            Console.WriteLine("\nBefore");
            tree.WalkTree();
            Console.WriteLine("\nRemove 10 twice");
            ICollection<int> colTree = tree;
            colTree.Remove(10);
            colTree.Remove(10);
            Console.WriteLine("\nAfter");
            tree.WalkTree();
            Console.ReadLine();
            Console.Clear();
        }
        private static void TestDeleteNodeNoChildrenInteger()
        {
            Console.WriteLine("TestDeleteNodeNoChildrenInteger()");
            Tree<int> tree = CreateATreeOfIntegers();
            Console.WriteLine("\nBefore");
            tree.WalkTree();
            Console.WriteLine("\nRemove 14");
            ICollection<int> colTree = tree;
            colTree.Remove(14);
            Console.WriteLine("\nAfter");
            tree.WalkTree();
            Console.ReadLine();
            Console.Clear();
        }

        private static void TestDeleteNodeOneChildInteger()
        {
            Console.WriteLine("TestDeleteNodeOneChildInteger()");
            Tree<int> tree = CreateATreeOfIntegers();
            Console.WriteLine("\nBefore");
            tree.WalkTree();
            Console.WriteLine("\nRemove -12");
            ICollection<int> colTree = tree;
            colTree.Remove(-12);
            Console.WriteLine("\nAfter");
            tree.WalkTree();
            Console.ReadLine();
            Console.Clear();
        }

        private static void TestDeleteNodeTwoChildrenInteger()
        {
            Console.WriteLine("TestDeleteNodeTwoChildrenInteger()");
            Tree<int> tree = CreateATreeOfIntegers();
            Console.WriteLine("\nBefore");
            tree.WalkTree();
            Console.WriteLine("\nRemove 11");
            ICollection<int> colTree = tree;
            colTree.Remove(11);
            Console.WriteLine("\nAfter");
            tree.WalkTree();
            Console.ReadLine();
            Console.Clear();
        }
        #endregion

        #region String Tests
        private static void TestStringTree()
        {
            Console.WriteLine("TestStringTree()");
            Tree<string> tree = CreateATreeOfStrings();
            Console.WriteLine("\nWalkTree()");
            tree.WalkTree();
            Console.WriteLine("\nCount: {0}", tree.Count);
            Console.WriteLine("\nRemove(\"p936\")");
            ICollection<string> colTree = tree;
            colTree.Remove("p936");
            Console.WriteLine("\nCount: {0}", tree.Count);
            Console.WriteLine("\nContains(\"p936\"): {0}", tree.Contains("p936"));
            Console.WriteLine("\nContains(\"a279\"): {0}", tree.Contains("a279"));
            Console.WriteLine("\nIndexOf(\"h624\"): {0}", tree.IndexOf("h624"));
            Console.WriteLine("\ntree[3]: {0}", tree[3]);
            Console.ReadLine();
            Console.Clear();
        }

        private static void TestIteratorsStrings()
        {
            Console.WriteLine("TestIteratorsStrings()");
            Tree<string> tree = CreateATreeOfStrings();
            Console.WriteLine("\nIn ascending order");
            foreach (var item in tree)
            {
                Console.WriteLine(item.ToString());
            }

            Console.WriteLine("\nIn descending order");
            try
            {
                foreach (var item in tree.Reverse())
                {
                    Console.WriteLine(item.ToString());
                }
            }
            catch (NotImplementedException)
            {
                Console.WriteLine("Not Implemented. You will implement this functionality in Exercise 3");
            }
            Console.ReadLine();
            Console.Clear();
        }
        private static void TestDeleteRootNodeString()
        {
            Console.WriteLine("TestDeleteRootNodeString()");
            Tree<string> tree = CreateATreeOfStrings();
            Console.WriteLine("\nBefore");
            tree.WalkTree();
            Console.WriteLine("\nRemove k203 twice");
            ICollection<string> colTree = tree;
            colTree.Remove("k203");
            colTree.Remove("k203");
            Console.WriteLine("\nAfter");
            tree.WalkTree();
            Console.ReadLine();
            Console.Clear();
        }
        private static void TestDeleteNodeNoChildrenString()
        {
            Console.WriteLine("TestDeleteNodeNoChildrenString()");
            Tree<string> tree = CreateATreeOfStrings();
            Console.WriteLine("\nBefore");
            tree.WalkTree();
            Console.WriteLine("\nRemove r483");
            ICollection<string> colTree = tree;
            colTree.Remove("r483");
            Console.WriteLine("\nAfter");
            tree.WalkTree();
            Console.ReadLine();
            Console.Clear();
        }

        private static void TestDeleteNodeOneChildString()
        {
            Console.WriteLine("TestDeleteNodeOneChildString()");
            Tree<string> tree = CreateATreeOfStrings();
            Console.WriteLine("\nBefore");
            tree.WalkTree();
            Console.WriteLine("\nRemove a279");
            ICollection<string> colTree = tree;
            colTree.Remove("a279");
            Console.WriteLine("\nAfter");
            tree.WalkTree();
            Console.ReadLine();
            Console.Clear();
        }

        private static void TestDeleteNodeTwoChildrenString()
        {
            Console.WriteLine("TestDeleteNodeTwoChildrenString()");
            Tree<string> tree = CreateATreeOfStrings();
            Console.WriteLine("\nBefore");
            tree.WalkTree();
            Console.WriteLine("\nRemove p936");
            ICollection<string> colTree = tree;
            colTree.Remove("p936");
            Console.WriteLine("\nAfter");
            tree.WalkTree();
            Console.ReadLine();
            Console.Clear();
        }
        #endregion

        #region TestResult Tests
        private static void TestTestResultTree()
        {
            TestResult def266 = new TestResult() { Deflection = 266, AppliedStress = 80, Temperature = 200, TestDate = DateTime.Now };
            TestResult def0 = new TestResult() { Deflection = 0, AppliedStress = 10, Temperature = 200, TestDate = DateTime.Now };
            TestResult def114 = new TestResult() { Deflection = 114, AppliedStress = 50, Temperature = 200, TestDate = DateTime.Now };
            Console.WriteLine("TestTestResultTree()");
            Tree<TestResult> tree = CreateATreeOfTestResults();
            Console.WriteLine("\nWalkTree()");
            tree.WalkTree();
            Console.WriteLine("\nCount: {0}", tree.Count);
            Console.WriteLine("\nRemove(def266)");
            ICollection<TestResult> colTree = tree;
            colTree.Remove(def266);
            Console.WriteLine("\nCount: {0}", tree.Count);
            Console.WriteLine("\nContains(def266): {0}", tree.Contains(def266));
            Console.WriteLine("\nContains(def0): {0}", tree.Contains(def0));
            Console.WriteLine("\nIndexOf(def114): {0}", tree.IndexOf(def114));
            Console.WriteLine("\ntree[3]: {0}", tree[3]);
            Console.ReadLine();
            Console.Clear();
        }

        private static void TestIteratorsTestResults()
        {
            Console.WriteLine("TestIteratorsTestResults()");
            Tree<TestResult> tree = CreateATreeOfTestResults();
            Console.WriteLine("\nIn ascending order");
            foreach (var item in tree)
            {
                Console.WriteLine(item.ToString());
            }

            Console.WriteLine("\nIn descending order");
            try
            {
                foreach (var item in tree.Reverse())
                {
                    Console.WriteLine(item.ToString());
                }
            }
            catch (NotImplementedException)
            {
                Console.WriteLine("Not Implemented. You will implement this functionality in Exercise 3");
            }
            Console.ReadLine();
            Console.Clear();
        }
        private static void TestDeleteRootNodeTestResults()
        {
            TestResult def190 = new TestResult() { Deflection = 190, AppliedStress = 70, Temperature = 200, TestDate = DateTime.Now };
            Console.WriteLine("TestDeleteRootNodeTestResults()");
            Tree<TestResult> tree = CreateATreeOfTestResults();
            Console.WriteLine("\nBefore");
            tree.WalkTree();
            Console.WriteLine("\nRemove def190 twice");
            ICollection<TestResult> colTree = tree;
            colTree.Remove(def190);
            colTree.Remove(def190);
            Console.WriteLine("\nAfter");
            tree.WalkTree();
            Console.ReadLine();
            Console.Clear();
        }
        private static void TestDeleteNodeNoChildrenTestResult()
        {
            TestResult def304 = new TestResult() { Deflection = 304, AppliedStress = 90, Temperature = 200, TestDate = DateTime.Now };
            Console.WriteLine("TestDeleteNodeNoChildrenTestResult()");
            Tree<TestResult> tree = CreateATreeOfTestResults();
            Console.WriteLine("\nBefore");
            tree.WalkTree();
            Console.WriteLine("\nRemove def304");
            ICollection<TestResult> colTree = tree;
            colTree.Remove(def304);
            Console.WriteLine("\nAfter");
            tree.WalkTree();
            Console.ReadLine();
            Console.Clear();
        }

        private static void TestDeleteNodeOneChildTestResult()
        {
            TestResult def0 = new TestResult() { Deflection = 0, AppliedStress = 10, Temperature = 200, TestDate = DateTime.Now };
            Console.WriteLine("TestDeleteNodeOneChildTestResult()");
            Tree<TestResult> tree = CreateATreeOfTestResults();
            Console.WriteLine("\nBefore");
            tree.WalkTree();
            Console.WriteLine("\nRemove def0");
            ICollection<TestResult> colTree = tree;
            colTree.Remove(def0);
            Console.WriteLine("\nAfter");
            tree.WalkTree();
            Console.ReadLine();
            Console.Clear();
        }

        private static void TestDeleteNodeTwoChildrenTestResult()
        {
            TestResult def266 = new TestResult() { Deflection = 266, AppliedStress = 80, Temperature = 200, TestDate = DateTime.Now };
            Console.WriteLine("TestDeleteNodeTwoChildrenTestResult()");
            Tree<TestResult> tree = CreateATreeOfTestResults();
            Console.WriteLine("\nBefore");
            tree.WalkTree();
            Console.WriteLine("\nRemove def266");
            ICollection<TestResult> colTree = tree;
            colTree.Remove(def266);
            Console.WriteLine("\nAfter");
            tree.WalkTree();
            Console.ReadLine();
            Console.Clear();
        }
        #endregion

        #region Create Some Trees

        /// <summary>
        /// Creates a tree of integers.
        ///             10
        ///         /        \
        ///      5              11
        ///    /   \           /   \
        ///  -12     5       10     15
        ///     \                  /
        ///      0               14
        ///     /
        ///    -8
        /// </summary>
        /// <returns>Tree of integers.</returns>
        private static Tree<int> CreateATreeOfIntegers()
        {
            Tree<int> tree = new Tree<int>(10);
            ICollection<int> colTree = tree;
            colTree.Add(5);
            colTree.Add(11);
            colTree.Add(5);
            colTree.Add(-12);
            colTree.Add(15);
            colTree.Add(0);
            colTree.Add(14);
            colTree.Add(-8);
            colTree.Add(10);
            return tree;
        }

        /// <summary>
        /// Creates a tree of strings.
        ///            k203
        ///         /        \
        ///      h624          p936
        ///    /   \           /   \
        /// a279   h624      k203  z837
        ///     \                  /
        ///    e762              r483
        ///     /
        ///   d776
        /// </summary>
        /// <returns>Tree of strings.</returns>
        private static Tree<string> CreateATreeOfStrings()
        {
            Tree<string> tree = new Tree<string>("k203");
            ICollection<string> colTree = tree;
            colTree.Add("h624");
            colTree.Add("p936");
            colTree.Add("h624");
            colTree.Add("a279");
            colTree.Add("z837");
            colTree.Add("e762");
            colTree.Add("r483");
            colTree.Add("d776");
            colTree.Add("k203");
            return tree;
        }

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
            ICollection<TestResult> colTree = tree;
            colTree.Add(new TestResult() { Deflection = 114, AppliedStress = 40, Temperature = 200, TestDate = DateTime.Now });
            colTree.Add(new TestResult() { Deflection = 266, AppliedStress = 80, Temperature = 200, TestDate = DateTime.Now });
            colTree.Add(new TestResult() { Deflection = 114, AppliedStress = 50, Temperature = 200, TestDate = DateTime.Now });
            colTree.Add(new TestResult() { Deflection = 0, AppliedStress = 10, Temperature = 200, TestDate = DateTime.Now });
            colTree.Add(new TestResult() { Deflection = 342, AppliedStress = 100, Temperature = 200, TestDate = DateTime.Now });
            colTree.Add(new TestResult() { Deflection = 76, AppliedStress = 30, Temperature = 200, TestDate = DateTime.Now });
            colTree.Add(new TestResult() { Deflection = 304, AppliedStress = 90, Temperature = 200, TestDate = DateTime.Now });
            colTree.Add(new TestResult() { Deflection = 38, AppliedStress = 20, Temperature = 200, TestDate = DateTime.Now });
            colTree.Add(new TestResult() { Deflection = 190, AppliedStress = 70, Temperature = 200, TestDate = DateTime.Now });
            return tree;
        }

        #endregion
    }
}
