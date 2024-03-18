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
        /// Inits the new suffix tree.
        /// </summary>
        public SuffixTree()
        {
            rootNode = new SuffixTreeNode<ElementBaseType>();
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
        /// Retrives the list of matches which start with the
        /// given prefix.
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public List<ElementBaseType[]> GetMatchingElements(ElementBaseType[] prefix)
        {
            return GetMatchingElements(prefix, 0, rootNode);
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
        /// Stores the root node of the suffix tree.
        /// </summary>
        private readonly SuffixTreeNode<ElementBaseType> rootNode;


        /// <summary>
        /// Adds an element to the tree structure.
        /// </summary>
        /// <param name="element">The element to be added.</param>
        /// <param name="currentPos">The current position in the list of base elements.</param>
        /// <param name="currentNode">The current tree node where the current suffix of the element will be added.</param>
        private void AddElement(ElementBaseType[] element, int currentPos, SuffixTreeNode<ElementBaseType> currentNode)
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

        private List<ElementBaseType[]> CollectMatchingElements(SuffixTreeNode<ElementBaseType> currentNode)
        {
            List<ElementBaseType[]> results = new List<ElementBaseType[]>();

            foreach (ElementBaseType label in currentNode.Labels())
            {
                SuffixTreeNode<ElementBaseType>? treeNode = currentNode.GetTreeNode(label);
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

        private List<ElementBaseType[]> GetMatchingElements(ElementBaseType[] prefix, int startIndex, SuffixTreeNode<ElementBaseType> currentNode)
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
            SuffixTreeNode<ElementBaseType>? childNode = currentNode.GetTreeNode(currentLabel);

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

        /// <summary>
        /// Removes an element from the tree, and returns true on success,
        /// i.g. if the element was a member of the tree before removal.
        /// </summary>
        /// <param name="element">The element to be removed.</param>
        /// <param name="currentPos">The current position in the element under consideration.</param>
        /// <param name="currentNode">The current tree node used as current root node.</param>
        /// <returns>Returns true, of the element was successfully removed from the tree,
        /// otherwise false, that is if the element was not a menber of the tree at all.</returns>
        private bool RemoveElement(ElementBaseType[] element, int currentPos, SuffixTreeNode<ElementBaseType> currentNode)
        {
            if (currentPos < element.Length)
            {
                SuffixTreeNode<ElementBaseType>? nextNode = currentNode.GetTreeNode(element[currentPos]);
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

    }
}
