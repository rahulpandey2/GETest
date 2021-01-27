using System;
using System.Collections.Generic;
using System.Linq;


// A C# Program to find distance between 
// n1 and n2 using one traversal 

public class Node
{
    public Node left, right;
    public int key;

    public Node(int key)
    {
        this.key = key;
        left = null;
        right = null;
    }
}

class Tree
{
    public Node insert(Node root, int v, bool isLeft)
    {
        if (root == null)
        {
            root = new Node(v);
        }
        else if (isLeft) //(v < root.key)
        {
            root.left = insert(root.left, v, isLeft);
        }
        else
        {
            root.right = insert(root.right, v, isLeft);
        }

        return root;
    }

    public void traverse(Node root)
    {
        if (root == null)
        {
            return;
        }

        traverse(root.left);
        traverse(root.right);
    }
}


class GFG
{

    static List<int> _leafNodes = new List<int>();
    static void getLeafNodes(Node root)
    {
        // If node is null, return 
        if (root == null)
        {
            return;
        }

        // If node is leaf node, print its data     
        if (root.left == null && root.right == null)
        {
            _leafNodes.Add(root.key);
            return;
        }

        // If left child exists, check for leaf 
        // recursively 
        if (root.left != null)
        {
            getLeafNodes(root.left);
        }

        // If right child exists, check for leaf 
        // recursively 
        if (root.right != null)
        {
            getLeafNodes(root.right);
        }

    }

    // Driver Code
    public static void Main(string[] args)
    {
        Console.WriteLine("Please type input \n4\n3\n2\n3 4\n5 6 7 8\n9 -1 10 11 -1 -1 -1 12\n");


        Console.WriteLine("Output is 4 ");

        



        var k = Convert.ToInt32(Console.ReadLine());
        var height = Convert.ToInt32(Console.ReadLine());

        if (height == -1)
        {
            Console.WriteLine(-1);
        }

        Node root = null;
        Tree bst = new Tree();
        List<int> elements = new List<int>();

        // Read nodes
        for (int i = 0; i < height + 1; i++)
        {
            var input = Console.ReadLine();
            if (input.Length <= 1)
            {
                elements.Add(Convert.ToInt32(input));
            }
            else
            {
                elements.AddRange(input.Split(' ').Select(x => Convert.ToInt32(x)));
            }
        }

        //foreach (var item in elements)
        //{
        //    if (root is null)
        //    {
        //        root = new Node(item);
        //    }

        //    if (root.left is null)
        //        root.left = new Node(item);

        //    if (root.right is null)
        //        root.right = new Node(item);

        //    if (root.left != null && root.right != null)
        //        root = root.left;

        //    root.left.left = new Node(4);
        //    root.left.right = new Node(5);
        //    root.right.left = new Node(6);
        //    root.right.right = new Node(7);
        //    root.right.left.right = new Node(8);

        //}

        bool isLeft = true;
        bool isRoot = true;
        foreach (var item in elements)
        {
            root = bst.insert(root, item, isLeft);
            if (!isRoot)
                isLeft = !isLeft;
            isRoot = false;
        }

        getLeafNodes(root);

        Dictionary<string, int> keyValuePairs = new Dictionary<string, int>();
        // 1 3 5 7
        for (int i = 0; i < _leafNodes.Count; i++)
        {
            if (_leafNodes[i] == -1)
                continue;
            for (int j = 1; j < _leafNodes.Count - i; j++)
            {
                if (_leafNodes[j] == -1)
                    continue;

                keyValuePairs.Add($"({_leafNodes[i]},{_leafNodes[i + j]})", findDistance(root, _leafNodes[i], _leafNodes[i + j]));
            }
        }

        Console.WriteLine((from kvp in keyValuePairs where kvp.Value <= k select kvp.Value).Count());

    }

    // (To the moderator) in c++ solution these
    // variables are declared as pointers hence 
    //changes made to them reflects in the whole program 

    // Global static variable 
    public static int d1 = -1;
    public static int d2 = -1;
    public static int dist = 0;

    // A Binary Tree Node 

    // Returns level of key k if it is present 
    // in tree, otherwise returns -1 
    public static int findLevel(Node root, int k, int level)
    {
        // Base Case 
        if (root == null)
        {
            return -1;
        }

        // If key is present at root, or in left 
        // subtree or right subtree, return true; 
        if (root.key == k)
        {
            return level;
        }

        int l = findLevel(root.left, k, level + 1);
        return (l != -1) ? l : findLevel(root.right, k,
                                            level + 1);
    }

    // This function returns pointer to LCA of 
    // two given values n1 and n2. It also sets
    // d1, d2 and dist if one key is not ancestor of other 
    // d1 --> To store distance of n1 from root 
    // d2 --> To store distance of n2 from root 
    // lvl --> Level (or distance from root) of current node 
    // dist --> To store distance between n1 and n2 
    public static Node findDistUtil(Node root, int n1, int n2, int lvl)
    {

        // Base case 
        if (root == null)
        {
            return null;
        }

        // If either n1 or n2 matches with root's 
        // key, report the presence by returning 
        // root (Note that if a key is ancestor of 
        // other, then the ancestor key becomes LCA 
        if (root.key == n1)
        {
            d1 = lvl;
            return root;
        }
        if (root.key == n2)
        {
            d2 = lvl;
            return root;
        }

        // Look for n1 and n2 in left and right subtrees 
        Node left_lca = findDistUtil(root.left, n1,
                                    n2, lvl + 1);
        Node right_lca = findDistUtil(root.right, n1,
                                        n2, lvl + 1);

        // If both of the above calls return Non-NULL, 
        // then one key is present in once subtree and 
        // other is present in other, So this node is the LCA 
        if (left_lca != null && right_lca != null)
        {
            dist = (d1 + d2) - 2 * lvl;
            return root;
        }

        // Otherwise check if left subtree 
        // or right subtree is LCA 
        return (left_lca != null) ? left_lca : right_lca;
    }

    // The main function that returns distance 
    // between n1 and n2. This function returns -1 
    // if either n1 or n2 is not present in 
    // Binary Tree. 
    public static int findDistance(Node root, int n1, int n2)
    {
        d1 = -1;
        d2 = -1;
        dist = 0;
        Node lca = findDistUtil(root, n1, n2, 1);

        // If both n1 and n2 were present
        // in Binary Tree, return dist 
        if (d1 != -1 && d2 != -1)
        {
            return dist;
        }

        // If n1 is ancestor of n2, consider
        // n1 as root and find level 
        // of n2 in subtree rooted with n1 
        if (d1 != -1)
        {
            dist = findLevel(lca, n2, 0);
            return dist;
        }

        // If n2 is ancestor of n1, consider 
        // n2 as root and find level 
        // of n1 in subtree rooted with n2 
        if (d2 != -1)
        {
            dist = findLevel(lca, n1, 0);
            return dist;
        }

        return -1;
    }

}

