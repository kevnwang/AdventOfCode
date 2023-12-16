namespace AdventOfCode2023.Day16
{
    internal class Day16 : Day
    {
        internal override string GetPartOneAnswer(IEnumerable<string> input)
        {
            var map = input.Select(line => line.ToCharArray()).ToArray();
            var energized = new Dictionary<(int, int), HashSet<Alignment>>();
            Traverse(map, 0, 0, Direction.East, energized);
            return energized.Count().ToString();
        }

        private void GoDirection(char[][] map, int row, int col, Direction direction, IDictionary<(int, int), HashSet<Alignment>> energized)
        {
            switch (direction)
            {
                case Direction.North:
                    Traverse(map, row - 1, col, Direction.North, energized);
                    break;
                case Direction.East:
                    Traverse(map, row, col + 1, Direction.East, energized);
                    break;
                case Direction.South:
                    Traverse(map, row + 1, col, Direction.South, energized);
                    break;
                case Direction.West:
                    Traverse(map, row, col - 1, Direction.West, energized);
                    break;
            }
        }

        private Alignment GetAlignment(Direction direction)
        {
            switch (direction)
            {
                case Direction.North:
                case Direction.South:
                    return Alignment.Vertical;
                case Direction.East:
                case Direction.West:
                    return Alignment.Horizontal;
                default:
                    throw new Exception("Invalid direction");
            }
        }

        private void Traverse(char[][] map, int row, int col, Direction direction, IDictionary<(int, int), HashSet<Alignment>> energized)
        {
            if (row < 0 || col < 0 || row >= map.Length || col >= map[row].Length)
            {
                return;
            }

            if (map[row][col] == '.' && energized.TryGetValue((row, col), out var alignments))
            {
                if (alignments.Contains(GetAlignment(direction)))
                {
                    return;
                }
            }

            if (!energized.ContainsKey((row, col)))
            {
                energized[(row, col)] = new HashSet<Alignment>();
            }
            energized[(row, col)].Add(GetAlignment(direction));

            switch (map[row][col])
            {
                case '.':
                    GoDirection(map, row, col, direction, energized);
                    break;
                case '/':
                    switch (direction)
                    {
                        case Direction.North:
                            direction = Direction.East;
                            break;
                        case Direction.East:
                            direction = Direction.North;
                            break;
                        case Direction.South:
                            direction = Direction.West;
                            break;
                        case Direction.West:
                            direction = Direction.South;
                            break;
                    }
                    GoDirection(map, row, col, direction, energized);
                    break;
                case '\\':
                    switch (direction)
                    {
                        case Direction.North:
                            direction = Direction.West;
                            break;
                        case Direction.East:
                            direction = Direction.South;
                            break;
                        case Direction.South:
                            direction = Direction.East;
                            break;
                        case Direction.West:
                            direction = Direction.North;
                            break;
                    }
                    GoDirection(map, row, col, direction, energized);
                    break;
                case '-':
                    switch (direction)
                    {
                        case Direction.North:
                        case Direction.South:
                            GoDirection(map, row, col, Direction.East, energized);
                            GoDirection(map, row, col, Direction.West, energized);
                            break;
                        case Direction.East:
                            GoDirection(map, row, col, Direction.East, energized);
                            break;
                        case Direction.West:
                            GoDirection(map, row, col, Direction.West, energized);
                            break;
                    }
                    break;
                case '|':
                    switch (direction)
                    {
                        case Direction.East:
                        case Direction.West:
                            GoDirection(map, row, col, Direction.North, energized);
                            GoDirection(map, row, col, Direction.South, energized);
                            break;
                        case Direction.North:
                            GoDirection(map, row, col, Direction.North, energized);
                            break;
                        case Direction.South:
                            GoDirection(map, row, col, Direction.South, energized);
                            break;
                    }
                    break;
            }
        }

        private enum Direction
        {
            North,
            East,
            South,
            West
        }

        private enum Alignment
        {
            Vertical,
            Horizontal
        }

        internal override string GetPartTwoAnswer(IEnumerable<string> input)
        {
            var map = input.Select(line => line.ToCharArray()).ToArray();
            var energized = new Dictionary<(int, int), HashSet<Alignment>>();
            var maxEnergized = 0;
            for (var row = 0; row < map.Length; row++)
            {
                Traverse(map, row, 0, Direction.East, energized);
                maxEnergized = Math.Max(maxEnergized, energized.Count());
                energized.Clear();

                Traverse(map, row, map[row].Length - 1, Direction.West, energized);
                maxEnergized = Math.Max(maxEnergized, energized.Count());
                energized.Clear();
            }

            for (var col = 0; col < map[0].Length; col++)
            {
                Traverse(map, 0, col, Direction.South, energized);
                maxEnergized = Math.Max(maxEnergized, energized.Count());
                energized.Clear();

                Traverse(map, map.Length - 1, col, Direction.North, energized);
                maxEnergized = Math.Max(maxEnergized, energized.Count());
                energized.Clear();
            }
            return maxEnergized.ToString();
        }
    }
}
