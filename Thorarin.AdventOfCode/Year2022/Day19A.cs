using System.Text.RegularExpressions;
using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2022;

[Puzzle(Year = 2022, Day = 19, Part = 1)]
public class Day19A : Puzzle
{
    private List<Blueprint> _blueprints = new();

    public override void ParseInput(TextReader reader)
    {
        var regex = new Regex("Blueprint \\d+: Each ore robot costs (\\d+) ore. Each clay robot costs (\\d+) ore. Each obsidian robot costs (\\d+) ore and (\\d+) clay. Each geode robot costs (\\d+) ore and (\\d+) obsidian.");

        foreach (string line in reader.AsLines())
        {
            var match = regex.Match(line);
            if (!match.Success) throw new Exception();

            var oreRobot = new Cost(int.Parse(match.Groups[1].ValueSpan), 0, 0);
            var clayRobot = new Cost(int.Parse(match.Groups[2].ValueSpan), 0, 0);
            var obsidianRobot = new Cost(int.Parse(match.Groups[3].ValueSpan), int.Parse(match.Groups[4].ValueSpan), 0);
            var geodeRobot = new Cost(int.Parse(match.Groups[5].ValueSpan), 0, int.Parse(match.Groups[6].ValueSpan));

            _blueprints.Add(new Blueprint(oreRobot, clayRobot, obsidianRobot, geodeRobot));
        }
    }

    public override Output SampleExpectedOutput => 33;

    //public override Output ProblemExpectedOutput => 3412;

    public override Output Run()
    {
        var state = InitializeState(_blueprints[0]);
        state = state.Wait();
        state = state.Wait();
        state = state.Build(1);
        state = state.Wait();
        state = state.Build(1);


        //var sw = Stopwatch.StartNew();
        //var a = Simulate(_blueprints[0]);
        //Console.WriteLine($"1: {a} in {sw.Elapsed.TotalSeconds}");

        //var b = Simulate(_blueprints[1]);

        var bla = _blueprints
            .AsParallel()
            .Select((bp, index) =>
            {
                int geodes = Simulate(bp);
                Console.WriteLine($"{index + 1}: {geodes}" );
                return geodes * (index + 1);
            })
            .Sum();

        Console.WriteLine(bla);


        return bla;
    }

    private int Simulate(Blueprint blueprint)
    {
        var state = new State(
            blueprint,
            new ResourceState(0, 1, new[] { blueprint.ClayRobot.Ore, blueprint.ObsidianRobot.Ore, blueprint.GeodeRobot.Ore }.Max()),
            new ResourceState(0, 0, blueprint.ObsidianRobot.Clay),
            new ResourceState(0, 0, blueprint.GeodeRobot.Obsidian),
            new ResourceState(0, 0, int.MaxValue));

        var best = Simulate(state, 24);

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

    private State Simulate(State state, int remaining)
    {
        if (remaining == 0)
        {
            return state;
        }

        State best = state;

        foreach (var move in Moves(state))
        {
            var next = Simulate(move, remaining - 1);
            if (next.Geode.Current > best.Geode.Current)
            {
                best = next;
            }
        }

        return best;

        IEnumerable<State> Moves(State state)
        {
            if (state.CanBuild(3))
            {
                yield return state.Build(3);
                yield break;
            }

            if (state.Obsidian.Robots < state.Obsidian.Max)
            {
                if (state.CanBuild(2))
                    yield return state.Build(2);

                if (state.CanBuild(1))
                    yield return state.Build(1);
            }

            if (state.Obsidian.Robots < state.Obsidian.Max || state.Ore.Robots < state.Blueprint.GeodeRobot.Ore)
            {
                if (state.CanBuild(0))
                    yield return state.Build(0);

            }

            yield return state.Wait();
        }
    }


    private record State(Blueprint Blueprint, ResourceState Ore, ResourceState Clay, ResourceState Obsidian, ResourceState Geode)
    {

        public bool CanBuild(int robot)
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

        public State Build(int robot)
        {
            var cost = GetCost(robot);

            return new State(
                Blueprint,
                Ore with { Current = Ore.Current - cost.Ore + Ore.Robots, Robots = Ore.Robots + (robot == 0 ? 1 : 0) },
                Clay with { Current = Clay.Current - cost.Clay + Clay.Robots, Robots = Clay.Robots + (robot == 1 ? 1 : 0) },
                Obsidian with { Current = Obsidian.Current - cost.Obsidian + Obsidian.Robots, Robots = Obsidian.Robots + (robot == 2 ? 1 : 0) },
                Geode with { Current = Geode.Current + Geode.Robots, Robots = Geode.Robots + (robot == 3 ? 1 : 0) });
        }

        private Cost GetCost(int robot)
        {
            return robot switch
            {
                0 => Blueprint.OreRobot,
                1 => Blueprint.ClayRobot,
                2 => Blueprint.ObsidianRobot,
                3 => Blueprint.GeodeRobot,
                _ => throw new ArgumentOutOfRangeException(nameof(robot), robot, null)
            };
        }
        private ResourceState GetResource(int robot)
        {
            return robot switch
            {
                0 => Ore,
                1 => Clay,
                2 => Obsidian,
                3 => Geode,
                _ => throw new ArgumentOutOfRangeException(nameof(robot), robot, null)
            };
        }

    }

    private record ResourceState(int Current, int Robots, int Max);

    private record Blueprint(Cost OreRobot, Cost ClayRobot, Cost ObsidianRobot, Cost GeodeRobot)
    {
    }

    private record struct Cost(int Ore, int Clay, int Obsidian)
    {
    }
}