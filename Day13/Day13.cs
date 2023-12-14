namespace AdventOfCode2023.Day13
{
    internal class Day13 : Day
    {
        internal override string GetPartOneAnswer(IEnumerable<string> input)
        {
            var sum = 0;
            var lines = new List<char[]>();
            foreach (var line in input)
            {
                if (line.Length == 0)
                {
                    sum += CalculateReflection(lines.ToArray());
                    lines.Clear();
                }
                else 
                {
                    lines.Add(line.ToCharArray());
                }
            }
            return sum.ToString();
        }

        internal override string GetPartTwoAnswer(IEnumerable<string> input)
        {
            var sum = 0;
            var lines = new List<char[]>();
            foreach (var line in input)
            {
                if (line.Length == 0)
                {
                    sum += CalculateReflection(lines.ToArray(), smudgesAllowed: 1);
                    lines.Clear();
                }
                else
                {
                    lines.Add(line.ToCharArray());
                }
            }
            return sum.ToString();
        }

        private int CalculateReflection(char[][] pattern, int smudgesAllowed = 0)
        {
            for (var i = 0; i < pattern[0].Length - 1; i++)
            {
                var smudges = 0;
                var distanceFromEnd = pattern[0].Length - i - 1;
                for (var j = Math.Max(0, pattern[0].Length - 2 * distanceFromEnd); j <= i; j++)
                {
                    var distanceFromMirror = i - j;
                    smudges += pattern.Count(line => line[j] != line[i + distanceFromMirror + 1]);
                    if (smudges > smudgesAllowed)
                    {
                        goto noMirror;
                    }
                }
                if (smudges == smudgesAllowed)
                {
                    return i + 1;
                }

            noMirror:
                continue;
            }

            for (var i = 0; i < pattern.Length - 1; i++)
            {
                var smudges = 0;
                var distanceFromEnd = pattern.Length - i - 1;
                for (var j = Math.Max(0, pattern.Length - 2 * distanceFromEnd); j <= i; j++)
                {
                    var distanceFromMirror = i - j;
                    var diff = pattern[j].Select((value, k) => pattern[i + distanceFromMirror + 1][k] != value);
                    smudges += diff.Count(d => d == true);
                    if (smudges > smudgesAllowed)
                    {
                        goto noMirror;
                    }
                }

                if (smudges == smudgesAllowed)
                {
                    return 100 * (i + 1);
                }

            noMirror:
                continue;
            }

            throw new Exception("No mirror found");
        }
    }
}
