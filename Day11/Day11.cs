namespace AdventOfCode2023.Day11
{
    internal class Day11 : Day
    {
        internal override string GetPartOneAnswer(IEnumerable<string> input)
        {
            return Solve(input);
        }

        public string Solve(IEnumerable<string> input, int multiplier = 2)
        {
            var inputChars = input.Select(line => line.ToCharArray());
            var strArr = inputChars.Select(row => row.Select(c => c.ToString()).ToArray()).ToArray();
            Expand(strArr, multiplier);
            strArr = Flip(strArr);
            Expand(strArr, multiplier);
            strArr = Flip(strArr);
            var galaxies = GetGalaxies(strArr);
            return GetSum(strArr, galaxies.ToArray()).ToString();
        }

        public void Expand(string[][] inputArr, int multiplier = 2)
        {
            for (var i = inputArr.Length - 1; i >= 0; i--)
            {
                if (inputArr[i].All(c => c == "." || int.TryParse(c, out _)))
                {
                    Array.Fill(inputArr[i], multiplier.ToString());
                }
            }
        }

        public string[][] Flip(string[][] inputArr)
        {
            var flippedArr = new string[inputArr[0].Length][];
            for (var i = 0; i < inputArr[0].Length; i++)
            {
                flippedArr[i] = inputArr.Select(row => row[i]).ToArray();
            }
            return flippedArr;
        }

        public IEnumerable<(int, int)> GetGalaxies(string[][] inputArr)
        {
            var galaxies = new List<(int, int)>();
            for (int i = 0; i < inputArr.Length; i++)
            {
                for (int j = 0; j < inputArr[i].Length; j++)
                {
                    var str = inputArr[i][j];
                    if (str == "#")
                    {
                        galaxies.Add((i, j));
                    }
                }
            }
            return galaxies;
        }

        public long GetSum(string[][] map, (int, int)[] galaxiesArr)
        {
            long sum = 0;
            for (int i = 0; i < galaxiesArr.Length; i++)
            {
                for (int j = i + 1; j < galaxiesArr.Length; j++)
                {
                    sum += CountSteps(map, galaxiesArr[i], galaxiesArr[j]);
                }
            }
            return sum;
        }

        public long CountSteps(string[][] map, (int row, int col) g1, (int row, int col) g2)
        {
            int minRow = Math.Min(g1.row, g2.row);
            int maxRow = Math.Max(g1.row, g2.row);
            int minCol = Math.Min(g1.col, g2.col);
            int maxCol = Math.Max(g1.col, g2.col);

            long steps = 0;
            for (int i = minRow + 1; i <= maxRow; i++)
            {
                var s = map[i][minCol];
                if (int.TryParse(s, out var multiplier))
                {
                    steps += multiplier;
                }
                else
                {
                    steps++;
                }
            }

            for (int j = minCol + 1; j <= maxCol; j++)
            {
                var s = map[maxRow][j];
                if (int.TryParse(s, out var multiplier))
                {
                    steps += multiplier;
                }
                else
                {
                    steps++;
                }
            }

            return steps;
        }

        internal override string GetPartTwoAnswer(IEnumerable<string> input)
        {
            return Solve(input, multiplier: 1000000);
        }
    }
}
