namespace AdventOfCode2023.Day9
{
    internal class Day9 : Day
    {
        internal override string GetPartOneAnswer(IEnumerable<string> input)
        {
            return GenerateSequencesAndExtrapolate(input);
        }

        private string GenerateSequencesAndExtrapolate(IEnumerable<string> input, bool backwards = false)
        {
            var sum = 0;
            foreach (var line in input)
            {
                var sequence = line.Split().Select(int.Parse).ToList();
                var sequences = new List<IEnumerable<int>>
                    {
                        sequence
                    };

                while (!sequence.All(x => x == 0))
                {
                    var sequenceArr = sequence.ToArray();
                    sequence = new List<int>();
                    for (var i = 1; i < sequenceArr.Length; i++)
                    {
                        sequence.Add(sequenceArr[i] - sequenceArr[i - 1]);
                    }
                    sequences.Add(sequence);
                }

                var nextVal = 0;
                for (var i = sequences.Count - 2; i >= 0; i--)
                {
                    nextVal = backwards ? sequences[i].First() - nextVal : sequences[i].Last() + nextVal;
                }
                sum += nextVal;
            }
            return sum.ToString();
        }

        internal override string GetPartTwoAnswer(IEnumerable<string> input)
        {
            return GenerateSequencesAndExtrapolate(input, backwards: true);
        }
    }
}
