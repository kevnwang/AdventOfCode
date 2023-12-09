namespace AdventOfCode2023.Day6
{
    internal class Day6 : Day
    {
        internal override string GetPartOneAnswer(IEnumerable<string> input)
        {
            var totalWays = 1;
            var times = input.First().Split(':')[1].Split().Where(n => !string.IsNullOrWhiteSpace(n)).Select(int.Parse).ToList();
            var distances = input.ElementAt(1).Split(':')[1].Split().Where(n => !string.IsNullOrWhiteSpace(n)).Select(int.Parse).ToList();

            for (var i = 0; i < times.Count; i++)
            {
                var time = times[i];
                var distance = distances[i];

                var ways = 0;
                for (var j = 1; j < time; j++)
                {
                    if (j * (time - j) > distance)
                    {
                        ways++;
                    }
                }

                totalWays *= ways;
            }
            return totalWays.ToString();
        }

        internal override string GetPartTwoAnswer(IEnumerable<string> input)
        {
            var time = long.Parse(input.First().Split(':')[1].Replace(" ", ""));
            var distance = long.Parse(input.ElementAt(1).Split(':')[1].Replace(" ", ""));

            for (var j = 1; j < time; j++)
            {
                if (j * (time - j) > distance)
                {
                    return (time - j - j + 1).ToString();
                }
            }

            return "0";
        }
    }
}
