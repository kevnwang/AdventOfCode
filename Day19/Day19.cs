namespace AdventOfCode2023.Day19
{
    internal class Day19 : Day
    {
        internal override string GetPartOneAnswer(IEnumerable<string> input)
        {
            var workflows = new Dictionary<string, string[]>();
            var start = false;
            var sum = 0;
            foreach (var line in input)
            {
                if (string.IsNullOrEmpty(line))
                {
                    start = true;
                }
                else if (!start)
                {
                    var split = line.Split('{');
                    var name = split[0];
                    var rules = split[1].TrimEnd('}').Split(',');
                    workflows[name] = rules;
                }
                else
                {
                    var trimmed = line.TrimStart('{').TrimEnd('}');
                    var split = trimmed.Split(',');
                    var x = int.Parse(split[0][2..]);
                    var m = int.Parse(split[1][2..]);
                    var a = int.Parse(split[2][2..]);
                    var s = int.Parse(split[3][2..]);
                    var parts = new Dictionary<string, int>
                    {
                        { "x", x },
                        { "m", m },
                        { "a", a },
                        { "s", s },
                    };
                    var next = "in";
                    do
                    {
                        var rules = workflows[next];
                        foreach (var rule in rules)
                        {
                            if (rule == "A" || rule == "R")
                            {
                                next = rule;
                                break;
                            }

                            if (rule.Contains('<'))
                            {
                                var (part, value, toGo) = ParseRule(rule, '<');
                                if (parts[part] < value)
                                {
                                    next = toGo;
                                    break;
                                }
                            }

                            if (rule.Contains('>'))
                            {
                                var (part, value, toGo) = ParseRule(rule, '>');
                                if (parts[part] > value)
                                {
                                    next = toGo;
                                    break;
                                }
                            }

                            next = rule;
                        }
                    } while (next != "A" && next != "R");

                    if (next == "A")
                    {
                        sum += x + m + a + s;
                    }
                }
            }
            return sum.ToString();
        }

        private (string, int, string) ParseRule(string rule, char delim)
        {
            var splitRule = rule.Split(delim);
            var part = splitRule[0];
            var splitValue = splitRule[1].Split(':');
            var value = int.Parse(splitValue[0]);
            var dest = splitValue[1];
            return (part, value, dest);
        }

        internal override string GetPartTwoAnswer(IEnumerable<string> input)
        {
            var workflows = new Dictionary<string, string[]>();
            foreach (var line in input)
            {
                if (string.IsNullOrEmpty(line))
                {
                    var restraints = new Dictionary<string, (int, int)>
                    {
                        { "x", (1, 4000) },
                        { "m", (1, 4000) },
                        { "a", (1, 4000) },
                        { "s", (1, 4000) }
                    };
                    return GetCombos(workflows, "in", restraints).ToString();
                }

                var split = line.Split('{');
                var name = split[0];
                var rules = split[1].TrimEnd('}').Split(',');
                workflows[name] = rules;
            }
            throw new Exception("No answer found");
        }

        private long GetCombos(IDictionary<string, string[]> workflows, string workflow, IDictionary<string, (int min, int max)> restaints)
        {
            if (workflow == "A")
            {
                return restaints.Values.Select(restraint => (long)restraint.max - restraint.min + 1).Aggregate((x, y) => x * y);
            }

            if (workflow == "R")
            {
                return 0;
            }

            var rules = workflows[workflow];
            long combos = 0;
            foreach (var rule in rules)
            {
                if (rule.Contains('<'))
                {
                    var (part, value, dest) = ParseRule(rule, '<');
                    var newRestraints = new Dictionary<string, (int min, int max)>(restaints)
                    {
                        [part] = (restaints[part].min, value - 1)
                    };
                    combos += GetCombos(workflows, dest, newRestraints);

                    restaints[part] = (value, restaints[part].max);
                }
                else if (rule.Contains('>'))
                {
                    var (part, value, dest) = ParseRule(rule, '>');
                    var newRestraints = new Dictionary<string, (int min, int max)>(restaints)
                    {
                        [part] = (value + 1, restaints[part].max)
                    };
                    combos += GetCombos(workflows, dest, newRestraints);

                    restaints[part] = (restaints[part].min, value);
                }
                else
                {
                    combos += GetCombos(workflows, rule, restaints);
                };
            }

            return combos;
        }
    }
}
