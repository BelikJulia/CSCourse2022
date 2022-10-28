using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BinaryTree;

namespace TestHarness
{
    class Program
    {
        static void Main(string[] args)
        {
            // Modify the test harness to use the BuildTree method.
            IBinaryTree<int> tree = Tree<int>.BuildTree<int>(1, new int[] { 4, 7, 3, 4, 5 });
            Console.WriteLine("Current Tree: ");
            tree.WalkTree();
            Console.WriteLine("Add 15");
            tree.Add(15);
            Console.WriteLine("Current Tree: ");
            tree.WalkTree();
            Console.WriteLine("Remove 5");
            tree.Remove(5);
            Console.WriteLine("Current Tree: ");
            tree.WalkTree();
            Console.ReadLine();
        }
    }
}
