using System.Globalization;
using System.Linq.Expressions;
using Thorarin.AdventOfCode.Framework;
using Thorarin.AdventOfCode.Extensions;

namespace Thorarin.AdventOfCode.Year2022
{
    public abstract class Day11Base : Puzzle
    {
        protected List<Monkey> Monkeys { get; } = new();

        public override void ParseInput(TextReader reader)
        {
            while (reader.Peek() >= 0)
            {
                reader.ReadLine();
                var startingItems = reader.ReadLine().Substring(18).Split(',')
                    .Select(x => int.Parse(x, NumberStyles.AllowLeadingWhite)).ToList();
                var operation = reader.ReadLine().Substring(19);
                var test = int.Parse(reader.ReadLine().Split(' ').Last());
                var trueMonkey = int.Parse(reader.ReadLine().Split(' ').Last());
                var falseMonkey = int.Parse(reader.ReadLine().Split(' ').Last());

                if (reader.Peek() >= 0) reader.ReadLine();

                Monkeys.Add(new Monkey
                {
                    Number = Monkeys.Count,
                    Operation = GetOperation(operation),
                    Test = test,
                    TrueMonkey = trueMonkey,
                    FalseMonkey = falseMonkey
                });

                Monkeys[^1].Items.Enqueue(startingItems);
            }
        }

        private Func<int, long> GetOperation(string operation)
        {
            string[] split = operation.Split(' ');

            var input = Expression.Parameter(typeof(int), "old");
            var old = Expression.Convert(input, typeof(long));

            Expression left = split[0] switch
            {
                "old" => old,
                _ => Expression.Constant(long.Parse(split[0]))
            };

            Expression right = split[2] switch
            {
                "old" => old,
                _ => Expression.Constant(long.Parse(split[2]))
            };

            Expression binaryOperation = split[1] switch
            {
                "*" => Expression.Multiply(left, right),
                "+" => Expression.Add(left, right),
                _ => throw new NotImplementedException()
            };

            var lamdba = Expression.Lambda(binaryOperation, input);

            return (Func<int, long>)lamdba.Compile();
        }

        protected long CalculateMonkeyBusinessLevel()
        {
            return Monkeys
                .Select(m => m.Inspections)
                .OrderDescending()
                .Take(2)
                .Aggregate((a, b) => a * b);
        }

        protected void DoMonkeyBusinessRounds(int rounds, Func<long, int> reduceWorryLevels)
        {
            for (int round = 1; round <= rounds; round++)
            {
                foreach (var monkey in Monkeys)
                {
                    DoMonkeyBusiness(monkey, reduceWorryLevels);
                }
            }
        }

        private void DoMonkeyBusiness(Monkey monkey, Func<long, int> reduceWorryLevels)
        {
            while (monkey.HasItems)
            {
                int item = reduceWorryLevels(monkey.InspectItem());
                monkey.ThrowItem(item, Monkeys);
            }
        }

        protected class Monkey
        {
            public int Number { get; init; }
            public Queue<int> Items { get; } = new();
            public required Func<int, long> Operation { get; init; }

            public required int Test { get; init; }
            public required int TrueMonkey { get; init; }
            public required int FalseMonkey { get; init; }

            public long Inspections { get; private set; }

            public override string ToString()
            {
                return $"Monkey {Number}: " + string.Join(", ", Items.ToArray());
            }
            public bool HasItems => Items.Count > 0;

            public long InspectItem()
            {
                var item = Items.Dequeue();
                Inspections++;

                return Operation(item);
            }

            public void ThrowItem(int item, IReadOnlyList<Monkey> monkeys)
            {
                int newMonkey = DecideThrowTarget(item);
                monkeys[newMonkey].Items.Enqueue(item);
            }

            public int DecideThrowTarget(int item)
            {
                return item % Test == 0 ? TrueMonkey : FalseMonkey;
            }
        }
    }
}
