using SixLabors.ImageSharp;
using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2021;

[Puzzle(Year = 2021, Day = 18, Part = 1)]
public class Day18A : Puzzle
{
    private Queue<INumber> _numbers;

    public override void ParseInput(TextReader reader)
    {
        _numbers = new();
        foreach (var line in reader.AsLines())
        {
            var lineReader = new StringReader(line);

            _numbers.Enqueue(NumberPair.Parse(lineReader));
        }
    }

    public override Output Run()
    {
        var zeNumber = _numbers.Dequeue();

        while (_numbers.Count > 0)
        {
            zeNumber = zeNumber.Add(_numbers.Dequeue());
            zeNumber.Reduce();
        }

        string final = zeNumber.ToString();

        return 0;
    }


    public interface INumber
    {
        int Magnitude();

        INumber Add(INumber other);

        INumber Reduce();

        NumberPair? Parent { get; set; }

        bool Split();
    }

    public class NumberPair : INumber
    {
        private INumber _left;
        private INumber _right;

        public INumber Left
        {
            get => _left;
            set
            {
                _left = value;
                _left.Parent = this;
            }
        }

        public INumber Right
        {
            get => _right;
            set
            {
                _right = value;
                _right.Parent = this;
            }
        }

        public NumberPair(INumber left, INumber right)
        {
            Left = left;
            Right = right;
        }
        
        public static NumberPair Parse(StringReader reader)
        {
            var start = reader.Read();
            if (start != '[') throw new Exception();

            INumber left;
            
            if (reader.Peek() == '[')
            {
                left = Parse(reader);
                if (reader.Read() != ',') throw new Exception();
            }
            else
            {
                string str = reader.ReadUntil(',')!;
                left = new SingleNumber(int.Parse(str));
            }
            
            INumber right;
            if (reader.Peek() == '[')
            {
                right = Parse(reader);
                if (reader.Read() != ']') throw new Exception();
            }
            else
            {
                string str = reader.ReadUntil(']')!;
                right = new SingleNumber(int.Parse(str));
            }

            return new NumberPair(left, right);
        }
        
        public int Magnitude()
        {
            throw new NotImplementedException();
        }

        public INumber Add(INumber other)
        {
            var number = new NumberPair(this, other);
            return number;
        }

        public INumber Reduce()
        {
            Console.WriteLine("Start:   " + ToString());
            while (true)
            {
                if (Explode())
                {
                    Console.WriteLine("Explode: " + ToString());
                    continue;
                }

                if (Split())
                {
                    Console.WriteLine("Split:   " + ToString());
                    continue;
                }
                    
                break;
            }
            return this;
        }

        public NumberPair? Parent { get; set; }

        public bool Explode(int depth = 0)
        {
            if (depth == 4)
            {
                var numberLeft = FindAdjacentRegularNumber();
                if (numberLeft != null)
                {
                    numberLeft.Number += ((SingleNumber)Left).Number;
                }
                var numberRight = FindRightAdjacentRegularNumber();
                if (numberRight != null)
                {
                    numberRight.Number += ((SingleNumber)Right).Number;
                }

                Console.WriteLine($"Explode {this}, to {numberLeft} and {numberRight}");
                
                var replacement = new SingleNumber(0);
                
                //Parent!.Delete(this);

                if (Parent.Left == this)
                {
                    Parent.Left = replacement;
                }
                else if (Parent.Right == this)
                {
                    Parent.Right = replacement;
                }
                
                return true;
            }
            
            bool exploded = false;
            if (Left is NumberPair l)
            {
                exploded = l.Explode(depth + 1);
            }

            if (!exploded && Right is NumberPair r)
            {
                exploded = r.Explode(depth + 1);
            }

            return exploded;
        }

        public bool Split()
        {
            return Left.Split() || Right.Split();
        }
        
        private SingleNumber? FindAdjacentRegularNumber()
        {
            var current = Parent;
            var previous = this;

            while (current != null && current.Left == previous)
            {
                previous = current;
                current = current.Parent;
            }

            if (current == null) return null;
            if (current.Right is SingleNumber r) return r;
            
            return current.Left switch
            {
                SingleNumber l => l,
                NumberPair p => GetRightMostSingleNumber(p),
                _ => throw new ArgumentOutOfRangeException()
            };            
        }
        
        private SingleNumber? FindRightAdjacentRegularNumber()
        {
            var current = Parent;
            var previous = this;

            while (current != null && current.Right == previous)
            {
                previous = current;
                current = current.Parent;
            }

            if (current == null) return null;
            if (current.Left != previous && Left is SingleNumber r) return r;

            return current.Right switch
            {
                SingleNumber l => l,
                NumberPair p => GetLeftMostSingleNumber(p),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private SingleNumber GetLeftMostSingleNumber(NumberPair number)
        {
            if (number.Left is NumberPair p)
            {
                return GetLeftMostSingleNumber(p);
            }
            return (SingleNumber)number.Left;
        }
        
        private SingleNumber GetRightMostSingleNumber(NumberPair number)
        {
            if (number.Right is NumberPair p)
            {
                return GetLeftMostSingleNumber(p);
            }
            return (SingleNumber)number.Right;
        }        

        bool Delete(INumber number)
        {
            var numberToKeep = Left == number ? Right : Left;
            
            if (Parent.Right == this)
            {
                Parent.Right = numberToKeep;
                numberToKeep.Parent = Parent;
                return true;
            }

            if (Parent.Left == this)
            {
                Parent.Left = numberToKeep;
                numberToKeep.Parent = Parent;
                return true;
            }

            throw new Exception("Error deleting");
        }
        

        public override string ToString()
        {
            return $"[{Left},{Right}]";
        }
    }

    public class SingleNumber : INumber
    {
        public int Number { get; set; }

        public SingleNumber(int number)
        {
            Number = number;
        }
        
        public int Magnitude()
        {
            throw new NotImplementedException();
        }

        public INumber Add(INumber other)
        {
            throw new NotImplementedException();
        }

        public INumber Reduce()
        {
            throw new NotImplementedException();
        }

        public NumberPair? Parent { get; set; }
        public bool Split()
        {
            if (Number < 10) return false;

            var (quotient, remainder) = Math.DivRem(Number, 2);

            var replacement = new NumberPair(new SingleNumber(quotient), new SingleNumber(quotient + remainder));

            if (Parent.Left == this)
            {
                Parent.Left = replacement;
            }
            else if (Parent.Right == this)
            {
                Parent.Right = replacement;
            }

            return true;
        }

        public override string ToString()
        {
            return Number.ToString();
        }
    }
}