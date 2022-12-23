namespace Thorarin.AdventOfCode.Year2022.Day22
{
    internal static class DirectionExtensions
    {
        public static Direction TurnLeft(this Direction direction)
        {
            return (Direction)MathEx.Modulo((int)direction - 1, 4);
        }

        public static Direction TurnRight(this Direction direction)
        {
            return (Direction)(((int)direction + 1) % 4);
        }

        public static Direction Reverse(this Direction direction)
        {
            return (Direction)(((int)direction + 2) % 4);
        }
    }
}
