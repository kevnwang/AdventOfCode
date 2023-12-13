using System.Collections.Immutable;

namespace AdventOfCode2023.Day07
{
    internal class Day07 : Day
    {
        internal override string GetPartOneAnswer(IEnumerable<string> input)
        {
            var hands = input.ToImmutableSortedDictionary(line => line.Split()[0], line => int.Parse(line.Split()[1]), new PokerHandComparer());
            int sum = 0;
            int count = 1;
            foreach (var (_, value) in hands)
            {
                sum += value * count;
                count++;
            }
            return sum.ToString();
        }

        private class PokerHandComparer : IComparer<string>
        {
            private bool _withJokers;

            public PokerHandComparer()
            {
            }

            public PokerHandComparer(bool withJokers)
            {
                _withJokers = withJokers;
            }

            public int Compare(string? x, string? y)
            {
                var xMatches = new SortedDictionary<char, int>();
                var xJokers = 0;
                var yMatches = new SortedDictionary<char, int>();
                var yJokers = 0;
                for (int i = 0; i < 5; i++)
                {
                    if (_withJokers && x[i] == 'J')
                    {
                        xJokers++;
                    }
                    else
                    {
                        xMatches[x[i]] = xMatches.ContainsKey(x[i]) ? xMatches[x[i]] + 1 : 1;
                    }

                    if (_withJokers && y[i] == 'J')
                    {
                        yJokers++;
                    }
                    else
                    {
                        yMatches[y[i]] = yMatches.ContainsKey(y[i]) ? yMatches[y[i]] + 1 : 1;
                    }
                }
                var xMax = xMatches.Values.DefaultIfEmpty().Max() + xJokers;
                var yMax = yMatches.Values.DefaultIfEmpty().Max() + yJokers;

                if (xMax > 3 || yMax > 3) // five of a kind, quads
                {
                    var compare = xMax.CompareTo(yMax);
                    if (compare != 0)
                    {
                        return compare;
                    }
                    return CompareHandStrengths(x, y);
                }
                else if (xMax == 3 && xMatches.Count == 2 && yMax == 3 && yMatches.Count == 2) // both boats
                {
                    return CompareHandStrengths(x, y);
                }
                else if (xMax == 3 && xMatches.Count == 2) // x boat
                {
                    return 1;
                }
                else if (yMax == 3 && yMatches.Count == 2) // y boat
                {
                    return -1;
                }
                else if (xMax == 3 && yMax == 3) // both trips
                {
                    return CompareHandStrengths(x, y);
                }
                else if (xMax == 3 || yMax == 3) // either trips
                {
                    return xMax.CompareTo(yMax);
                }
                else if (xMax == 2 && xMatches.Count == 3 && yMax == 2 && yMatches.Count == 3) // both two pairs
                {
                    return CompareHandStrengths(x, y);
                }
                else if (xMax == 2 && xMatches.Count == 3) // x two pair    
                {
                    return 1;
                }
                else if (yMax == 2 && yMatches.Count == 3) // y two pair
                {
                    return -1;
                }
                else if (xMax == 2 && xMatches.Count == 4 && yMax == 2 && yMatches.Count == 4) // both pair
                {
                    return CompareHandStrengths(x, y);
                }
                else if (xMax == 2 || yMax == 2) // either pair
                {
                    return xMax.CompareTo(yMax);
                }
                else // high card
                {
                    return CompareHandStrengths(x, y);
                }
            }

            public int CompareHandStrengths(string x, string y)
            {
                for (var i = 0; i < 5; i++)
                {
                    var xValue = GetCardValue(x[i]);
                    var yValue = GetCardValue(y[i]);
                    var compare = xValue.CompareTo(yValue);
                    if (compare != 0)
                    {
                        return compare;
                    }
                }
                return 0;
            }

            private int GetCardValue(char card)
            {
                return card switch
                {
                    'A' => 14,
                    'K' => 13,
                    'Q' => 12,
                    'J' => _withJokers ? 1 : 11,
                    'T' => 10,
                    _ => int.Parse(card.ToString()),
                };
            }
        }

        internal override string GetPartTwoAnswer(IEnumerable<string> input)
        {
            var hands = input.ToImmutableSortedDictionary(line => line.Split()[0], line => int.Parse(line.Split()[1]), new PokerHandComparer(withJokers: true));
            int sum = 0;
            int count = 1;
            foreach (var (_, value) in hands)
            {
                sum += value * count;
                count++;
            }
            return sum.ToString();
        }
    }
}
