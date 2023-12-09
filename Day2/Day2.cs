namespace AdventOfCode2023.Day2
{
    internal class Day2 : Day
    {
        internal override string GetPartOneAnswer(IEnumerable<string> input)
        {
            var sum = 0;
            foreach (var line in input)
            {
                var split = line.Split(':');
                var gameId = int.Parse(split[0].Split()[1]);

                var splitSets = split[1].Trim().Split(';');
                foreach (var set in splitSets)
                {
                    var reveal = set.Trim().Split(',');
                    foreach (var cubes in reveal)
                    {
                        var splitCubes = cubes.Trim().Split();
                        var num = int.Parse(splitCubes[0]);
                        var color = splitCubes[1];

                        if (color == "red" && num > 12 || color == "green" && num > 13 || color == "blue" && num > 14)
                        {
                            goto endOfLoop;
                        }
                    }
                }

                sum += gameId;

            endOfLoop:
                continue;
            }

            return sum.ToString();
        }

        internal override string GetPartTwoAnswer(IEnumerable<string> input)
        {
            var sum = 0;
            foreach (var line in input)
            {
                var maxRed = 0;
                var maxGreen = 0;
                var maxBlue = 0;

                var split = line.Split(':');

                var splitSets = split[1].Trim().Split(';');
                foreach (var set in splitSets)
                {
                    var reveal = set.Trim().Split(',');
                    foreach (var cubes in reveal)
                    {
                        var splitCubes = cubes.Trim().Split();
                        var num = int.Parse(splitCubes[0]);
                        var color = splitCubes[1];

                        if (color == "red" && num > maxRed)
                        {
                            maxRed = num;
                        }
                        else if (color == "green" && num > maxGreen)
                        {
                            maxGreen = num;
                        }
                        else if (color == "blue" && num > maxBlue)
                        {
                            maxBlue = num;
                        }
                    }
                }

                sum += maxRed * maxGreen * maxBlue;
            }

            return sum.ToString();
        }
    }
}
