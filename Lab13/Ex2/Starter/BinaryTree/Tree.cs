using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BinaryTree
{
    /// <summary>
    /// This generic class is an implementation of IBinaryTree and IList.
    /// <para>
    /// This class implements a recursive definition of a tree, where each node in the tree
    /// has left and right children, which are also trees.
    /// </para>
    /// </summary>
    /// <typeparam name="TItem">Type of item to store in the tree, must implement IComparable.</typeparam>    
    public class Tree<TItem> : IList<TItem>, IBinaryTree<TItem> where TItem : IComparable<TItem>
    {
        /// <summary>
        /// Data of type TItem to store in the tree.
        /// </summary>
        public TItem NodeData { get; set; }

        /// <summary>
        /// The left branch of the Tree.
        /// </summary>
        public Tree<TItem> LeftTree { get; set; }

        /// <summary>
        /// The right branch of the tree.
        /// </summary>
        public Tree<TItem> RightTree { get; set; }

        // Add a private integer variable position to define
        // the node's position in the tree.
        private int position;

        /// <summary>
        /// Constructor to initialize a new Tree node.
        /// </summary>
        /// <param name="nodeValue">Value of type TItem to add to the Tree.</param>
        public Tree(TItem nodeValue)
        {
            this.NodeData = nodeValue;
            this.LeftTree = null;
            this.RightTree = null;
            this.position = -1;
        }

        #region IBinaryTree<TItem> members

        /// <summary>
        /// Adds a new item to the tree, duplicates are allowed.
        /// <para>
        /// Performs a recursive descent searching for the node where the new item should be added.
        /// </para>
        /// </summary>
        /// <param name="newItem">Item of type TItem to add.</param>
        public void Add(TItem newItem)
        {
            // If we're adding something, the position field will become 
            // invalid. Reset position to -1.
            this.position = -1;

            // Get the value of the current node.
            TItem currentNodeValue = this.NodeData;

            // Do we need to insert on the left tree?
            if (currentNodeValue.CompareTo(newItem) > 0)
            {
                // Is the left tree null?
                if (this.LeftTree == null)
                {
                    this.LeftTree = new Tree<TItem>(newItem);
                }
                else // Call Add recursively
                {
                    this.LeftTree.Add(newItem);
                }
            }
            else // Insert on the right tree
            {
                // Is the right tree null?
                if (this.RightTree == null)
                {
                    this.RightTree = new Tree<TItem>(newItem);
                }
                else // Call add recursively
                {
                    this.RightTree.Add(newItem);
                }
            }
        }

        /// <summary>
        /// Removes an item from the tree.
        /// <para>
        /// Note that you can't remove the last node from the tree.
        /// </para>
        /// <para>
        /// If the item is not found in the tree, nothing happens.
        /// </para>
        /// <para>
        /// The remove algorithm needs to treat removing the root node differently from other
        /// nodes - if the root node has no children it can't be removed.
        /// </para>
        /// <para>
        /// In general remove has to deal with 3 scenarios - nodes with no children,
        /// nodes with a single child, and nodes with two children.
        /// </para>
        /// </summary>
        /// <param name="itemToRemove"></param>
        public void Remove(TItem itemToRemove)
        {

            // Can't remove null
            if (itemToRemove == null) return;

            // If we're deleting something, the position field will become 
            // invalid. Reset position to -1
            this.position = -1;

            // Do we need to scan the LeftTree for the item?
            if (this.NodeData.CompareTo(itemToRemove) > 0 && this.LeftTree != null)
            {
                // Check the LeftTree
                // Note that we are looking down 2 levels - we can't remove
                // 'this' only LeftTree or RightTree.
                if (this.LeftTree.NodeData.CompareTo(itemToRemove) == 0)
                {
                    // LeftTree has no children - set LeftTree to null
                    if (this.LeftTree.LeftTree == null && this.LeftTree.RightTree == null)
                        this.LeftTree = null;
                    else // Remove LeftTree
                        RemoveNodeWithChildren(this.LeftTree);
                }
                else
                {
                    // Keep looking - call Remove recursively
                    this.LeftTree.Remove(itemToRemove);
                }
            }

            // Do we need to scan the RightTree for the item?
            if (this.NodeData.CompareTo(itemToRemove) < 0 && this.RightTree != null)
            {
                // Check the RightTree
                // Note that we are looking down 2 levels - we can't remove
                // 'this' only LeftTree or RightTree.
                if (this.RightTree.NodeData.CompareTo(itemToRemove) == 0)
                {
                    // RightTree has no children - set RightTree to null
                    if (this.RightTree.LeftTree == null && this.RightTree.RightTree == null)
                        this.RightTree = null;
                    else // Remove the RightTree
                        RemoveNodeWithChildren(this.RightTree);
                }
                else
                {
                    // Keep looking - call Remove recursively
                    this.RightTree.Remove(itemToRemove);
                }
            }

            // This will only apply at the root node
            if (this.NodeData.CompareTo(itemToRemove) == 0)
            {
                // No children - do nothing, a tree must have at least one node.               
                if (this.LeftTree == null && this.RightTree == null)
                    return;
                else // Root node has children
                    RemoveNodeWithChildren(this);
            }
        }

        /// <summary>
        /// Walks the tree in Node value order, writing results to the Console.
        /// </summary>
        public void WalkTree()
        {
            // Recursive descent of Left tree
            if (this.LeftTree != null)
            {
                this.LeftTree.WalkTree();
            }

            Console.WriteLine(this.NodeData.ToString());

            // Recursive descent of Right tree
            if (this.RightTree != null)
            {
                this.RightTree.WalkTree();
            }
        }

        #endregion

        #region Utility methods

        /// <summary>
        /// Utility method to find the left most Descendent of a Tree node.
        /// </summary>
        /// <param name="node">Tree node to start from.</param>
        /// <returns>Left most desscendent.</returns>
        private Tree<TItem> GetLeftMostDescendent(Tree<TItem> node)
        {
            while (node.LeftTree != null)
            {
                node = node.LeftTree;
            }
            return node;
        }

        /// <summary>
        /// Utility method to copy values from another Tree node to this node.
        /// </summary>
        /// <param name="node">Tree node to copy from.</param>
        private void copyNodeToThis(Tree<TItem> node)
        {
            this.NodeData = node.NodeData;
            this.LeftTree = node.LeftTree;
            this.RightTree = node.RightTree;
        }

        /// <summary>
        /// Utility method used by the Remove method.
        /// <para>
        /// It removes a node that has either one or two children.
        /// </para>
        /// </summary>
        /// <param name="node"></param>
        private void RemoveNodeWithChildren(Tree<TItem> node)
        {
            // Check node has children
            if (node.LeftTree == null && node.RightTree == null)
                throw new ArgumentException("Node has no children");

            // Tree node has only one child - replace Tree node with its child node
            if (node.LeftTree == null ^ node.RightTree == null)
            {
                if (node.LeftTree == null)
                    node.copyNodeToThis(node.RightTree);
                else
                    node.copyNodeToThis(node.LeftTree);
            }
            else
            // Node has two children - replace Tree node's value with its "in order successor" node value
            // and then remove the in order successor node.
            {
                // Find the in order successor - left most descendent of its RightTree node.
                Tree<TItem> successor = GetLeftMostDescendent(node.RightTree);

                // Copy the node value from the in order successor
                node.NodeData = successor.NodeData;

                // Remove the in order successor node
                if (node.RightTree.RightTree == null && node.RightTree.LeftTree == null)
                    node.RightTree = null; // Successor node had no children.
                else
                    node.RightTree.Remove(successor.NodeData); // Recursive call.
            }
        }

        // Create a "int indexTree(int index)" method that sets the position field for each tree node.
        private int indexTree(int index)
        {
            if (this.LeftTree != null)
            {
                index = this.LeftTree.indexTree(index);
            }

            this.position = index;

            index++;

            if (this.RightTree != null)
            {
                index = this.RightTree.indexTree(index);
            }

            return index;
        }

        // Create a "Tree<TItem> getItemAtIndex(int index)" method that finds a Tree node at a specified position.
        private Tree<TItem> GetItemAtIndex(int index)
        {
            // Add the index values if they're not already there
            if (this.position == -1)
            {
                this.indexTree(0);
            }
            
            if (this.position > index)
            {
                return this.LeftTree.GetItemAtIndex(index);
            }
            if (this.position < index)
            {
                return this.RightTree.GetItemAtIndex(index);
            }
            return this;
        }

        // Implement a "int getCount(int accumulator)" to count the number of nodes in the tree
        private int GetCount(int accumulator)
        {
            if (this.LeftTree != null)
            {
                accumulator = LeftTree.GetCount(accumulator);
            }
            accumulator++;
            if (this.RightTree != null)
            {
                accumulator = RightTree.GetCount(accumulator);
            }
            return accumulator;
        }

        #endregion


        public int IndexOf(TItem item)
        {
            if (item == null) return -1;

            // Add the index values if they're not already there
            if (this.position == -1)
                this.indexTree(0);

            // Find the item - searching the tree for a matching Node.
            if (item.CompareTo(this.NodeData) < 0)
            {
                if (this.LeftTree == null)
                {
                    return -1;
                }
                return this.LeftTree.IndexOf(item);
            }
            if (item.CompareTo(this.NodeData) > 0)
            {
                if (this.RightTree == null)
                {
                    return -1;
                }
                return this.RightTree.IndexOf(item);
            }
            return this.position;

        }

        public void Insert(int index, TItem item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public TItem this[int index]
        {
            get
            {
                if (index < 0 | index >= Count)
                {
                    throw new ArgumentOutOfRangeException
                        ("index", index, "Indexer out of range");
                }

                return GetItemAtIndex(index).NodeData;
            }
            set
            {
                throw new NotImplementedException();
            }
        }


        public void Clear()
        {
            LeftTree = null;
            RightTree = null;

            NodeData = default(TItem);

        }

        public bool Contains(TItem item)
        {
            if (NodeData.CompareTo(item) == 0) return true;

            if (NodeData.CompareTo(item) > 0)
            {
                if (this.LeftTree != null)
                    return this.LeftTree.Contains(item);
            }
            else
            {
                if (this.RightTree != null)
                    return this.RightTree.Contains(item);
            }

            return false;
        }

        public void CopyTo(TItem[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get
            {
                return GetCount(0);
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        bool ICollection<TItem>.Remove(TItem item)
        {
            if (this.Contains(item) == true)
            {
                this.Remove(item);
                return true;
            }
            return false;
        }

        public IEnumerator<TItem> GetEnumerator()
        {
            return new TreeEnumerator<TItem>(this);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
