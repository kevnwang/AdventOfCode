namespace AdventOfCode2023
{
    internal abstract class Day
    {
        internal void PrintAnswers()
        {
            var day = GetType().Name;
            Console.WriteLine("Day " + day.Substring(3));
            Console.WriteLine("Part 1: " + GetPartOneAnswer(File.ReadLines($"../../../{day}/input.txt")));
            Console.WriteLine("Part 2: " + GetPartTwoAnswer(File.ReadLines($"../../../{day}/input.txt")));
            Console.WriteLine();
        }

        internal abstract string GetPartOneAnswer(IEnumerable<string> input);

        internal abstract string GetPartTwoAnswer(IEnumerable<string> input);
    }
}
