using System;
using System.Collections.Generic;

public interface Node
{
    // The adjacency of the Node
    List<Node> Adjacency();
    // List of Nodes that satisfied the condition
    List<Node> Path();
    bool Connect(Node node, Condition evaluate);
}

/*
create 2 nodes, A and B perspectively, A connect B with condition X

X evaluate receive A and B
so X is not generics if A and B are not generic, otherwise X is generics
*/

public class TheNode : Node
{
    int count;
    List<Node> next;
    List<Condition> conditions;
    public TheNode()
    {
        next = new List<Node>();
        conditions = new List<Condition>();
    }

    public List<Node> Adjacency()
    {
        return next;
    }

    public bool Connect(Node node, Condition condition)
    {
        // avoid cyclic
        if (cyclic(node))
        {
            return false;
        }

        next.Add(node);
        conditions.Add(condition);
        count++;

        return true;
    }

    public List<Node> Path()
    {
        List<Node> nodes = new List<Node>();
        for (int i = 0; i < count; i++)
        {
            if (conditions[i].Evaluate())
            {
                nodes.Add(next[i]);
            }
        }

        return nodes;
    }

    private bool cyclic(Node insert)
    {
        List<Node> adjacency = next;
        HashSet<Node> visited = new HashSet<Node>();
        Stack<Node> stack = new Stack<Node>();
        visited.Add(this);
        stack.Push(insert);

        while (stack.Count > 0)
        {
            int length = stack.Count;
            List<Node> newNode = new List<Node>();
            for (int i = 0; i < length; i++)
            {
                Node node = stack.Pop();
                if (visited.Contains(node))
                {
                    return true;
                }

                visited.Add(node);
                newNode.AddRange(node.Adjacency());
            }

            for (int i = 0; i < newNode.Count; i++)
            {
                stack.Push(newNode[i]);
            }
        }

        return false;
    }
}