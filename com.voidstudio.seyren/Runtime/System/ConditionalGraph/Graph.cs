using System;
using System.Collections.Generic;

public class Graph
{
    List<Node> nodes;
    List<Node> queue;
    bool cached;
    public Graph()
    {
        nodes = new List<Node>();
        queue = new List<Node>();
        cached = false;
    }

    public void Add(Node node)
    {
        nodes.Add(node);
        cached = false;
    }

    public List<Node> TopologicalSort()
    {
        if (cached) return queue;

        Dictionary<Node, List<Node>> graph = new Dictionary<Node, List<Node>>();
        for (int loop = 0; loop < nodes.Count; loop++)
        {
            if (!graph.ContainsKey(nodes[loop]))
            {
                graph.Add(nodes[loop], new List<Node>());
            }

            List<Node> set = graph[nodes[loop]];
            List<Node> adjacency = nodes[loop].Adjacency();
            foreach (var item in adjacency)
            {
                set.Add(item);
            }
        }

        queue = new List<Node>(nodes);
        HashSet<Node> visited = new HashSet<Node>();
        int i = nodes.Count - 1;
        for (int loop = 0; loop < nodes.Count; loop++)
        {
            if (!visited.Contains(nodes[loop])) {
                i = dfsTopological(i, nodes[loop], visited, queue, graph);
            }
        }

        cached = true;
        return queue;   
    }

    private int dfsTopological(int i, Node at, HashSet<Node> visited, List<Node> ordering, Dictionary<Node, List<Node>> graph)
    {
        visited.Add(at);

        List<Node> nodes = graph[at];

        if (nodes != null)
        {
            foreach (Node node in nodes)
            {
                if (!visited.Contains(node))
                {
                    i = dfsTopological(i, node, visited, ordering, graph);
                }
            }
        }

        ordering[i] = at;
        return i - 1;
    }
}