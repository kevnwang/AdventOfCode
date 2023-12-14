namespace AdventOfCode2023.Day14
{
    internal class Day14 : Day
    {
        internal override string GetPartOneAnswer(IEnumerable<string> input)
        {
            var inputArr = input.Select(line => line.ToCharArray()).ToArray();
            TiltNorth(inputArr);

            return GetSum(inputArr).ToString();
        }

        private int GetSum(char[][] inputArr)
        {
            var sum = 0;
            for (int i = 0; i < inputArr.Length; i++)
            {
                for (int j = 0; j < inputArr[i].Length; j++)
                {
                    if (inputArr[i][j] == 'O')
                    {
                        sum += inputArr.Length - i;
                    }
                }
            }
            return sum;
        }

        private void TiltNorth(char[][] inputArr)
        {
            for (int i = 1; i < inputArr.Length; i++)
            {
                for (int j = 0; j < inputArr[i].Length; j++)
                {
                    if (inputArr[i][j] != 'O')
                    {
                        continue;
                    }

                    var indexAbove = i - 1;
                    while (indexAbove >= 0 && inputArr[indexAbove][j] == '.')
                    {
                        indexAbove--;
                    }
                    indexAbove++;
                    if (indexAbove != i)
                    {
                        inputArr[i][j] = '.';
                        inputArr[indexAbove][j] = 'O';
                    }
                }
            }
        }
        private void TiltEast(char[][] inputArr)
        {
            for (int i = inputArr[0].Length - 2; i >= 0; i--)
            {
                for (int j = 0; j < inputArr.Length; j++)
                {
                    if (inputArr[j][i] != 'O')
                    {
                        continue;
                    }

                    var indexRight = i + 1;
                    while (indexRight < inputArr[0].Length && inputArr[j][indexRight] == '.')
                    {
                        indexRight++;
                    }
                    indexRight--;
                    if (indexRight != i)
                    {
                        inputArr[j][i] = '.';
                        inputArr[j][indexRight] = 'O';
                    }
                }
            }
        }

        private void TiltWest(char[][] inputArr)
        {
            for (int i = 1; i < inputArr[0].Length; i++)
            {
                for (int j = 0; j < inputArr.Length; j++)
                {
                    if (inputArr[j][i] != 'O')
                    {
                        continue;
                    }

                    var indexLeft = i - 1;
                    while (indexLeft >= 0 && inputArr[j][indexLeft] == '.')
                    {
                        indexLeft--;
                    }
                    indexLeft++;
                    if (indexLeft != i)
                    {
                        inputArr[j][i] = '.';
                        inputArr[j][indexLeft] = 'O';
                    }
                }
            }
        }

        private void TiltSouth(char[][] inputArr)
        {
            for (int i = inputArr.Length - 2; i >= 0; i--)
            {
                for (int j = 0; j < inputArr[i].Length; j++)
                {
                    if (inputArr[i][j] != 'O')
                    {
                        continue;
                    }

                    var indexBelow = i + 1;
                    while (indexBelow < inputArr.Length && inputArr[indexBelow][j] == '.')
                    {
                        indexBelow++;
                    }
                    indexBelow--;
                    if (indexBelow != i)
                    {
                        inputArr[i][j] = '.';
                        inputArr[indexBelow][j] = 'O';
                    }
                }
            }
        }

        internal override string GetPartTwoAnswer(IEnumerable<string> input)
        {
            var inputArr = input.Select(line => line.ToCharArray()).ToArray();
            var memory = new Dictionary<string, int>();
            var cycle = 0;
            var key = GenerateKey(inputArr);
            while (!memory.ContainsKey(key))
            {
                memory[key] = cycle;
                cycle++;
                RunCycle(inputArr);
                key = GenerateKey(inputArr);
            }

            var cycleLength = cycle - memory[key];
            var remainingCycles = (1000000000 - cycle) % cycleLength;
            foreach (var _ in Enumerable.Range(0, remainingCycles))
            {
                RunCycle(inputArr);
            }
            return GetSum(inputArr).ToString();
        }

        public string GenerateKey(char[][] inputArr) => new string(inputArr.SelectMany(x => new string(x)).ToArray());

        private void RunCycle(char[][] inputArr)
        {
            TiltNorth(inputArr);
            TiltWest(inputArr);
            TiltSouth(inputArr);
            TiltEast(inputArr);
        }
    }
}
