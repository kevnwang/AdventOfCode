namespace AdventOfCode2023.Day01
{
    internal class Day01 : Day
    {
        private readonly IReadOnlyDictionary<string, char> _numberStrs = new Dictionary<string, char>
            {
                { "one", '1' },
                { "two", '2' },
                { "three", '3' },
                { "four", '4' },
                { "five", '5' },
                { "six", '6' },
                { "seven", '7' },
                { "eight", '8' },
                { "nine", '9' },
            };
        internal override string GetPartOneAnswer(IEnumerable<string> input)
        {
            var sum = 0;
            foreach (var line in input)
            {
                char firstDigit = default;
                for (int i = 0; i < line.Length; i++)
                {
                    if (char.IsDigit(line[i]))
                    {
                        firstDigit = line[i];
                        break;
                    }
                }

                char lastDigit = default;
                for (int i = line.Length - 1; i >= 0; i--)
                {
                    if (char.IsDigit(line[i]))
                    {
                        lastDigit = line[i];
                        break;
                    }
                }

                sum += int.Parse(firstDigit.ToString() + lastDigit.ToString());
            }

            return sum.ToString();
        }

        internal override string GetPartTwoAnswer(IEnumerable<string> input)
        {
            var sum = 0;
            foreach (var line in input)
            {
                char firstDigit = default;
                for (int i = 0; i < line.Length; i++)
                {
                    if (char.IsDigit(line[i]))
                    {
                        firstDigit = line[i];
                        break;
                    }
                    else if (TryScanNumberForward(line, i, out var numberScanned))
                    {
                        firstDigit = numberScanned;
                        break;
                    }
                }

                char lastDigit = default;
                for (int i = line.Length - 1; i >= 0; i--)
                {
                    if (char.IsDigit(line[i]))
                    {
                        lastDigit = line[i];
                        break;
                    } 
                    else if (TryScanNumberBackward(line, i, out var numberScanned))
                    {
                        lastDigit = numberScanned;
                        break;
                    }
                }

                sum += int.Parse(firstDigit.ToString() + lastDigit.ToString());
            }

            return sum.ToString();
        }

        private bool TryScanNumberForward(string line, int index, out char numberScanned)
        {
            numberScanned = default;
            foreach (var entry in _numberStrs)
            {
                if (entry.Key.Length + index > line.Length)
                {
                    continue;
                }

                if (entry.Key == line.Substring(index, entry.Key.Length))
                {
                    numberScanned = entry.Value;
                    return true;
                }
            }

            return false;
        }

        private bool TryScanNumberBackward(string line, int index, out char numberScanned)
        {
            numberScanned = default;
            foreach (var entry in _numberStrs)
            {
                if (index - entry.Key.Length < -1)
                {
                    continue;
                }

                if (entry.Key == line.Substring(index - entry.Key.Length + 1, entry.Key.Length))
                {
                    numberScanned = entry.Value;
                    return true;
                }
            }

            return false;
        }
    }
}
