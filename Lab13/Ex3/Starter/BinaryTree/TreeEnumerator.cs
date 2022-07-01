using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BinaryTree
{
    class TreeEnumerator<TItem> : IEnumerator<TItem> where TItem : IComparable<TItem>
    {
        private Tree<TItem> currentData = null;

        private TItem currentItem = default(TItem);

        private Queue<TItem> enumData = null;

        public TreeEnumerator(Tree<TItem> data)
        {
            this.currentData = data;
        }

        private void Populate(Queue<TItem> enumQueue, Tree<TItem> tree)
        {
            if (tree.LeftTree != null)
            {
                Populate(enumQueue, tree.LeftTree);
            }

            enumQueue.Enqueue(tree.NodeData);

            if (tree.RightTree != null)
            {
                Populate(enumQueue, tree.RightTree);
            }
        }

        public TItem Current
        {
            get
            {
                if (this.enumData == null)
                {
                    throw new InvalidOperationException("Use MoveNext before calling Current");
                }
                return this.currentItem;
            }
        }

        public void Dispose()
        {
            enumData.Clear();
        }

        object System.Collections.IEnumerator.Current
        {
            get { throw new NotImplementedException(); }
        }

        public bool MoveNext()
        {
            if (this.enumData == null)
            {
                this.enumData = new Queue<TItem>();
                Populate(this.enumData, this.currentData);
            }

            if (this.enumData.Count > 0)
            {
                this.currentItem = this.enumData.Dequeue();
                return true;
            }
            return false;
        }

        public void Reset()
        {
            Populate(this.enumData, this.currentData);
        }
    }
}
