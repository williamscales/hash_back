using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace HashBack
{
    public class HashTree
    {
        private class Node<T>
        {
            public Node<T> Left { get; set; }
            public Node<T> Right { get; set; }
            public T Value { get; set; }

            public Node() { }
            public Node(T data) : this(data, null, null) { }
            public Node(T data, Node<T> left, Node<T> right)
            {
                Left = left;
                Right = right;
                Value = data;
            }
        }

        private Node<byte[]> tree = null;

        public string RootHash
        {
            get { return BitConverter.ToString(tree.Value).Replace("-", ""); }
        }

	    public HashTree(List<byte[]> hashes)
	    {
            // The number of leaf nodes needed is ceil(log2(N)), i.e. the
            // smallest power of 2 >= N where N is the number of hashes
            var N = hashes.Count;
            var width = Math.Ceiling(Math.Log(N, 2));

            // We build the tree from the bottom up, by making a node for each
            // hash. We put these nodes in a queue which we later process two
            // at a time to build the tree.
            var nodes = new Queue<Node<byte[]>>();
            SHA256 hasher = new SHA256Managed();
            foreach (byte[] hash in hashes)
            {
                var node = new Node<byte[]>(hash);
                nodes.Enqueue(node);
            }

            // Make sure there are enough leaf nodes to fill the tree
            if (nodes.Count < width)
            {
                var zeroBytes = new byte[] { 0, 0, 0, 0 };
                var zeroHash = hasher.ComputeHash(zeroBytes);
                var zeroNode = new Node<byte[]>(zeroHash);

                while (nodes.Count < width) { nodes.Enqueue(zeroNode); }
            }

            // Process the nodes two at a time to build the tree
            while (nodes.Count > 1)
            {
                var first = nodes.Dequeue();
                var second = nodes.Dequeue();

                // We hash the concatenation of the two nodes' values and store
                // that in the parent node.
                var length = first.Value.Length + second.Value.Length;
                var toHash = new byte[length];
                for (int i=0; i < first.Value.Length; i++)
                {
                    toHash[i] = first.Value[i];
                }
                for (int i=0; i < second.Value.Length; i++)
                {
                    toHash[first.Value.Length + i] = second.Value[i];
                }
                var hash = hasher.ComputeHash(toHash);
                var parent = new Node<byte[]>(hash, first, second);
                nodes.Enqueue(parent);
            }
            // The last node in the queue is the root
            tree = nodes.Dequeue();
	    }
    }
}