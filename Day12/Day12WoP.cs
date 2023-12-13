namespace AdventOfCode2023.Day12
{
    internal class Day12WoP
    {
        internal string GetPartOneAnswer(IEnumerable<string> input)
        {
            var sum = 0;
            foreach (var line in input)
            {
                var lineSplit = line.Split();
                var springs = lineSplit[0].ToCharArray();
                var groups = lineSplit[1].Split(',').Select(int.Parse).ToArray();
                sum += GetNumArrangements(springs, groups);
            }
            return sum.ToString();
        }

        public int GetNumArrangements(char[] springs, int[] groups)
        {
            if (groups.Length == 0)
            {
                return !springs.Any(s => s == '#') ? 1 : 0;
            }
            Console.WriteLine($"{new string(springs)} {string.Concat(groups.Select(g => g.ToString() + ","))}");

            var groupSize = groups[0];
            var group = new Queue<char>();
            var foundWall = false;
            for (var i = 0; i < springs.Length; i++)
            {
                if (springs[i] == '?')
                {
                    group.Enqueue(springs[i]);
                }
                else if (springs[i] == '#')
                {
                    foundWall = true;
                    group.Enqueue(springs[i]);
                }
                else if (!foundWall)
                {
                    group.Clear();
                }
                else
                {
                    return 0;
                }

                if (group.Count() == groupSize)
                {
                    if (i < springs.Length - 1 && springs[i + 1] == '#')
                    {
                        if (foundWall)
                        {
                            var first = group.Dequeue();
                            if (first != '?')
                            {
                                return 0;
                            }
                        }
                        else
                        {
                            group.Clear();
                        }
                    }
                    else
                    {
                        var numArrangements = 0;
                        numArrangements += GetNumArrangements(springs.Skip(i + 2).ToArray(), groups.Skip(1).ToArray());
                        if (!foundWall)
                        {
                            numArrangements += GetNumArrangements(springs.Skip(i - groupSize + 2).ToArray(), groups);
                        }
                        return numArrangements;
                    }
                }
            }

            return 0;
        }

        internal string GetPartTwoAnswer(IEnumerable<string> input)
        {
            return "Not implemented yet";
        }
    }
}
