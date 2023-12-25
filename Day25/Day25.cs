namespace AdventOfCode2023.Day25
{
    internal class Day25 : Day
    {
        internal override string GetPartOneAnswer(IEnumerable<string> input)
        {
            var graph = new Dictionary<string, HashSet<string>>();
            foreach (var line in input)
            {
                var split = line.Split(": ");
                var node = split[0];
                var connections = split[1].Split();
                if (!graph.ContainsKey(node))
                {
                    graph[node] = [];
                }
                foreach (var connection in connections)
                {
                    if (!graph.ContainsKey(connection))
                    {
                        graph[connection] = [];
                    }
                    graph[node].Add(connection);
                    graph[connection].Add(node);
                }
            }

            var random = new Random();
            var counts = new Dictionary<(string, string), int>();
            foreach (var _ in Enumerable.Range(0, 1000))
            {
                var src = graph.ElementAt(random.Next(graph.Count)).Key;
                var dest = graph.ElementAt(random.Next(graph.Count)).Key;

                var bfs = new Queue<(string, List<string>)>();
                bfs.Enqueue((src, new List<string>()));
                var visited = new HashSet<string>();
                while (bfs.TryDequeue(out var bfsNode))
                {
                    var (node, path) = bfsNode;
                    if (!visited.Contains(node))
                    {
                        if (node == dest)
                        {
                            path.Add(src);
                            for (var i = 1; i < path.Count; i++)
                            {
                                var n1 = path[i - 1];
                                var n2 = path[i];
                                UpdateCounts(counts, n1, n2);
                            }
                            break;
                        }

                        visited.Add(node);
                        foreach (var connection in graph[node])
                        {
                            bfs.Enqueue((connection, new List<string>(path) { node }));
                        }
                    }
                }
            }

            var countList = counts.ToList();
            countList.Sort((x, y) => -x.Value.CompareTo(y.Value));
            var top3 = countList.Take(3).Select(x => x.Key);
            foreach (var (n1, n2) in top3)
            {
                graph[n1].Remove(n2);
                graph[n2].Remove(n1);
            }

            var connectedComponents = GetConnectedComponents(graph);
            if (connectedComponents.Count != 2)
            {
                throw new Exception("Not two connected components");
            }
            var mult = connectedComponents.ElementAt(0).Count * connectedComponents.ElementAt(1).Count;
            return mult.ToString();
        }

        private void UpdateCounts(Dictionary<(string, string), int> counts, string n1, string n2)
        {
            if (counts.ContainsKey((n1, n2)))
            {
                counts[(n1, n2)]++;
            }
            else if (counts.ContainsKey((n2, n1)))
            {
                counts[(n2, n1)]++;
            }
            else
            {
                counts[(n1, n2)] = 1;
            }
        }

        private Stack<HashSet<string>> GetConnectedComponents(Dictionary<string, HashSet<string>> graph)
        {
            var nodes = graph.Keys;
            var visited = new HashSet<string>();
            var connectedComponents = new Stack<HashSet<string>>();
            foreach (var node in nodes)
            {
                if (visited.Contains(node))
                {
                    continue;
                }

                connectedComponents.Push(new HashSet<string> { node });

                var bfs = new Queue<string>();
                bfs.Enqueue(node);

                while (bfs.TryDequeue(out var bfsNode))
                {
                    var connections = graph[bfsNode];
                    visited.Add(bfsNode);
                    connectedComponents.Peek().Add(bfsNode);
                    foreach (var connection in connections)
                    {
                        if (!visited.Contains(connection))
                        {
                            bfs.Enqueue(connection);
                        }
                    }
                }
            }
            return connectedComponents;
        }

        internal override string GetPartTwoAnswer(IEnumerable<string> input)
        {
            return "Free star";
        }
    }
}
