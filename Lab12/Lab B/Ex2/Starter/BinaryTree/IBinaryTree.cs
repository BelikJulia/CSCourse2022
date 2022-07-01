using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BinaryTree
{
    /// <summary>
    /// Interface that defines the basic functionality of a generic binary tree.
    /// </summary>
    /// <typeparam name="TItem">Type of item to store in the tree, must implement IComparable.</typeparam>
    public interface IBinaryTree<TItem> where TItem : IComparable<TItem>
    {
        /// <summary>
        /// Adds a new item to the tree.
        /// </summary>
        /// <param name="newItem">Item to add.</param>
        void Add(TItem newItem);

        /// <summary>
        /// Remove an item from the tree.
        /// <para>
        /// This method will search for an item with the same value in the tree, to remove.
        /// </para>
        /// <para>
        /// Nothing will happen if there is not a matching item in the tree.
        /// </para>
        /// </summary>
        /// <param name="itemToRemove">Item to remove.</param>
        void Remove(TItem itemToRemove);

        /// <summary>
        /// Displays the contents of the tree in order.
        /// </summary>
        void WalkTree();
    }
    public class Tree<TItem> : IBinaryTree<TItem> where TItem : IComparable<TItem>
    {
        public TItem NodeData { get; set; }
        public Tree<TItem> LeftTree { get; set; }
        public Tree<TItem> RightTree { get; set; }
        public Tree(TItem nodeValue)
        {
            this.NodeData = nodeValue;
            this.LeftTree = null;
            this.RightTree = null;
        }
        public void Add(TItem newItem)
        {

            TItem currentNodeValue = this.NodeData;
            if (currentNodeValue.CompareTo(newItem) > 0)
            {
                if (this.LeftTree == null)
                {
                    this.LeftTree = new Tree<TItem>(newItem);
                }
                else
                {
                    this.LeftTree.Add(newItem);
                }
            }
            else
            {
                if (this.RightTree == null)
                {
                    this.RightTree = new Tree<TItem>(newItem);
                }
                else
                {
                    this.RightTree.Add(newItem);
                }
            }
        }
        public void WalkTree()
        {
            if (this.LeftTree != null)
            {
                this.LeftTree.WalkTree();
            }
            Console.WriteLine(this.NodeData.ToString());
            if (this.RightTree != null)
            {
                this.RightTree.WalkTree();
            }
        }
        public void Remove(TItem itemToRemove)
        {
            if (itemToRemove == null)
            {
                return;
            }
            if (this.NodeData.CompareTo(itemToRemove) > 0
            && this.LeftTree != null)
            {
                if (this.LeftTree.NodeData.CompareTo(itemToRemove) == 0)
                {
                    if (this.LeftTree.LeftTree == null
                    && this.LeftTree.RightTree == null)
                    {
                        this.LeftTree = null;
                    }
                    else
                    {
                        RemoveNodeWithChildren(this.LeftTree);
                    }
                }
                else
                {
                    this.LeftTree.Remove(itemToRemove);
                }
            } 
            if (this.NodeData.CompareTo(itemToRemove) < 0
            && this.RightTree != null)
            {
                if (this.RightTree.NodeData.CompareTo(itemToRemove) == 0)
                {
                    if (this.RightTree.LeftTree == null
                    && this.RightTree.RightTree == null)
                    {
                        this.RightTree = null;
                    }
                    else
                    {
                        RemoveNodeWithChildren(this.RightTree);
                    }
                }
                else
                { 
                    this.RightTree.Remove(itemToRemove);
                }
            }
            if (this.NodeData.CompareTo(itemToRemove) == 0)
            {
                if (this.LeftTree == null && this.RightTree == null)
                {
                    return;
                }
                else
                {
                    RemoveNodeWithChildren(this);
                }
            }
        }
        private void RemoveNodeWithChildren(Tree<TItem> node)
        {
            // Check whether the node has children. 
            if (node.LeftTree == null && node.RightTree == null)
            {
                throw new ArgumentException("Node has no children");
            }
            // The tree node has only one child - replace the 
            // tree node with its child node. 
            if (node.LeftTree == null ^ node.RightTree == null)
            {
                if (node.LeftTree == null)
                {
                    node.CopyNodeToThis(node.RightTree);
                }
                else
                {
                    node.CopyNodeToThis(node.LeftTree);
                }
            }
            else
            // The tree node has two children - replace the tree node's value 
            // with its "in order successor" node value and then remove the 
            // in order successor node.
            {
                // Find the in order successor – the leftmost descendant of 
                // its RightTree node.
                Tree<TItem> successor = GetLeftMostDescendant(node.RightTree);
                // Copy the node value from the in order successor. 
                node.NodeData = successor.NodeData;
                // Remove the in order successor node. 
                if (node.RightTree.RightTree == null &&
                node.RightTree.LeftTree == null)
                {
                    node.RightTree = null; // The successor node had no 
                    // children.
                }
                else
                {
                    node.RightTree.Remove(successor.NodeData);
                }
            }
        }
        private void CopyNodeToThis(Tree<TItem> node)
        {
            this.NodeData = node.NodeData;
            this.LeftTree = node.LeftTree;
            this.RightTree = node.RightTree;
        }
        private Tree<TItem> GetLeftMostDescendant(Tree<TItem> node)
        {
            while (node.LeftTree != null)
            {
                node = node.LeftTree;
            }
            return node;
        }
    }
}
