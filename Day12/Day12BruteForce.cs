using System.Text.RegularExpressions;

namespace AdventOfCode2023.Day12
{
    internal class Day12BRuteForce
    {
        internal string GetPartOneAnswer(IEnumerable<string> input)
        {
            var sum = 0;
            foreach (var line in input)
            {
                var lineSplit = line.Split();
                var springs = lineSplit[0].ToCharArray();
                var groups = lineSplit[1].Split(',').Select(int.Parse).ToArray();
                var asum = GetNumArrangements(springs, groups);
                Console.WriteLine(asum);
                sum += asum;
            }
            return sum.ToString();
        }

        public int GetNumArrangements(char[] springs, int[] groups)
        {
            if (springs.All(c => c != '?'))
            {
                var r = new Regex(@"#+");
                var matches = r.Matches(new string(springs));
                return matches.Select(m => m.Length).SequenceEqual(groups) ? 1 : 0;
            }

            var index = Array.IndexOf(springs, '?');
            if (index == -1)
            {
                return 0;
            }

            var numArrangements = 0;
            springs[index] = '#';
            numArrangements += GetNumArrangements(springs, groups);
            springs[index] = '.';
            numArrangements += GetNumArrangements(springs, groups);
            springs[index] = '?';
            return numArrangements;
        }

        internal string GetPartTwoAnswer(IEnumerable<string> input)
        {
            return "Not implemented yet";
        }
    }
}
