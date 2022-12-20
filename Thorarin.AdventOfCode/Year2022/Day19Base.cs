using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Microsoft.VisualBasic.CompilerServices;
using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2022;

public abstract class Day19Base : Puzzle
{
    protected readonly List<Blueprint> _blueprints = new();

    public override void ParseInput(TextReader reader)
    {
        var regex = new Regex("Blueprint \\d+: Each ore robot costs (\\d+) ore. Each clay robot costs (\\d+) ore. Each obsidian robot costs (\\d+) ore and (\\d+) clay. Each geode robot costs (\\d+) ore and (\\d+) obsidian.");

        int number = 1;
        foreach (string line in reader.AsLines())
        {
            var match = regex.Match(line);
            if (!match.Success) throw new Exception();

            var oreRobot = new Cost(int.Parse(match.Groups[1].ValueSpan), 0, 0);
            var clayRobot = new Cost(int.Parse(match.Groups[2].ValueSpan), 0, 0);
            var obsidianRobot = new Cost(int.Parse(match.Groups[3].ValueSpan), int.Parse(match.Groups[4].ValueSpan), 0);
            var geodeRobot = new Cost(int.Parse(match.Groups[5].ValueSpan), 0, int.Parse(match.Groups[6].ValueSpan));

            _blueprints.Add(new Blueprint(number++, oreRobot, clayRobot, obsidianRobot, geodeRobot));
        }
    }

    protected State InitializeState(Blueprint blueprint, int remaining)
    {
        return new State(
            blueprint,
            new ResourceState(0, 1, new[] { blueprint.ClayRobot.Ore, blueprint.ObsidianRobot.Ore, blueprint.GeodeRobot.Ore }.Max()),
            new ResourceState(0, 0, blueprint.ObsidianRobot.Clay),
            new ResourceState(0, 0, blueprint.GeodeRobot.Obsidian),
            new ResourceState(0, 0, int.MaxValue),
            remaining);
    }

