namespace Thorarin.AdventOfCode.Framework
{
    public record StringOutput(string Value) : Output
    {
        protected override string StringValue => Value;

        public static implicit operator StringOutput(string value)
        {
            return new StringOutput(value);
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
