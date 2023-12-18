namespace AdventOfCode2023.Day17
{
    internal class Day17 : Day
    {
        internal override string GetPartOneAnswer(IEnumerable<string> input)
        {
            return Solve(input);
        }

        private string Solve(IEnumerable<string> input, bool isUltraCrucible = false)
        {
            var map = input.Select(line => line.ToCharArray().Select(c => c - '0').ToArray()).ToArray();

            var frontier = new PriorityQueue<Node, int>();
            var starterNode = new Node(row: 0, col: 0, direction: default, consecutive: 0, visited: new HashSet<(int, int)> { (0, 0) });
            frontier.Enqueue(element: starterNode, priority: 0);

            var memory = new Dictionary<(int, int), HashSet<(Direction, int)>>();

            while (frontier.TryDequeue(out var current, out var currentDistance))
            {
                if (current.row == map.Length - 1 && current.col == map[current.row].Length - 1)
                {
                    if (isUltraCrucible && current.consecutive < 4)
                    {
                        continue;
                    }

                    return currentDistance.ToString();
                }

                AdjustFrontierNode(map, current.row - 1, current.col, Direction.Up, current, currentDistance, frontier, memory, isUltraCrucible);
                AdjustFrontierNode(map, current.row + 1, current.col, Direction.Down, current, currentDistance, frontier, memory, isUltraCrucible);
                AdjustFrontierNode(map, current.row, current.col - 1, Direction.Left, current, currentDistance, frontier, memory, isUltraCrucible);
                AdjustFrontierNode(map, current.row, current.col + 1, Direction.Right, current, currentDistance, frontier, memory, isUltraCrucible);
            }

            throw new Exception("Can't reach end");
        }

        private void AdjustFrontierNode(
            int[][] map,
            int row,
            int col,
            Direction toDirection,
            Node current,
            int currentDistance,
            PriorityQueue<Node, int> frontier,
            Dictionary<(int, int), HashSet<(Direction, int)>> memory,
            bool isUltraCrucible = false)
        {
            if (row < 0 || row >= map.Length || col < 0 || col >= map[row].Length
                || current.visited.Contains((row, col)))
            {
                return;
            }

            if (isUltraCrucible)
            {
                if (toDirection != current.direction && current.consecutive < 4 && current.consecutive > 0 || toDirection == current.direction && current.consecutive == 10)
                {
                    return;
                }
            }
            else
            {
                if (toDirection == current.direction && current.consecutive == 3)
                {
                    return;
                }
            }

            var dist = currentDistance + map[row][col];
            var newVisited = new HashSet<(int, int)>(current.visited) { (row, col) };
            var newConsecutive = toDirection == current.direction ? current.consecutive + 1 : 1;

            if (memory.TryGetValue((row, col), out var values) && values.Contains((toDirection, newConsecutive)))
            {
                return;
            }

            if (!memory.ContainsKey((row, col)))
            {
                memory[(row, col)] = new HashSet<(Direction, int)>();
            }
            memory[(row, col)].Add((toDirection, newConsecutive));

            var node = new Node(row, col, toDirection, newConsecutive, newVisited);
            frontier.Enqueue(node, dist);
        }

        private record Node (int row, int col, Direction direction, int consecutive, HashSet<(int, int)> visited);

        private enum Direction
        {
            Up,
            Down,
            Left,
            Right
        }

        internal override string GetPartTwoAnswer(IEnumerable<string> input)
        {
            return Solve(input, isUltraCrucible: true);
        }
    }
}