    protected State Simulate(State state, Dictionary<StateKey, State> cache, ref int overallBest)
    {
        if (cache.TryGetValue(state.Key, out var cachedState))
        {
            return cachedState;
        }

        if (state.Remaining == 0)
        {
            if (state.Geode.Current > overallBest)
            {
                overallBest = state.Geode.Current;
                RunContext.SetStatusAsync($"New best for {state.Blueprint.Number}: {overallBest}");
            }

            return state;
        }

        State best = state;

        foreach (var move in Moves(state))
        {
            if (state.CalcBestPossible(state.Remaining) > overallBest)
            {
                var next = Simulate(move, cache, ref overallBest);
                if (next.Geode.Current > best.Geode.Current)
                {
                    best = next;
                }
            }
        }

        cache[state.Key] = best;

        return best;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    private static IEnumerable<State> Moves(State state)
    {
        // T -1:
        // In the last minute, building new robots is useless, as they won't
        // have harvested any resources at T.
        //
        // T -2:
        // Only building geode robots is useful, as the other robots only
        // facilitate building geode robots.
        //
        // T -3:
        // Same as T -2, as robots need another minute to yield resources.
        //
        // T -4:
        // Obsidian and ore robots can also be built, since they may lead to
        // a geode robot getting build on T -2.
        //
        // T -5:
        // Same as T -4, as robots need another minute to yield resources.
        // 
        // T -6:
        // Clay robots can also be built, so they have harvested clay by the start
        // of T -4, which is the last minute in which construction of an obsidian
        // robot can meaningfully start.


        // There are supposedly edge cases where this rule would not apply,
        // but it's not a problem for my input or any others, as far as I know.
        if (state.Remaining >= 2 && state.CanBuild(Robot.Geode))
        {
            yield return state.Build(Robot.Geode);
            yield break;
        }

        if (state.Remaining >= 4)
        {
            if (state.CanBuild(Robot.Obsidian))
                yield return state.Build(Robot.Obsidian);

            if (state.Remaining >= 6)
            {
                if (state.CanBuild(Robot.Clay))
                    yield return state.Build(Robot.Clay);
            }

            if (state.CanBuild(Robot.Ore))
                yield return state.Build(Robot.Ore);
        }

        yield return state.Wait();
    }

    protected readonly record struct StateKey(int Ore, int OreRobots, int Clay, int ClayRobots, int Obsidian, int ObsidianRobots, int Remaining);

    protected record State(Blueprint Blueprint, ResourceState Ore, ResourceState Clay, ResourceState Obsidian, ResourceState Geode, int Remaining)
    {
        public int BuildStop { get; set; }

        public State(Blueprint blueprint, ResourceState ore, ResourceState clay, ResourceState obsidian, ResourceState geode, int remaining, int buildStop) 
            : this(blueprint, ore, clay, obsidian, geode, remaining)
        {
            BuildStop = buildStop;
        }

        public bool CanBuild(Robot robot)
        {
            if ((BuildStop & (int)robot) != 0) return false;

            var resource = GetResource(robot);
            if (resource.Robots >= resource.MaxRobots) return false;

            // If we have enough resources of this type to build any robot type for the remaining minutes, don't build more robots.
            // Credit: https://www.reddit.com/r/adventofcode/comments/zpy5rm/2022_day_19_what_are_your_insights_and/
            // Improvement: Robots built in the last minute are useless, so use minutes remaining minus one.
            if (robot != Robot.Geode && resource.Robots * (Remaining - 1) + resource.Current >= (Remaining - 1) * Blueprint.Max[robot])
            {
                BuildStop |= (int)robot;
                return false;
            }

            var cost = GetCost(robot);
            return cost.Ore <= Ore.Current && cost.Clay <= Clay.Current && cost.Obsidian <= Obsidian.Current;
        }

        public State Wait()
        {
            return new State(
                Blueprint,
                Ore with { Current = Ore.Current + Ore.Robots },
                Clay with { Current = Clay.Current + Clay.Robots },
                Obsidian with { Current = Obsidian.Current + Obsidian.Robots },
                Geode with { Current = Geode.Current + Geode.Robots },
                Remaining - 1,
                BuildStop);
        }

        public State Build(Robot robot)
        {
            var cost = GetCost(robot);

            return new State(
                Blueprint,
                Ore with
                {
                    Current = Ore.Current - cost.Ore + Ore.Robots,
                    Robots = Ore.Robots + (robot == Robot.Ore ? 1 : 0)
                },
                Clay with
                {
                    Current = Clay.Current - cost.Clay + Clay.Robots,
                    Robots = Clay.Robots + (robot == Robot.Clay ? 1 : 0)
                },
                Obsidian with
                {
                    Current = Obsidian.Current - cost.Obsidian + Obsidian.Robots,
                    Robots = Obsidian.Robots + (robot == Robot.Obsidian ? 1 : 0)
                },
                Geode with
                {
                    Current = Geode.Current + Geode.Robots,
                    Robots = Geode.Robots + (robot == Robot.Geode ? 1 : 0)
                },
                Remaining - 1,
                BuildStop);
        }

        private Cost GetCost(Robot robot)
        {
            return robot switch
            {
                Robot.Ore => Blueprint.OreRobot,
                Robot.Clay => Blueprint.ClayRobot,
                Robot.Obsidian => Blueprint.ObsidianRobot,
                Robot.Geode => Blueprint.GeodeRobot,
                _ => throw new ArgumentOutOfRangeException(nameof(robot), robot, null)
            };
        }
        private ResourceState GetResource(Robot robot)
        {
            return robot switch
            {
                Robot.Ore => Ore,
                Robot.Clay => Clay,
                Robot.Obsidian => Obsidian,
                Robot.Geode => Geode,
                _ => throw new ArgumentOutOfRangeException(nameof(robot), robot, null)
            };
        }

        public int CalcBestPossible(int remaining)
        {
            return Geode.Current + (remaining * Geode.Robots) + MathEx.Termial(remaining - 1);
        }

        public StateKey Key => new(Ore.Current, Ore.Robots, Clay.Current, Clay.Robots, Obsidian.Current, Obsidian.Robots, Remaining);
    }

    protected readonly record struct ResourceState(int Current, int Robots, int MaxRobots);

    protected record Blueprint(int Number, Cost OreRobot, Cost ClayRobot, Cost ObsidianRobot, Cost GeodeRobot)
    {
        private Cost? _max;

        public Cost Max
        {
            get
            {
                if (_max == null)
                {
                    _max = new Cost(
                        MathEx.Max(OreRobot.Ore, ClayRobot.Ore, ObsidianRobot.Ore, GeodeRobot.Ore),
                        MathEx.Max(OreRobot.Clay, ClayRobot.Clay, ObsidianRobot.Clay, GeodeRobot.Clay),
                        MathEx.Max(OreRobot.Obsidian, ClayRobot.Obsidian, ObsidianRobot.Obsidian, GeodeRobot.Obsidian));
                }

                return _max;
            }
        }
    };

    protected record Cost(int Ore, int Clay, int Obsidian)
    {
        public int this[Robot index] => index switch
        {
            Robot.Ore => Ore,
            Robot.Clay => Clay,
            Robot.Obsidian => Obsidian,
            _ => throw new ArgumentOutOfRangeException(nameof(index), index, null)
        };
    }

    protected enum Robot
    {
        Ore = 1,
        Clay = 2,
        Obsidian = 4,
        Geode = 8
    }

    protected class Answer : IOutput
    {
        public Answer(long value, IList<int> intermediate)
        {
            Value = value;
            Intermediate = intermediate;
        }
        public long Value { get; }
        public IList<int> Intermediate { get; }

        protected bool Equals(Answer other)
        {
            return Value == other.Value && Intermediate.SequenceEqual(other.Intermediate);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Answer)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Value, Intermediate);
        }

        public override string ToString()
        {
            return $"{Value} ({string.Join(", ", Intermediate)})";
        }
    }
}