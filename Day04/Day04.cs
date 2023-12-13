namespace AdventOfCode2023.Day04
{
    internal class Day04 : Day
    {
        internal override string GetPartOneAnswer(IEnumerable<string> input)
        {
            var sum = 0;
            foreach (var line in input)
            {
                var split = line.Split('|');
                var winningNumbers = split[0].Split(':')[1].Split().Where(n => !string.IsNullOrEmpty(n));
                var myNumbers = split[1].Split().Where(n => !string.IsNullOrEmpty(n));

                var winningSet = new HashSet<string>(winningNumbers);
                var numWinning = myNumbers.Count(winningSet.Contains);
                sum += numWinning == 0 ? 0 : (int) Math.Pow(2, numWinning - 1);
            }
            return sum.ToString();
        }

        internal override string GetPartTwoAnswer(IEnumerable<string> input)
        {
            var modifiers = Enumerable.Repeat(1, input.Count()).ToArray();
            var inputArr = input.ToArray();
            for (var i = 0; i < inputArr.Count(); i++)
            {
                var line = inputArr[i];
                var split = line.Split('|');
                var winningNumbers = split[0].Split(':')[1].Split().Where(n => !string.IsNullOrEmpty(n));
                var myNumbers = split[1].Split().Where(n => !string.IsNullOrEmpty(n));

                var winningSet = new HashSet<string>(winningNumbers);
                var numWinning = myNumbers.Count(winningSet.Contains);

                foreach (var j in Enumerable.Range(1, numWinning))
                {
                    modifiers[i + j] += modifiers[i];
                }
            }
            return modifiers.Sum().ToString();
        }
    }
}
