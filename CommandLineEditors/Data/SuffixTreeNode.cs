using System.Collections.Generic;

namespace CommandLineEditors.Data
{
    internal sealed class SuffixTreeNode<E>
    {

        /// <summary>
        /// Gets and sets the marker property.
        /// </summary>
        public bool ElementEndMarker
        {
            get => endMarker;
            set => endMarker = value;
        }

        /// <summary>
        /// Inits a new Tree node, which has no child nodes.
        /// </summary>
        public SuffixTreeNode()
        {
            childNodes = new Dictionary<E, SuffixTreeNode<E>>();
            endMarker = false;
        }

        /// <summary>
        /// Returns true if the parameter label is present in
        /// the list of child node labels.
        /// </summary>
        /// <param name="label">The label being searched for.</param>
        /// <returns>Returns true if the label is a stored child node label.</returns>
        public bool ContainsNodeLabel(E label)
        {
            return childNodes.ContainsKey(label);
        }

        /// <summary>
        /// Returns a enumerator of all child nodes stored in this
        /// tree node.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SuffixTreeNode<E>> ChildNodes()
        {
            return childNodes.Values;
        }

        /// <summary>
        /// Determines whether this tree node has no child nodes.
        /// </summary>
        /// <returns>Returns true if there are no child nodes connected
        /// to this tree node, false, if it contains at least one
        /// child node.</returns>
        public bool HasNoChildNodes()
        {
            return childNodes.Count == 0;
        }

        /// <summary>
        /// Gets the number of child nodes connected to this tree node.
        /// </summary>
        public int NumberOfChildNodes => childNodes.Count;

        /// <summary>
        /// Returns a enumeration of all labels used in this tree node.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<E> Labels()
        {
            return childNodes.Keys;
        }

        /// <summary>
        /// Creates a new tree node and adds it to this tree node.
        /// If the label is already present in this tree node, then
        /// the existing reference is returned, and no new instance
        /// is created.
        /// </summary>
        /// <param name="label">The label for the node.</param>
        /// <returns>Returns a TreeNode that is linked to this instance of
        /// a tree node as his parent node.</returns>
        public SuffixTreeNode<E> CreateTreeNode(E label)
        {
            if (!childNodes.TryGetValue(label, out SuffixTreeNode<E> node))
            {
                node = new SuffixTreeNode<E>();
                childNodes.Add(label, node);
            }
            return node;
        }

        /// <summary>
        /// Gets a tree node that is stored for the given label.
        /// </summary>
        /// <param name="label"></param>
        /// <returns>Returns the child node that is registered at this
        /// tree node for the label, or null if nothing is found.</returns>
        public SuffixTreeNode<E> GetTreeNode(E label)
        {
            childNodes.TryGetValue(label, out SuffixTreeNode<E> node);
            return node;
        }

        /// <summary>
        /// Removes the tree node for the label from this tree node.
        /// </summary>
        /// <param name="label">The label that connects this tree node to the child node
        /// that is about to be removed.</param>
        /// <returns>Returns true if the node was removed, i.g. if there was a child
        /// node connected with this tree node for the given label.</returns>
        public bool RemoveChildNode(E label)
        {
            return childNodes.Remove(label);
        }


        /// <summary>
        /// Stores the list of child nodes of this tree node.
        /// </summary>
        private readonly Dictionary<E, SuffixTreeNode<E>> childNodes;

        /// <summary>
        /// Stores an end marker which flags that an element
        /// ended at this node. Sometimes there are two elements
        /// that share the same prefix, which makes this marker
        /// necessary to find out how many real elements are
        /// hidden in a cingle branch of the tree.
        /// </summary>
        private bool endMarker;


    }
}
