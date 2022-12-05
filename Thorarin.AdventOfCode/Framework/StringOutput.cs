namespace Thorarin.AdventOfCode.Framework
{
    public record StringOutput(string Value) : Output
    {
        protected override string StringValue => Value;

        public override string ToString()
        {
            return Value;
        }
    }
}
