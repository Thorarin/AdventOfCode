using System.Reflection;
using System.Text;

namespace Thorarin.AdventOfCode.Framework;

public record Output(long Value)
{
    public static implicit operator Output(long output)
    {
        return new Output(output);
    }

    public sealed override string ToString()
    {
        if (GetType() == typeof(Output))
        {
            return Value.ToString();    
        }
        
        StringBuilder sb = new();

        sb.Append(Value);

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