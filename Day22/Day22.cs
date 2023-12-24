namespace AdventOfCode2023.Day22
{
    internal class Day22 : Day
    {
        internal override string GetPartOneAnswer(IEnumerable<string> input)
        {
            var (numCanDisintegrate, _) = Solve(input);
            return numCanDisintegrate.ToString();
        }

        private (int, int) Solve(IEnumerable<string> input)
        {
            var map = new ((int, int, int), (int, int, int))[309][][];
            for (var i = 0; i < map.Length; i++)
            {
                map[i] = new ((int, int, int), (int, int, int))[10][];
                for (var j = 0; j < map[i].Length; j++)
                {
                    map[i][j] = new ((int, int, int), (int, int, int))[10];
                }
            }

            foreach (var line in input)
            {
                var split = line.Split('~');
                var firstEnd = split[0].Split(',').Select(int.Parse).ToArray();
                var otherEnd = split[1].Split(',').Select(int.Parse).ToArray();
                var end1 = (firstEnd[0], firstEnd[1], firstEnd[2]);
                var end2 = (otherEnd[0], otherEnd[1], otherEnd[2]);
                var brick = (end1, end2);
                WriteToMap(map, brick, brick);
            }

            CollapseMapAndReturnNumFalls(map);

            var bricks = Flatten(map).Distinct().Where(x => x != default);

            var numCanDisintegrate = 0;
            var numFalls = 0;
            foreach (var brick in bricks)
            {
                var mapWithBrickRemoved = map.Select(plane => plane.Select(row => row.Select(space => space == brick ? default : space).ToArray()).ToArray()).ToArray();
                var mapWithBrickRemovedFlattened = new List<((int, int, int), (int, int, int))>(Flatten(mapWithBrickRemoved));
                numFalls += CollapseMapAndReturnNumFalls(mapWithBrickRemoved);
                var mapWithBrickRemovedAfterFallFlattened = Flatten(mapWithBrickRemoved);
                if (mapWithBrickRemovedFlattened.SequenceEqual(mapWithBrickRemovedAfterFallFlattened))
                {
                    numCanDisintegrate++;
                }
            }
            return (numCanDisintegrate, numFalls);
        }
        private IEnumerable<((int, int, int), (int, int, int))> Flatten(((int, int, int), (int, int, int))[][][] map) => map.SelectMany(x => x).SelectMany(x => x);

        private void WriteToMap(((int, int, int), (int, int, int))[][][] map,
            ((int x, int y, int z) end1, (int x, int y, int z) end2) range,
            ((int, int, int), (int, int,int)) brick)
        {
            var minX = Math.Min(range.end1.x, range.end2.x);
            var maxX = Math.Max(range.end1.x, range.end2.x);
            var minY = Math.Min(range.end1.y, range.end2.y);
            var maxY = Math.Max(range.end1.y, range.end2.y);
            var minZ = Math.Min(range.end1.z, range.end2.z);
            var maxZ = Math.Max(range.end1.z, range.end2.z);
            map[minZ][minY][minX] = brick;
            for (int i = minX + 1; i <= maxX; i++)
            {
                map[minZ][minY][i] = brick;
            }

            for (int i = minY + 1; i <= maxY; i++)
            {
                map[minZ][i][minX] = brick;
            }

            for (int i = minZ + 1; i <= maxZ; i++)
            {
                map[i][minY][minX] = brick;
            }
        }

        private int CollapseMapAndReturnNumFalls(((int x, int y, int z) end1, (int x, int y, int z) end2)[][][] map)
        {
            HashSet<((int, int, int), (int, int, int))> attemptedBricks = new HashSet<((int, int, int), (int, int, int))>();
            var numFalls = 0;
            for (var z = 2; z < map.Length; z++)
            {
                for (var y = 0; y < map[z].Length; y++)
                {
                    for (var x = 0; x < map[z][y].Length; x++)
                    {
                        var brick = map[z][y][x];
                        if (!attemptedBricks.Contains(brick))
                        {
                            List<int> zList = new List<int>();
                            var minZ = Math.Min(brick.end1.z, brick.end2.z);
                            var minY = Math.Min(brick.end1.y, brick.end2.y);
                            var maxY = Math.Max(brick.end1.y, brick.end2.y);
                            var minX = Math.Min(brick.end1.x, brick.end2.x);
                            var maxX = Math.Max(brick.end1.x, brick.end2.x);

                            var i = minX;
                            var j = minY;
                            while (j <= maxY)
                            {
                                var height = GetFallHeight(map, i, j, minZ);
                                zList.Add(height);
                                j++;
                            }
                            i++;
                            j--;
                            while (i <= maxX)
                            {
                                var height = GetFallHeight(map, i, j, minZ);
                                zList.Add(height);
                                i++;
                            }

                            var highestZ = zList.Max();
                            if (highestZ < minZ)
                            {
                                var fallHeight = minZ - highestZ;
                                WriteToMap(map, brick, default);
                                brick.end1.z -= fallHeight;
                                brick.end2.z -= fallHeight;
                                WriteToMap(map, brick, brick);
                                numFalls++;
                            }
                            attemptedBricks.Add(brick);
                        }
                    }
                }
            }
            return numFalls;
        }

        private int GetFallHeight(((int x, int y, int z) end1, (int x, int y, int z) end2)[][][] map, int i, int j, int k)
        {
            while (k > 1)
            {
                var zBelow = k - 1;
                var brickBelow = map[zBelow][j][i];
                if (brickBelow.end1.z > 0)
                {
                    break;
                }
                k--;
            }
            return k;
        }

        internal override string GetPartTwoAnswer(IEnumerable<string> input)
        {
            var (_, numFalls) = Solve(input);
            return numFalls.ToString();
        }
    }
}
