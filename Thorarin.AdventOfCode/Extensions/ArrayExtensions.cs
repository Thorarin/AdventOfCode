namespace Thorarin.AdventOfCode.Extensions;

public static class ArrayExtensions
{
    public static void Fill<T>(this T[,] array, T value)
    {
        int size1 = array.GetLength(0);
        int size2 = array.GetLength(1);

        for (int d1 = 0; d1 < size1; d1++)
        {
            for (int d2 = 0; d2 < size2; d2++)
            {
                array[d1, d2] = value;
            }
        } 
    }

    public static void Fill<T>(this T[] array, T value)
    {
        int size1 = array.GetLength(0);

        for (int d1 = 0; d1 < size1; d1++)
        {
            array[d1] = value;
        }
    }
}