namespace AdventOfCode2023.Day05
{
    internal class Day05 : Day
    {
        internal override string GetPartOneAnswer(IEnumerable<string> input)
        {
            var seeds = input.First().Split(':')[1].Trim().Split().Select(long.Parse);
            var maps = BuildMaps(input);

            var minLoc = long.MaxValue;
            foreach (var seed in seeds)
            {
                var loc = Traverse(maps, seed, mapIndex: 0);
                if (loc < minLoc)
                {
                    minLoc = loc;
                }
            }
            return minLoc.ToString();
        }

        private IEnumerable<(long, long, long)>[] BuildMaps(IEnumerable<string> input)
        {
            var inputArr = input.ToArray();
            var maps = new List<(long, long, long)>[7];

            var mapIndex = 0;
            var index = 3;
            maps[mapIndex] = new List<(long, long, long)>();
            while (index < inputArr.Length)
            {
                var line = inputArr[index];
                if (string.IsNullOrEmpty(line))
                {
                    mapIndex++;
                    maps[mapIndex] = new List<(long, long, long)>();
                    index += 2;
                    continue;
                }
                var lineArr = line.Split();
                var lineDest = long.Parse(lineArr[0]);
                var lineSource = long.Parse(lineArr[1]);
                var lineRange = long.Parse(lineArr[2]);
                maps[mapIndex].Add((lineDest, lineSource, lineRange));
                index++;
            }

            return maps;
        }

        private long Traverse(IEnumerable<(long, long, long)>[] maps, long n, int mapIndex)
        {
            if (mapIndex == maps.Length)
            {
                return n;
            }

            var map = maps[mapIndex];
            foreach (var (dest, source, range) in map)
            {
                if (n >= source && n < source + range)
                {
                    n = dest + n - source;
                    break;
                }
            }

            return Traverse(maps, n, mapIndex + 1);
        }

        internal override string GetPartTwoAnswer(IEnumerable<string> input)
        {
            var nums = input.First().Split(':')[1].Trim().Split().Select(long.Parse).ToArray();
            var maps = BuildMaps(input);

            var minLoc = long.MaxValue;
            for (var i = 0; i < nums.Length; i += 2)
            {
                var loc = TraverseSpans(maps, spans: new List<(long, long)> { (nums[i], nums[i + 1]) }, mapIndex: 0);
                if (loc < minLoc)
                {
                    minLoc = loc;
                }
            }
            return minLoc.ToString();
        }

        private long TraverseSpans(IEnumerable<(long, long, long)>[] maps, IEnumerable<(long, long)> spans, int mapIndex)
        {
            if (mapIndex == maps.Length)
            {
                return spans.MinBy(n => n.Item1).Item1;
            }

            var spanArr = spans.ToArray();
            var newSpans = new List<(long, long)>();
            for (var i = 0; i < spanArr.Length; i++)
            {
                var span = spanArr[i];
                var (spanStart, spanLength) = span;
                var map = maps[mapIndex];
                foreach (var (mapDest, mapSource, mapRange) in map)
                {
                    if (spanStart >= mapSource && spanStart < mapSource + mapRange)
                    {
                        var frontPadding = spanStart - mapSource;
                        var newStart = mapDest + frontPadding;
                        var spanAvailable = mapRange - frontPadding;
                        if (spanAvailable >= spanLength)
                        {
                            span = (newStart, spanLength);
                        }
                        else
                        {
                            span = (newStart, spanAvailable);
                            spanArr[i] = (spanStart + spanAvailable, spanLength - spanAvailable);
                            i--;
                        }
                        break;
                    }
                }

                newSpans.Add(span);
            }

            return TraverseSpans(maps, newSpans, mapIndex + 1);
        }
    }
}
