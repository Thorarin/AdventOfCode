namespace Thorarin.AdventOfCode.Framework;

public static class TimeSpanExtensions
{
    public static TimeSpanFormat GetHumanReadableFormat(this TimeSpan timeSpan)
    {
        if (timeSpan.TotalSeconds >= 1)
        {
            return TimeSpanFormat.Seconds;
        }

        if (timeSpan.TotalMilliseconds > 10)
        {
            return TimeSpanFormat.Milliseconds;
        }
        
        if (timeSpan.Ticks > 10)
        {
            return TimeSpanFormat.Microseconds;
        }
        
        return TimeSpanFormat.MicrosecondsSmall;
    } 
    
    public static string FormatHumanReadable(this TimeSpan timeSpan, TimeSpanFormat format)
    {
        switch (format)
        {
            case TimeSpanFormat.Seconds:
                return $"{timeSpan.TotalSeconds:0.00} s";
            case TimeSpanFormat.Milliseconds:
                return $"{timeSpan.TotalMilliseconds:0} ms";
            case TimeSpanFormat.MillisecondsSmall:
                return $"{timeSpan.TotalMilliseconds:0.00} ms";
            case TimeSpanFormat.Microseconds:
                return $"{(timeSpan.Ticks/10):0} µs";
            case TimeSpanFormat.MicrosecondsSmall:
                return $"{(timeSpan.Ticks/10):0.0} µs";
            default:
                throw new ArgumentOutOfRangeException(nameof(format));
        }
    }
}