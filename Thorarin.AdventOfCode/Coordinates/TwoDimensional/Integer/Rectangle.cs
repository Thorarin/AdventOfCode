namespace Thorarin.AdventOfCode.Coordinates.TwoDimensional.Integer
{
    public record struct Rectangle(Pos A, Pos B)
    {
        public void Deconstruct(out int x1, out int y1, out int x2, out int y2)
        {
            x1 = A.X;
            y1 = A.Y;
            x2 = B.X;
            y2 = B.Y;
        }
    }
}
