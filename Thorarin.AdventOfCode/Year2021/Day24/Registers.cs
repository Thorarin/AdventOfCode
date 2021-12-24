using System.Runtime.InteropServices;

namespace Thorarin.AdventOfCode.Year2021.Day24;

[StructLayout(LayoutKind.Sequential)]
public ref struct Registers
{
    public long W { get; set; }
    public long X { get; set; }
    public long Y { get; set; }
    public long Z { get; set; }
}