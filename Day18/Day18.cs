using System.Text;

namespace AdventOfCode2023.Day18
{
    internal class Day18 : Day
    {
        internal override string GetPartOneAnswer(IEnumerable<string> input)
        {
            return Solve(input, line =>
            {
                var split = line.Split();
                var d = split[0];
                var num = int.Parse(split[1]);
                return (d, num);
            });
        }

        internal override string GetPartTwoAnswer(IEnumerable<string> input)
        {
            return Solve(input, line =>
            {
                var dirs = new Dictionary<char, string>
                {
                    {'0', "R"},
                    {'1', "D"},
                    {'2', "L"},
                    {'3', "U"},
                };

                var code = line.Split()[2];
                var d = dirs[code[7]];
                var num = Convert.ToInt32(code[2..7], 16);
                return (d, num);
            });
        }

        private string Solve(IEnumerable<string> input, Func<string, (string, int)> parser)
        {
            var walls = new List<Wall>();
            var x = 0;
            var y = 0;
            var xMin = int.MaxValue;
            var xMax = int.MinValue;
            var dir = "S";
            var firstDir = "S";
            Wall wall = null;
            long sum = 0;
            foreach (var line in input)
            {
                var (d, num) = parser(line);
                sum += num;
                switch (d)
                {
                    case "R":
                        if (wall != null)
                        {
                            if (dir == "D")
                            {
                                wall.CharBot = 'L';
                            }
                            else
                            {
                                wall.CharTop = 'F';
                            }
                            walls.Add(wall);
                        }
                        y += num;
                        break;
                    case "D":
                        wall = new Wall
                        {
                            Col = y,
                            RowTop = x,
                            RowBot = x + num,
                            CharTop = dir == "R" ? '7' : 'F'
                        };
                        x += num;
                        break;
                    case "L":
                        if (wall != null)
                        {
                            if (dir == "D")
                            {
                                wall.CharBot = 'J';
                            }
                            else
                            {
                                wall.CharTop = '7';
                            }
                            walls.Add(wall);
                        }
                        y -= num;
                        break;
                    case "U":
                        wall = new Wall
                        {
                            Col = y,
                            RowTop = x - num,
                            RowBot = x,
                            CharBot = dir == "L" ? 'L' : 'J'
                        };
                        x -= num;
                        break;
                }

                xMax = Math.Max(xMax, x);
                xMin = Math.Min(xMin, x);
                if (firstDir == "S")
                {
                    firstDir = d;
                }
                dir = d;
            }

            if (wall != null)
            {
                if (dir == "D")
                {
                    wall.CharBot = firstDir == "R" ? 'L' : 'J';
                }
                else
                {
                    wall.CharTop = firstDir == "R" ? 'F' : '7';
                }
                walls.Add(wall);
            }

            for (var i = xMin + 1; i < xMax; i++)
            {
                var wallsInRow = walls.Where(w => w.RowTop <= i && w.RowBot >= i).OrderBy(w => w.Col);
                var foundWall = false;
                var bot = 'S';
                var top = 'S';
                var col = 0;
                foreach (var w in wallsInRow)
                {
                    if (i == w.RowTop)
                    {
                        top = w.CharTop;
                        if (top == '7' && bot == 'L')
                        {
                            col = w.Col;
                            bot = 'S';
                        }
                        else if (!foundWall)
                        {
                            foundWall = true;
                            col = w.Col;
                        }
                        else
                        {
                            if (top != '7')
                            {
                                sum += w.Col - col - 1;
                            }
                            foundWall = false;
                        }
                    }
                    else if (i == w.RowBot)
                    {
                        bot = w.CharBot;
                        if (top == 'F' && bot == 'J')
                        {
                            col = w.Col;
                            top = 'S';
                        }
                        else if (!foundWall)
                        {
                            foundWall = true;
                            col = w.Col;
                        }
                        else
                        {
                            if (bot != 'J')
                            {
                                sum += w.Col - col - 1;
                            }
                            foundWall = false;
                        }
                    }
                    else if (foundWall)
                    {
                        sum += w.Col - col - 1;
                        foundWall = false;
                    }
                    else
                    {
                        foundWall = true;
                        col = w.Col;
                    }
                }
            }

            return sum.ToString();
        }

        private record Wall
        {
            public required int Col { get; init; }
            public required int RowTop { get; init; }
            public required int RowBot { get; init; }
            public char CharTop { get; set; }
            public char CharBot { get; set; }
        }
    }
}
