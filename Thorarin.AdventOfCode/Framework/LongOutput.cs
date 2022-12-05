using System.Reflection;
using System.Text;

namespace Thorarin.AdventOfCode.Framework
{
    public record LongOutput(long Value) : Output
    {
        protected override string StringValue => Value.ToString();

        public sealed override string ToString()
        {
            if (GetType() == typeof(Output))
            {
                return StringValue;
            }

            StringBuilder sb = new();

            sb.Append(StringValue);

            var extraProperties = GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(prop => prop.DeclaringType != typeof(Output))
                .ToList();

            if (extraProperties.Count > 0)
            {
                sb.Append(" (");
                foreach (var prop in extraProperties)
                {
                    sb.Append(prop.Name).Append(" = ").Append(prop.GetValue(this)).Append(", ");
                }
                sb.Length -= 2;
                sb.Append(")");
            }

            return sb.ToString();
        }
    }
}
