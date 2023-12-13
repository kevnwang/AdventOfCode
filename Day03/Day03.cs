using System.Text.RegularExpressions;

namespace AdventOfCode2023.Day03
{
    internal class Day03 : Day
    {
        internal override string GetPartOneAnswer(IEnumerable<string> input)
        {
            var inputArr = input.ToArray();
            var sum = 0;
            for (var i = 0; i < inputArr.Length; i++)
            {
                var line = inputArr[i];
                var numbers = Regex.Split(line, @"\D+").Where(num => !string.IsNullOrEmpty(num));
                var index = 0;
                foreach (var num in numbers)
                {
                    index = line.Substring(index).IndexOf(num) + index;
                    for (var j = index; j < num.Length + index; j++)
                    {
                        (var isAdjacent, _, _) = IsSymbolAdjacent(inputArr, i, j);
                        if (isAdjacent)
                        {
                            sum += int.Parse(num);
                            break;
                        }
                    }

                    index += num.Length;
                }
            }
            return sum.ToString();
        }

        private (bool, int, int) IsSymbolAdjacent(string[] inputArr, int row, int col, bool checkGear = false)
        {
            var topLeft = IsSymbol(inputArr, row - 1, col - 1, checkGear);
            if (topLeft.Item1) return topLeft;

            var top = IsSymbol(inputArr, row - 1, col, checkGear);
            if (top.Item1) return top;

            var topRight = IsSymbol(inputArr, row - 1, col + 1, checkGear);
            if (topRight.Item1) return topRight;

            var left = IsSymbol(inputArr, row, col - 1, checkGear);
            if (left.Item1) return left;

            var right = IsSymbol(inputArr, row, col + 1, checkGear);
            if (right.Item1) return right;

            var bottomLeft = IsSymbol(inputArr, row + 1, col - 1, checkGear);
            if (bottomLeft.Item1) return bottomLeft;

            var bottom = IsSymbol(inputArr, row + 1, col, checkGear);
            if (bottom.Item1) return bottom;

            var bottomRight = IsSymbol(inputArr, row + 1, col + 1, checkGear);
            if (bottomRight.Item1) return bottomRight;

            return (false, default, default);
        }

        private (bool, int, int) IsSymbol(string[] inputArr, int row, int col, bool checkGear)
        {
            if (row < 0 || row >= inputArr.Length)
            {
                return (false, row, col);
            }

            if (col < 0 || col >= inputArr[row].Length)
            {
                return (false, row, col);
            }

            var c = inputArr[row][col];

            if (checkGear)
            {
                return (c == '*', row, col);
            }

            return (!char.IsDigit(c) && c != '.', row, col);
        }

        internal override string GetPartTwoAnswer(IEnumerable<string> input)
        {
            var gearRatios = new Dictionary<(int, int), IEnumerable<int>>();

            var inputArr = input.ToArray();
            for (var i = 0; i < inputArr.Length; i++)
            {
                var line = inputArr[i];
                var numbers = Regex.Split(line, @"\D+").Where(num => !string.IsNullOrEmpty(num));
                var index = 0;
                foreach (var num in numbers)
                {
                    index = line.Substring(index).IndexOf(num) + index;
                    for (var j = index; j < num.Length + index; j++)
                    {
                        var (isAdjacent, row, col) = IsSymbolAdjacent(inputArr, i, j, checkGear: true);
                        if (isAdjacent)
                        {
                            gearRatios[(row, col)] = gearRatios.TryGetValue((row, col), out var list) ? list.Append(int.Parse(num)) : new List<int> { int.Parse(num) };
                            break;
                        }
                    }

                    index += num.Length;
                }
            }

            return gearRatios
                .Where(kvp => kvp.Value.Count() == 2)
                .Select(kvp => kvp.Value.Aggregate((a, b) => a * b))
                .Sum()
                .ToString();
        }
    }
}
