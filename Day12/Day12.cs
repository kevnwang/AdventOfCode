namespace AdventOfCode2023.Day12
{
    internal class Day12 : Day
    {
        internal override string GetPartOneAnswer(IEnumerable<string> input)
        {
            long sum = 0;
            foreach (var line in input)
            {
                var lineSplit = line.Split();
                var springs = lineSplit[0].ToCharArray();
                var groups = lineSplit[1].Split(',').Select(int.Parse).ToArray();
                var memory = new Dictionary<string, long>();
                sum += GetNumArrangements(springs, groups, memory);
            }
            return sum.ToString();
        }

        public long GetNumArrangements(char[] springs, int[] groups, IDictionary<string, long> memory)
        {
            if (memory.TryGetValue(GenerateKey(springs, groups), out var arrangements))
            {
                return arrangements;
            }

            if (groups.Length == 0)
            {
                return !springs.Any(s => s == '#') ? 1 : 0; // If there are springs left it's not valid
            }

            var groupSize = groups[0];
            var group = new Queue<char>();
            var foundWall = false;
            for (var i = 0; i < springs.Length; i++)
            {
                // count springs
                if (springs[i] == '?')
                {
                    group.Enqueue(springs[i]);
                }
                else if (springs[i] == '#')
                {
                    foundWall = true;
                    group.Enqueue(springs[i]);
                }
                else if (!foundWall) // If we haven't found a wall yet, we can afford to start over
                {
                    group.Clear();
                }
                else // If we have, we're forced to take it, but we're still trying to search for springs to match the group size, so it's not valid
                {
                    return 0;
                }

                if (group.Count() == groupSize)
                {
                    if (i < springs.Length - 1 && springs[i + 1] == '#') // If the next spring is a wall, let's see if we can slide the "window" of springs over to make it work
                    {
                        var first = group.Dequeue(); // Pop the first spring to attempt a window slide. This decreates group.Count() so we can try again on the next iteration
                        if (first == '#')
                        {
                            return 0; // We can't slide over a wall, so the current group of springs is too large, so it's invalid
                        }
                    }
                    else
                    {
                        long numArrangements = 0;
                        numArrangements += GetNumArrangements(springs.Skip(i + 2).ToArray(), groups.Skip(1).ToArray(), memory); // take the group and skip the next spring, since there needs to be spaces in between groups

                        var startIndexOfCurrentGroup = i - groupSize + 1;
                        if (springs[startIndexOfCurrentGroup] == '?')
                        {
                            numArrangements += GetNumArrangements(springs.Skip(startIndexOfCurrentGroup + 1).ToArray(), groups, memory); // don't take the group and pretend the first unknown spring is operational
                        }

                        memory[GenerateKey(springs, groups)] = numArrangements;
                        return numArrangements;
                    }
                }
            }

            return 0;
        }

        private string GenerateKey(char[] springs, int[] groups) => $"{new string(springs)} {string.Join(",", groups)}";

        internal override string GetPartTwoAnswer(IEnumerable<string> input)
        {
            long sum = 0;
            foreach (var line in input)
            {
                var lineSplit = line.Split();
                lineSplit[0] = $"{lineSplit[0]}?{lineSplit[0]}?{lineSplit[0]}?{lineSplit[0]}?{lineSplit[0]}";
                lineSplit[1] = $"{lineSplit[1]},{lineSplit[1]},{lineSplit[1]},{lineSplit[1]},{lineSplit[1]}";
                var springs = lineSplit[0].ToCharArray();
                var groups = lineSplit[1].Split(',').Select(int.Parse).ToArray();
                var memory = new Dictionary<string, long>();
                sum += GetNumArrangements(springs, groups, memory);
            }
            return sum.ToString();
        }
    }
}
