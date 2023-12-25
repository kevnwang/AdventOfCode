namespace AdventOfCode2023.Day21
{
    internal class Day21 : Day
    {
        internal override string GetPartOneAnswer(IEnumerable<string> input)
        {
            var map = input.Select(x => x.ToCharArray()).ToArray();
            var day10 = new Day10.Day10();
            var startingLoc = day10.GetStartingLocation(map);
            var visited = BfsGetVisited(map, startingLoc);
            return visited.Where(kvp => kvp.Value % 2 == 0 && kvp.Value <= 64).Count().ToString();
        }

        internal override string GetPartTwoAnswer(IEnumerable<string> input)
        {
            var map = input.Select(x => x.ToCharArray()).ToArray();
            var day10 = new Day10.Day10();
            var startingLoc = day10.GetStartingLocation(map);
            var visitedFromStarting = BfsGetVisited(map, startingLoc);
            var evenAll = visitedFromStarting.Where(kvp => kvp.Value % 2 == 0).Count();
            var oddAll = visitedFromStarting.Where(kvp => kvp.Value % 2 == 1).Count();

            var halfDim = map.Length / 2;
            var oddCorners = visitedFromStarting.Where(kvp => kvp.Value % 2 == 1 && kvp.Value > halfDim).Count();

            (int, int)[] corners = [(0, 0), (0, map.Length - 1), (map.Length - 1, 0), (map.Length - 1, map.Length - 1)];
            var evenCorners = corners.Select(corner => BfsGetVisited(map, corner).Where(kvp => kvp.Value % 2 == 0 && kvp.Value <= halfDim).Count()).Sum();

            var steps = 26501365;
            long n = steps / map.Length;

            long ans = (n + 1) * (n + 1) * oddAll + n * n * evenAll - (n + 1) * oddCorners + n * evenCorners;

            return ans.ToString();
        }

        private IDictionary<(int row, int col), int> BfsGetVisited(char[][] map, (int, int) startingLoc)
        {
            var bfs = new Queue<((int, int), int)>();
            bfs.Enqueue((startingLoc, 0));
            var visited = new Dictionary<(int row, int col), int>();
            while (bfs.TryDequeue(out var plot))
            {
                var ((row, col), dist) = plot;
                if (visited.ContainsKey((row, col)))
                {
                    continue;
                }
                visited[(row, col)] = dist;
                AddNeighbor(map, row + 1, col, dist + 1, bfs, visited);
                AddNeighbor(map, row - 1, col, dist + 1, bfs, visited);
                AddNeighbor(map, row, col + 1, dist + 1, bfs, visited);
                AddNeighbor(map, row, col - 1, dist + 1, bfs, visited);
            }
            return visited;
        }

        private void AddNeighbor(char[][] map, int row, int col, int dist, Queue<((int, int) coord, int)> bfs, IDictionary<(int, int), int> visited)
        {
            if (row < 0 || col < 0 || row >= map.Length || col >= map[row].Length || visited.ContainsKey((row, col)) || map[row][col] == '#')
            {
                return;
            }

            bfs.Enqueue(((row, col), dist));
        }
    }
}
