using System.Text.RegularExpressions;
using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2022;

[Puzzle(Year = 2022, Day = 19, Part = 2)]
public class Day19B : Puzzle
{
    private const int Minutes = 21;

    private List<Blueprint> _blueprints = new();

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

    // 5.92
    // 2.11, 8.11
    public override Output SampleExpectedOutput => 3472;

    public override Output ProblemExpectedOutput => 13475;

    public override Output Run()
    {
        var bla = _blueprints
            .Take(3)
            .AsParallel()
            .Select((bp, index) =>
            {
                int geodes = Simulate(bp);
                Console.WriteLine($"{index + 1}: {geodes}" );
                return geodes;
            })
            .Aggregate((a, b) => a * b);

        Console.WriteLine(bla);

        return bla;
    }

    private int Simulate(Blueprint blueprint)
    {
        int overallBest = 0;

        var state = new State(
            blueprint,
            new ResourceState(0, 1, new[] { blueprint.ClayRobot.Ore, blueprint.ObsidianRobot.Ore, blueprint.GeodeRobot.Ore }.Max()),
            new ResourceState(0, 0, blueprint.ObsidianRobot.Clay),
            new ResourceState(0, 0, blueprint.GeodeRobot.Obsidian),
            new ResourceState(0, 0, int.MaxValue));

        var best = Simulate(state, Minutes, ref overallBest);

        return best.Geode.Current;
    }

    private State InitializeState(Blueprint blueprint)
    {
        return new State(
            blueprint,
            new ResourceState(0, 1, new[] { blueprint.ClayRobot.Ore, blueprint.ObsidianRobot.Ore, blueprint.GeodeRobot.Ore }.Max()),
            new ResourceState(0, 0, blueprint.ObsidianRobot.Clay),
            new ResourceState(0, 0, blueprint.GeodeRobot.Obsidian),
            new ResourceState(0, 0, int.MaxValue));
    }

    private State Simulate(State state, int remaining, ref int overallBest)
    {
        if (remaining == 0)
        {
            if (state.Geode.Current > overallBest)
            {
                overallBest = state.Geode.Current;
                //RunContext.SetStatus($"New best for {state.Blueprint.Number}: {overallBest}");
            }

            return state;
        }

        State best = state;

        foreach (var move in Moves(state, remaining))
        {
            if (state.CalcBestPossible(remaining) > overallBest)
            {
                var next = Simulate(move, remaining - 1, ref overallBest);
                if (next.Geode.Current > best.Geode.Current)
                {
                    best = next;
                }
            }
        }

        return best;

        IEnumerable<State> Moves(State state, int remaining)
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
                    yield return state.Build(Robot.Ore);

            }

            yield return state.Wait();
        }
    }


    private record State(Blueprint Blueprint, ResourceState Ore, ResourceState Clay, ResourceState Obsidian, ResourceState Geode)
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
                Ore with { Current = Ore.Current - cost.Ore + Ore.Robots, Robots = Ore.Robots + (robot == Robot.Ore ? 1 : 0) },
                Clay with { Current = Clay.Current - cost.Clay + Clay.Robots, Robots = Clay.Robots + (robot == Robot.Clay ? 1 : 0) },
                Obsidian with { Current = Obsidian.Current - cost.Obsidian + Obsidian.Robots, Robots = Obsidian.Robots + (robot == Robot.Obsidian ? 1 : 0) },
                Geode with { Current = Geode.Current + Geode.Robots, Robots = Geode.Robots + (robot == Robot.Geode ? 1 : 0) });
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

    private record ResourceState(int Current, int Robots, int Max);

    private record Blueprint(int Number, Cost OreRobot, Cost ClayRobot, Cost ObsidianRobot, Cost GeodeRobot)
    {
    }

    private record struct Cost(int Ore, int Clay, int Obsidian)
    {
    }

    private enum Robot
    {
        Ore = 0,
        Clay = 1,
        Obsidian = 2,
        Geode = 3
    }
}