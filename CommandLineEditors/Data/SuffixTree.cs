using System;
using System.Collections.Generic;

namespace CommandLineEditors.Data
{
    /// <summary>
    /// The SuffixTree is a data structure which makes the lookup
    /// of the list of possible matches easy.
    /// </summary>
    public class SuffixTree<ElementBaseType>
    {

        /// <summary>
        /// Stores the root node of the suffix tree.
        /// </summary>
        private readonly TreeNode<ElementBaseType> rootNode;



        /// <summary>
        /// Inits the new suffix tree.
        /// </summary>
        public SuffixTree()
        {
            rootNode = new TreeNode<ElementBaseType>();
        }



        /// <summary>
        /// Adds an element to the suffix tree.
        /// </summary>
        /// <param name="element"></param>
        public void AddElement(ElementBaseType[] element)
        {
            AddElement(element, 0, rootNode);
        }



        /// <summary>
        /// Adds an element to the tree structure.
        /// </summary>
        /// <param name="element">The element to be added.</param>
        /// <param name="currentPos">The current position in the list of base elements.</param>
        /// <param name="currentNode">The current tree node where the current suffix of the element will be added.</param>
        private void AddElement(ElementBaseType[] element, int currentPos, TreeNode<ElementBaseType> currentNode)
        {
            if (currentPos < element.Length)
            {
                AddElement(element, currentPos + 1, currentNode.CreateTreeNode(element[currentPos]));
            }
            else if (currentPos == element.Length)
            {
                currentNode.ElementEndMarker = true;
            }
        }



        /// <summary>
        /// Removes the element from the suffix tree. If the element is
        /// not given in the tree, this method does nothing.
        /// </summary>
        /// <param name="element">The element to be deleted.</param>
        public void RemoveElement(ElementBaseType[] element)
        {
            RemoveElement(element, 0, rootNode);
        }



        /// <summary>
        /// Removes an element from the tree, and returns true on success,
        /// i.g. if the element was a member of the tree before removal.
        /// </summary>
        /// <param name="element">The element to be removed.</param>
        /// <param name="currentPos">The current position in the element under consideration.</param>
        /// <param name="currentNode">The current tree node used as current root node.</param>
        /// <returns>Returns true, of the element was successfully removed from the tree,
        /// otherwise false, that is if the element was not a menber of the tree at all.</returns>
        private bool RemoveElement(ElementBaseType[] element, int currentPos, TreeNode<ElementBaseType> currentNode)
        {
            if (currentPos < element.Length)
            {
                TreeNode<ElementBaseType> nextNode = currentNode.GetTreeNode(element[currentPos]);
                // is there is no child node for the current label,
                // the element that the user requests to remove is
                // not present in the tree. We do nothing and return
                // false as a signal that removal was not successful.
                if (nextNode == null)
                {
                    return false;
                }

                if (RemoveElement(element, currentPos + 1, nextNode))
                {
                    // Check if this is the only child node of the current node.
                    // If so, this must me the nextNode we found for the current
                    // label.
                    if (currentNode.NumberOfChildNodes == 1)
                    {
                        currentNode.RemoveChildNode(element[currentPos]);
                        // But only if the current node is not an end marker
                        // node, it may also be deleted.
                        return !currentNode.ElementEndMarker;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                // The end of the depth-phase reached, so we check if the node
                // is marked as a end node, which shows us that the element is
                // a member of the tree indeed.
                if (currentNode.ElementEndMarker)
                {
                    // this node is no longer an end node
                    currentNode.ElementEndMarker = false;
                    // Only if the current node has no child nodes, we are
                    // allowed to remove this node from its parent node.
                    return currentNode.HasNoChildNodes();
                }
                else
                {
                    return false;
                }
            }

            // By default, do not remove anything.
            return false;
        }



        /// <summary>
        /// Retrives the list of matches which start with the
        /// given prefix.
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public List<ElementBaseType[]> GetMatchingElements(ElementBaseType[] prefix)
        {
            return GetMatchingElements(prefix, 0, rootNode);
        }



        private List<ElementBaseType[]> GetMatchingElements(ElementBaseType[] prefix, int startIndex, TreeNode<ElementBaseType> currentNode)
        {
            // if the prefix has ended, we gather all matching elements from
            // the tree spanning from the currentNode.
            if (!(startIndex < prefix.Length))
            {
                return CollectMatchingElements(currentNode);
            }

            // the current label we are working on
            ElementBaseType currentLabel = prefix[startIndex];

            // gets the child node stored in the current node
            // as a child for the given label.
            TreeNode<ElementBaseType> childNode = currentNode.GetTreeNode(currentLabel);

            // if there is no registered child node, we just pass the empty list
            // as a result to the callee.
            if (childNode == null)
            {
                return new List<ElementBaseType[]>();
            }

            // ... otherwise we start to prepend the label to
            // all intermediate solutions
            List<ElementBaseType[]> matches = GetMatchingElements(prefix, startIndex + 1, childNode);

            for (int i = 0; i < matches.Count; i++)
            {
                ElementBaseType[] elem = matches[i];
                matches[i] = PrependLabel(currentLabel, matches[i]);
            }


            return matches;
        }



        /// <summary>
        /// Prepends the label in front of the match. This method creates
        /// a new array, and copies all elements to the position.
        /// </summary>
        /// <param name="label">The label which will be used as first element of the new array.</param>
        /// <param name="match">The match which will make up the rest of the new match.</param>
        /// <returns>Returns a new created array containing all elements as described.</returns>
        private ElementBaseType[] PrependLabel(ElementBaseType label, ElementBaseType[] match)
        {
            ElementBaseType[] newMatch = new ElementBaseType[match.Length + 1];
            newMatch[0] = label;
            Array.Copy(match, 0, newMatch, 1, match.Length);
            return newMatch;
        }



        private List<ElementBaseType[]> CollectMatchingElements(TreeNode<ElementBaseType> currentNode)
        {
            List<ElementBaseType[]> results = new List<ElementBaseType[]>();

            foreach (ElementBaseType label in currentNode.Labels())
            {
                TreeNode<ElementBaseType> treeNode = currentNode.GetTreeNode(label);
                if (treeNode != null)
                {
                    List<ElementBaseType[]> matches = CollectMatchingElements(treeNode);
                    foreach (ElementBaseType[] match in matches)
                    {
                        results.Add(PrependLabel(label, match));
                    }
                }
            }

            // if this is an end node, we also need to
            // create a result element of length 0.
            if (currentNode.ElementEndMarker)
            {
                results.Add(new ElementBaseType[0]);
            }

            return results;
        }



    }



    #region Tree Structure Classes

    internal class TreeNode<E>
    {

        /// <summary>
        /// Stores the list of child nodes of this tree node.
        /// </summary>
        private readonly Dictionary<E, TreeNode<E>> childNodes;

        /// <summary>
        /// Stores an end marker which flags that an element
        /// ended at this node. Sometimes there are two elements
        /// that share the same prefix, which makes this marker
        /// necessary to find out how many real elements are
        /// hidden in a cingle branch of the tree.
        /// </summary>
        private bool endMarker;


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
        public TreeNode()
        {
            childNodes = new Dictionary<E, TreeNode<E>>();
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
        public IEnumerable<TreeNode<E>> ChildNodes()
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
        public TreeNode<E> CreateTreeNode(E label)
        {
            if (!childNodes.TryGetValue(label, out TreeNode<E> node))
            {
                node = new TreeNode<E>();
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
        public TreeNode<E> GetTreeNode(E label)
        {
            childNodes.TryGetValue(label, out TreeNode<E> node);
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

    }

    #endregion


}
