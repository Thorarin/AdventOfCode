namespace Thorarin.AdventOfCode.Framework;

public record RunResult(IOutput Output, TimeSpan ParseDuration, TimeSpan RunDuration, IOutput? Expected)
{
    public TimeSpan TotalDuration => ParseDuration + RunDuration;
}