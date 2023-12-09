namespace AdventOfCode2023.Day8
{
    internal class Day8 : Day
    {
        internal override string GetPartOneAnswer(IEnumerable<string> input)
        {
            var map = GetMapAndInstructions(input, out var instructions);

            var next = instructions[0] == 'L' ? map["AAA"].Item1 : map["AAA"].Item2;
            var steps = 1;
            var instructionIndex = 1;
            while (next != "ZZZ")
            {
                var nextInstruction = instructions[instructionIndex];
                next = nextInstruction == 'L' ? map[next].Item1 : map[next].Item2;
                instructionIndex++;
                if (instructionIndex == instructions.Length)
                {
                    instructionIndex = 0;
                }
                steps++;
            }

            return steps.ToString();
        }

        private Dictionary<string, (string, string)> GetMapAndInstructions(IEnumerable<string> input, out string instructions)
        {
            instructions = string.Empty;
            var map = new Dictionary<string, (string, string)>();
            var inputArr = input.ToArray();
            for (var i = 0; i < inputArr.Length; i++)
            {
                if (i == 0)
                {
                    instructions = inputArr[i];
                    i++;
                }
                else
                {
                    var line = inputArr[i];
                    var key = line[0..3];
                    var left = line[7..10];
                    var right = line[12..15];
                    map[key] = (left, right);
                }
            }

            return map;
        }

        internal override string GetPartTwoAnswer(IEnumerable<string> input)
        {
            var map = GetMapAndInstructions(input, out var instructions);
            var startingNodes = map.Keys.Where(k => k[2] == 'A');

            long lcm = 1;
            foreach (var node in startingNodes)
            {
                var next = instructions[0] == 'L' ? map[node].Item1 : map[node].Item2;
                var steps = 1;
                var instructionIndex = 1;
                while (next[2] != 'Z')
                {
                    var nextInstruction = instructions[instructionIndex];
                    next = nextInstruction == 'L' ? map[next].Item1 : map[next].Item2;
                    instructionIndex++;
                    if (instructionIndex == instructions.Length)
                    {
                        instructionIndex = 0;
                    }
                    steps++;
                }
                lcm = LeastCommonMultiple(lcm, steps);
            }

            return lcm.ToString();
        }

        private long GreatestCommonFactor(long a, long b)
        {
            while (b != 0)
            {
                long temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        private long LeastCommonMultiple(long a, long b)
        {
            return a / GreatestCommonFactor(a, b) * b;
        }
    }
}
