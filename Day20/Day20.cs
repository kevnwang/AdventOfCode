namespace AdventOfCode2023.Day20
{
    internal class Day20 : Day
    {
        internal override string GetPartOneAnswer(IEnumerable<string> input)
        {
            return Solve(input);
        }

        private string Solve(IEnumerable<string> input, bool checkRx = false)
        {
            var button = new Button();
            var modules = new Dictionary<string, IModule>
            {
                { "button", button },
            };
            var configuration = new Dictionary<IModule, string[]>
            {
                { button, ["broadcaster"] }
            };
            foreach (var line in input)
            {
                var split = line.Split(" -> ");
                var left = split[0];
                var prefix = left[0];
                var name = left.Substring(1);
                var right = split[1].Split(", ");
                IModule module = default;
                switch (prefix)
                {
                    case 'b':
                        module = new Broadcast();
                        break;
                    case '%':
                        module = new FlipFlop(name);
                        break;
                    case '&':
                        module = new Conjunction(name);
                        break;
                }
                modules[prefix == 'b' ? "broadcaster" : name] = module;
                configuration[module] = right;
            }

            var conjunctions = modules.Where(kvp => kvp.Value is Conjunction);
            foreach (var (name, module) in conjunctions)
            {
                var conjunction = (Conjunction)module;
                var conjunctionInputs = configuration.Where(kvp => kvp.Value.Contains(name)).Select(kvp => kvp.Key).Select(module => module.Name);
                conjunction.InitializeInputs(conjunctionInputs);
            }

            var pulseCounts = new Dictionary<PulseLevel, long>
            {
                { PulseLevel.Low, 0 },
                { PulseLevel.High, 0 },
            };

            var pulses = new Queue<Pulse>();

            var lvModule = (Conjunction)modules["lv"];
            var lvInputNamesCount = lvModule.GetNumInputs();
            var lvInputCounts = new Dictionary<string, int>();
            var lvInputCycles = new Dictionary<string, long>();

            foreach (var i in Enumerable.Range(0, checkRx ? int.MaxValue : 1000))
            {
                pulses.Enqueue(new Pulse(default, "button", PulseLevel.Low));

                while (pulses.TryDequeue(out var pulse))
                {
                    if (checkRx && pulse.destName == "lv" && pulse.level == PulseLevel.High)
                    {
                        if (lvInputCounts.TryGetValue(pulse.sourceName, out var count))
                        {
                            lvInputCycles[pulse.sourceName] = i - count;

                            if (lvInputCycles.Count() == lvInputNamesCount)
                            {
                                return lvInputCycles.Values.Aggregate((x, y) => x * y).ToString();
                            }
                        }

                        lvInputCounts[pulse.sourceName] = i;
                    }

                    var moduleName = pulse.destName;
                    if (modules.TryGetValue(moduleName, out var module))
                    {
                        if (module.TryGetNewPulseLevel(pulse, out var newLevel))
                        {
                            var destNames = configuration[module];
                            foreach (var destName in destNames)
                            {
                                pulseCounts[newLevel]++;
                                var pulseToSend = new Pulse(moduleName, destName, newLevel);
                                pulses.Enqueue(pulseToSend);
                            }
                        }
                    }
                }
            }

            var mult = pulseCounts[PulseLevel.Low] * pulseCounts[PulseLevel.High];
            return mult.ToString();
        }

        private interface IModule
        {
            string Name { get; }

            bool TryGetNewPulseLevel(Pulse pulse, out PulseLevel newLevel);
        }

        private class Button : IModule
        {
            public string Name => "button";

            public bool TryGetNewPulseLevel(Pulse pulse, out PulseLevel newLevel)
            {
                newLevel = pulse.level;
                return true;
            }
        }

        private class Broadcast : IModule
        {
            public string Name => "broadcaster";

            public bool TryGetNewPulseLevel(Pulse pulse, out PulseLevel newLevel)
            {
                newLevel = pulse.level;
                return true;
            }
        }

        private class FlipFlop : IModule
        {
            private bool On { get; set; } = false;

            public FlipFlop(string name)
            {
                Name = name;
            }

            public string Name { get; init; }

            public bool TryGetNewPulseLevel(Pulse pulse, out PulseLevel newLevel)
            {
                newLevel = On ? PulseLevel.Low : PulseLevel.High;
                if (pulse.level == PulseLevel.Low)
                {
                    On = !On;
                    return true;
                }
                return false;
            }
        }

        private class Conjunction : IModule
        {
            private IDictionary<string, PulseLevel> Inputs { get; } = new Dictionary<string, PulseLevel>();

            public Conjunction(string name)
            {
                Name = name;
            }

            public string Name { get; init; }

            public bool TryGetNewPulseLevel(Pulse pulse, out PulseLevel newLevel)
            {
                Inputs[pulse.sourceName] = pulse.level;
                newLevel = Inputs.Values.All(level => level == PulseLevel.High) ? PulseLevel.Low : PulseLevel.High;
                return true;
            }

            internal void InitializeInputs(IEnumerable<string> names)
            {
                foreach (var name in names)
                {
                    Inputs[name] = PulseLevel.Low;
                }
            }

            internal int GetNumInputs()
            {
                return Inputs.Count();
            }
        }

        private record Pulse(string sourceName, string destName, PulseLevel level);

        private enum PulseLevel
        {
            Low,
            High
        }

        internal override string GetPartTwoAnswer(IEnumerable<string> input)
        {
            return Solve(input, checkRx: true);
        }
    }
}
