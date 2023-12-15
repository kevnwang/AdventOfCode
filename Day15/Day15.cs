using System.Collections.Specialized;

namespace AdventOfCode2023.Day15
{
    internal class Day15 : Day
    {
        internal override string GetPartOneAnswer(IEnumerable<string> input)
        {
            var line = input.First();
            var sequence = line.Split(',');
            var sum = 0;
            foreach (var s in sequence)
            {
                sum += GetHashCode(s);
            }
            return sum.ToString();
        }

        private int GetHashCode(string s)
        {
            var code = 0;
            for (int i = 0; i < s.Length; i++)
            {
                code += s[i];
                code *= 17;
                code %= 256;
            }
            return code;
        }

        internal override string GetPartTwoAnswer(IEnumerable<string> input)
        {
            var line = input.First();
            var sequence = line.Split(',');
            var boxes = new Dictionary<int, List<(string, int)>>();
            foreach (var s in sequence)
            {
                if (s.Last() == '-')
                {
                    var label = s.Substring(0, s.Length - 1);
                    var box = GetHashCode(label);
                    if (boxes.TryGetValue(box, out var lenses))
                    {
                        lenses.RemoveAll(x => x.Item1 == label);
                    }
                }
                else
                {
                    var split = s.Split('=');
                    var label = split[0];
                    var focalLength = int.Parse(split[1]);
                    var box = GetHashCode(label);
                    if (!boxes.ContainsKey(box))
                    {
                        boxes[box] = new List<(string, int)>();
                    }
                    var indexOf = boxes[box].FindIndex(x => x.Item1 == label);
                    if (indexOf != -1)
                    {
                        boxes[box][indexOf] = (label, focalLength);
                    }
                    else
                    {
                        boxes[box].Add((label, focalLength));
                    }
                }
            }

            var sum = 0;
            foreach (var (box, lenses) in boxes)
            {
                for (int i = 0; i < lenses.Count(); i++)
                {
                    sum += (box + 1) * (i + 1) * lenses[i].Item2;
                }
            }
            return sum.ToString();
        }
    }
}
