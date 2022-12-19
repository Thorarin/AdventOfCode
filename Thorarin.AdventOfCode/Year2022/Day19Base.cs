using System.Text.RegularExpressions;
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

    protected State InitializeState(Blueprint blueprint)
    {
        return new State(
            blueprint,
            new ResourceState(0, 1, new[] { blueprint.ClayRobot.Ore, blueprint.ObsidianRobot.Ore, blueprint.GeodeRobot.Ore }.Max()),
            new ResourceState(0, 0, blueprint.ObsidianRobot.Clay),
            new ResourceState(0, 0, blueprint.GeodeRobot.Obsidian),
            new ResourceState(0, 0, int.MaxValue));
    }

    protected State Simulate(State state, int remaining, Dictionary<(State, int), State> cache, ref int overallBest)
    {
        if (cache.TryGetValue((state, remaining), out var cachedState))
        {
            return cachedState;
        }

        if (remaining == 0)
        {
            if (state.Geode.Current > overallBest)
            {
                overallBest = state.Geode.Current;
                RunContext.SetStatusAsync($"New best for {state.Blueprint.Number}: {overallBest}");
            }

            return state;
        }

        State best = state;

        foreach (var move in Moves(state, remaining))
        {
            if (state.CalcBestPossible(remaining) > overallBest)
            {
                var next = Simulate(move, remaining - 1, cache, ref overallBest);
                if (next.Geode.Current > best.Geode.Current)
                {
                    best = next;
                }
            }
        }

        cache[(state, remaining)] = best;

        return best;
    }

    private static IEnumerable<State> Moves(State state, int remaining)
    {
        if (state.CanBuild(Robot.Geode, remaining))
        {
            yield return state.Build(Robot.Geode);
            yield break;
        }

        int neededObsidian = state.Blueprint.ObsidianRobot.Obsidian - state.Obsidian.Current;

        if (state.Obsidian.Robots < state.Obsidian.Max && remaining > neededObsidian)
        {
            if (state.CanBuild(Robot.Obsidian, remaining))
                yield return state.Build(Robot.Obsidian);

            if (MathEx.Termial(remaining) * state.Blueprint.GeodeRobot.Obsidian > state.Blueprint.ObsidianRobot.Clay)
            {
                if (state.CanBuild(Robot.Clay, remaining))
                    yield return state.Build(Robot.Clay);
            }
        }

        bool needOre = state.Obsidian.Robots < state.Obsidian.Max ||
                       state.Ore.Robots < state.Blueprint.GeodeRobot.Ore;

        bool willBreakEven = remaining > state.Blueprint.OreRobot.Ore;

        if (needOre && willBreakEven)
        {
            if (state.CanBuild(Robot.Ore, remaining))
            {
                yield return state.Build(Robot.Ore);
                yield break;
            }
        }

        yield return state.Wait();
    }

    protected readonly record struct State(Blueprint Blueprint, ResourceState Ore, ResourceState Clay, ResourceState Obsidian, ResourceState Geode)
    {
        public bool CanBuild(Robot robot, int remaining)
        {
            var resource = GetResource(robot);
            if (resource.Robots >= resource.Max) return false;

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
                Geode with { Current = Geode.Current + Geode.Robots });
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
                });
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
            return Geode.Current + (remaining * Geode.Robots) + MathEx.Termial(remaining);
        }
    }

    protected record ResourceState(int Current, int Robots, int Max);

    protected record Blueprint(int Number, Cost OreRobot, Cost ClayRobot, Cost ObsidianRobot, Cost GeodeRobot)
    {
    }

    protected readonly record struct Cost(int Ore, int Clay, int Obsidian)
    {
    }
    protected enum Robot
    {
        Ore = 0,
        Clay = 1,
        Obsidian = 2,
        Geode = 3
    }

    protected class Answer : IOutput
    {
        public Answer(long Value, IList<int> Intermediate)
        {
            this.Value = Value;
            this.Intermediate = Intermediate;
        }

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

        public long Value { get; }
        public IList<int> Intermediate { get; }
    }
}