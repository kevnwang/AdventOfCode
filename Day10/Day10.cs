namespace AdventOfCode2023.Day10
{
    internal class Day10 : Day
    {
        internal override string GetPartOneAnswer(IEnumerable<string> input)
        {
            var map = input.ToArray().Select(line => line.ToCharArray()).ToArray();
            var steps = TraverseLoop(map);
            var farthest = steps / 2;
            return farthest.ToString();
        }

        public (int, int) GetStartingLocation(char[][] map)
        {
            for (var i = 0; i < map.Length; i++)
            {
                for (var j = 0; j < map[i].Length; j++)
                {
                    if (map[i][j] == 'S')
                    {
                        return (i, j);
                    }
                }
            }
            throw new Exception("No starting location");
        }

        private (int, int, Direction) GetFirstPosition(char[][] map, int row, int col)
        {
            if (IsValidDirection(map, row, col, Direction.North))
                return (row - 1, col, Direction.North);
            if (IsValidDirection(map, row, col, Direction.East))
                return (row, col + 1, Direction.East);
            if (IsValidDirection(map, row, col, Direction.South))
                return (row + 1, col, Direction.South);
            if (IsValidDirection(map, row, col, Direction.West))
                return (row, col - 1, Direction.West);
            throw new Exception("No valid direction");
        }

        private bool IsValidDirection(char[][] map, int row, int col, Direction direction)
        {
            switch (direction)
            {
                case Direction.North:
                    if (row <= 0) return false;
                    var above = map[row - 1][col];
                    return above == '|' || above == '7' || above == 'F';
                case Direction.East:
                    if (col >= map[row].Length - 1) return false;
                    var right = map[row][col + 1];
                    return right == '-' || right == '7' || right == 'J';
                case Direction.South:
                    if (row >= map.Length - 1) return false;
                    var down = map[row + 1][col];
                    return down == '|' || down == 'J' || down == 'L';
                case Direction.West:
                    if (col <= 0) return false;
                    var left = map[row][col - 1];
                    return left == '-' || left == 'F' || left == 'L';
                default:
                    throw new Exception("Invalid direction");
            }
        }

        private (int, int, Direction) GetNextPosition(char[][] map, int row, int col, Direction directionFrom, bool[,]? loopLocations = default)
        {
            var symbol = map[row][col];
            if (loopLocations != null)
            {
                loopLocations[row, col] = true;
            }
            switch (symbol)
            {
                case '-':   
                    switch (directionFrom)
                    {
                        case Direction.East:
                            return (row, col + 1, directionFrom);
                        case Direction.West:
                            return (row, col - 1, directionFrom);
                        default:
                            throw new Exception("Invalid direction");
                    }
                case '|':
                    switch (directionFrom)
                    {
                        case Direction.North:
                            return (row - 1, col, directionFrom);
                        case Direction.South:
                            return (row + 1, col, directionFrom);
                        default:
                            throw new Exception("Invalid direction");
                    }
                case '7':
                    switch (directionFrom)
                    {
                        case Direction.North:
                            return (row, col - 1, Direction.West);
                        case Direction.East:
                            return (row + 1, col, Direction.South);
                        default:
                            throw new Exception("Invalid direction");
                    }
                case 'J':
                    switch (directionFrom)
                    {
                        case Direction.East:
                            return (row - 1, col, Direction.North);
                        case Direction.South:
                            return (row, col - 1, Direction.West);
                        default:
                            throw new Exception("Invalid direction");
                    }
                case 'L':
                    switch (directionFrom)
                    {
                        case Direction.South:
                            return (row, col + 1, Direction.East);
                        case Direction.West:
                            return (row - 1, col, Direction.North);
                        default:
                            throw new Exception("Invalid direction");
                    }
                case 'F':
                    switch (directionFrom)
                    {
                        case Direction.North:
                            return (row, col + 1, Direction.East);
                        case Direction.West:
                            return (row + 1, col, Direction.South);
                        default:
                            throw new Exception("Invalid direction");
                    }
                default:
                    throw new Exception("Invalid symbol");
            }
        }

        private enum Direction
        {
            North,
            East,
            South,
            West
        }

        private int TraverseLoop(char[][] map, bool[,]? loopLocations = default)
        {
            var (startRow, startCol) = GetStartingLocation(map);
            var (nextRow, nextCol, direction) = GetFirstPosition(map, startRow, startCol);
            var symbol = map[nextRow][nextCol];
            int steps = 1;
            while (symbol != 'S')
            {
                (nextRow, nextCol, direction) = GetNextPosition(map, nextRow, nextCol, direction, loopLocations);
                symbol = map[nextRow][nextCol];
                steps++;
            }
            if (loopLocations != null)
            {
                loopLocations[nextRow, nextCol] = true;
            }
            return steps;
        }

        internal override string GetPartTwoAnswer(IEnumerable<string> input)
        {
            var map = input.ToArray().Select(line => line.ToCharArray()).ToArray();
            var loopLocations = new bool[map.Length, map[0].Length];
            TraverseLoop(map, loopLocations: loopLocations);
            var numTiles = 0;
            for (var i = 0; i < map.Length; i++)
            {
                char lastWall = default;
                bool foundWall = false;
                for (var j = 0; j < map[i].Length; j++)
                {
                    if (loopLocations[i, j])
                    {
                        if (map[i][j] != '-')
                        {
                            if (!(lastWall == 'L' && map[i][j] == '7') && !(lastWall == 'F' && map[i][j] == 'J'))
                            {
                                foundWall = !foundWall;
                            }
                            lastWall = map[i][j];
                        }
                    }
                    else if (foundWall)
                    {
                        numTiles++;
                    }
                }
            }   
            return numTiles.ToString();
        }
    }
}
