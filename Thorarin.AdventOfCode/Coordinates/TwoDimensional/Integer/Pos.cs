namespace Thorarin.AdventOfCode.Coordinates.TwoDimensional.Integer
{
    public record struct Pos(int X, int Y)
    {
        public static implicit operator Pos((int X, int Y) pos)
        {
            return new Pos(pos.X, pos.Y);
        }
    };
}
