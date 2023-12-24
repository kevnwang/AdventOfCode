namespace AdventOfCode2023.Day24
{
    internal class Day24 : Day
    {
        internal override string GetPartOneAnswer(IEnumerable<string> input)
        {
            var stones = ParseStones(input);
            var count = CountIntersections(stones, out var _);
            return count.ToString();
        }

        private IEnumerable<((long x, long y, long z) position, (int x, int y, int z) velocity)> ParseStones(IEnumerable<string> input)
        {
            var stones = new List<((long x, long y, long z) position, (int x, int y, int z) velocity)>();
            foreach (var line in input)
            {
                var split = line.Split(" @ ");
                var positions = split[0].Split(", ").Select(long.Parse).ToArray();
                var velocities = split[1].Split(", ").Select(int.Parse).ToArray();
                stones.Add(((positions[0], positions[1], positions[2]), (velocities[0], velocities[1], velocities[2])));
            }
            return stones;
        }

        private int CountIntersections(IEnumerable<((long x, long y, long z) position, (int x, int y, int z) velocity)> stones,
            out List<(double, double)> intersections,
            (int x, int y) modifier = default,
            bool shortCircuit = false,
            bool checkTestArea = true)
        {
            var count = 0;
            intersections = new List<(double, double)>();
            for (var i = 0; i < stones.Count(); i++)
            {
                var stone1 = stones.ElementAt(i);
                stone1.velocity.x -= modifier.x;
                stone1.velocity.y -= modifier.y;

                var (a1, b1, c1) = GetConstants(stone1);

                for (var j = i + 1; j < stones.Count(); j++)
                {
                    var stone2 = stones.ElementAt(j);
                    stone2.velocity.x -= modifier.x;
                    stone2.velocity.y -= modifier.y;

                    var (a2, b2, c2) = GetConstants(stone2);
                    // a1x + b1y + c1 = 0 
                    // a2x + b2y + c2 = 0
                    // (i, y) = ((b1×c2 − b2×c1)/(a1×b2 − a2×b1), (c1×a2 − c2×a1)/(a1×b2 − a2×b1))
                    var x = (b1 * c2 - b2 * c1) / (a1 * b2 - a2 * b1);
                    var y = (c1 * a2 - c2 * a1) / (a1 * b2 - a2 * b1);
                    var intersection = (x, y);
                    intersections.Add(intersection);
                    if (IntersectsInFuture(stone1, intersection, checkTestArea) && IntersectsInFuture(stone2, intersection, checkTestArea))
                    {
                        count++;
                    }
                    else if (shortCircuit)
                    {
                        return 0;
                    }
                }
            }

            return count;
        }

        private bool IntersectsInFuture(((long, long, long), (int, int, int)) stone, (double, double) intersection, bool checkTestArea = true)
        {
            var ((px, py, _), (vx, vy, _)) = stone;
            var (x, y) = intersection;

            var min = 200000000000000;
            var max = 400000000000000;

            if (checkTestArea && (x < min || x > max || y < min || y > max))
            {
                return false;
            }

            if (double.IsNaN(x) && double.IsNaN(y))
            {
                return true;
            }

            return SameSign(vx, x - px) && SameSign(vy, y - py);
        }

        private bool SameSign(int velocity, double position) => 
            velocity > 0 && position > 0 || velocity < 0 && position < 0 || velocity == 0 && position == 0;

        private (double, double, double) GetConstants(((long, long, long), (int vx, int vy, int vz)) stone)
        {
            var ((px, py, _), (vx, vy, _)) = stone;
            var slope = (double)vy / vx;
            // py = m(px) + b
            var yIntercept = py - slope * px;

            // y = mx + b
            // -mx + y = b
            // -mx + y - b = 0

            var a = -slope;
            var b = 1;
            var c = -yIntercept;

            return (a, b, c);
        }

        internal override string GetPartTwoAnswer(IEnumerable<string> input)
        {
            var stones = ParseStones(input);

            var magnitude = 500;
            var totalCombos = Enumerable.Range(0, stones.Count()).Sum();
            for (var i = -magnitude; i <= magnitude; i++)
            {
                for (var j = -magnitude; j <= magnitude; j++)
                {
                    var count = CountIntersections(stones,
                        out List<(double x, double y)> intersections,
                        modifier: (i, j),
                        shortCircuit: true,
                        checkTestArea: false);

                    if (count == totalCombos)
                    {
                        intersections = intersections
                            .Where(i => !double.IsNaN(i.x) && !double.IsNaN(i.y))
                            .Select(i => (Math.Round(i.x), Math.Round(i.y)))
                            .ToList();

                        var distinct = intersections
                            .Distinct()
                            .Select(distinctI => (distinctI, intersections.Count(i => i == distinctI))).ToList();
                        distinct.Sort((x, y) => y.Item2 - x.Item2);

                        var ((rockX, rockY), _) = distinct.First();
                        var times = new List<double>();

                        foreach (var stone in stones)
                        {
                            var tX = (rockX - stone.position.x) / (stone.velocity.x - i);
                            var tY = (rockY - stone.position.y) / (stone.velocity.y - j);

                            var t = double.IsNaN(tX) ? tY : tX;
                            times.Add(t);
                        }

                        for (var k = -magnitude; k <= magnitude; k++)
                        {
                            var zList = stones.Select((stone, index) => stone.position.z + (stone.velocity.z - k) * times[index]);
                            if (zList.Distinct().Count() == 1)
                            {
                                var total = rockX + rockY + zList.Distinct().Single();
                                return total.ToString();
                            }
                        }
                    }
                }
            }

            throw new Exception("No answer found");
        }
    }
}
