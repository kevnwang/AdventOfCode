namespace AdventOfCode2023.Day23
{
    internal class Day23 : Day
    {
        internal override string GetPartOneAnswer(IEnumerable<string> input)
        {
            return Solve(input);
        }

        private string Solve(IEnumerable<string> input, bool considerSlopes = true)
        {
            var map = input.Select(line => line.ToCharArray()).ToArray();
            var startingCol = Array.IndexOf(map[0], '.');

            var graph = new Dictionary<(int, int), IDictionary<(int, int), int>>();

            var bfsVertices = new Queue<(int row, int col)>();
            bfsVertices.Enqueue((0, startingCol));
            while (bfsVertices.TryDequeue(out var vertex))
            {
                if (graph.ContainsKey(vertex))
                {
                    continue;
                }
                graph[vertex] = new Dictionary<(int, int), int>();

                var bfs = new Queue<(int row, int col, HashSet<(int, int)> visited)>();
                var neighbors = GetValidNeighbors(map, vertex.row, vertex.col);
                foreach (var neighbor in neighbors)
                {
                    bfs.Enqueue((neighbor.row, neighbor.col, new HashSet<(int, int)> { vertex }));
                }
                while (bfs.TryDequeue(out var coord))
                {
                    var (row, col, visited) = coord;

                    if (row < 0 || col < 0 || row >= map.Length || col >= map[row].Length || map[row][col] == '#' || visited.Contains((row, col)))
                    {
                        continue;
                    }

                    if (considerSlopes)
                    {
                        switch (map[row][col])
                        {
                            case '>':
                                bfs.Enqueue((row, col + 1, new HashSet<(int, int)>(visited) { (row, col) }));
                                continue;
                            case '^':
                                bfs.Enqueue((row - 1, col, new HashSet<(int, int)>(visited) { (row, col) }));
                                continue;
                            case '<':
                                bfs.Enqueue((row, col - 1, new HashSet<(int, int)>(visited) { (row, col) }));
                                continue;
                            case 'v':
                                bfs.Enqueue((row + 1, col, new HashSet<(int, int)>(visited) { (row, col) }));
                                continue;
                        }
                    }

                    if (GetValidNeighbors(map, row, col).Count() > 2 || row == 0 || row == map.Length - 1)
                    {
                        graph[vertex][(row, col)] = visited.Count();
                        bfsVertices.Enqueue((row, col));
                        continue;
                    }

                    bfs.Enqueue((row, col + 1, new HashSet<(int, int)>(visited) { (row, col) }));
                    bfs.Enqueue((row - 1, col, new HashSet<(int, int)>(visited) { (row, col) }));
                    bfs.Enqueue((row, col - 1, new HashSet<(int, int)>(visited) { (row, col) }));
                    bfs.Enqueue((row + 1, col, new HashSet<(int, int)>(visited) { (row, col) }));
                }
            }

            var bfsDists = new Queue<((int row, int col), IDictionary<(int, int), int>)>();
            bfsDists.Enqueue(((0, startingCol), new Dictionary<(int, int), int>()));
            var maxDist = 0;
            while (bfsDists.TryDequeue(out var vertex))
            {
                var (coord, visited) = vertex;

                if (coord.row == map.Length - 1)
                {
                    var sumDists = visited.Select(v => v.Value).Sum();
                    maxDist = Math.Max(maxDist, sumDists);
                    continue;
                }

                var neighbors = graph[coord];
                foreach (var (neighborCoord, neighborDist) in neighbors)
                {
                    if (!visited.ContainsKey(neighborCoord))
                    {
                        bfsDists.Enqueue((neighborCoord, new Dictionary<(int, int), int>(visited) { { coord, neighborDist } }));
                    }
                }
            }
            return maxDist.ToString();
        }

        private IEnumerable<(int row, int col)> GetValidNeighbors(char[][] map, int row, int col)
        {
            var neighbors = new List<(int, int)>();
            if (IsValidNeighbor(map, row + 1, col))
            {
                neighbors.Add((row + 1, col));
            }
            if (IsValidNeighbor(map, row - 1, col))
            {
                neighbors.Add((row - 1, col));
            }
            if (IsValidNeighbor(map, row, col + 1))
            {
                neighbors.Add((row, col + 1));
            }
            if (IsValidNeighbor(map, row, col - 1))
            {
                neighbors.Add((row, col - 1));
            }
            return neighbors;
        }

        private bool IsValidNeighbor(char[][] map, int row, int col) => row >= 0 && col >= 0 && row < map.Length && col < map[row].Length && map[row][col] != '#';

        internal override string GetPartTwoAnswer(IEnumerable<string> input)
        {
            return Solve(input, considerSlopes: false);
        }
    }
}
