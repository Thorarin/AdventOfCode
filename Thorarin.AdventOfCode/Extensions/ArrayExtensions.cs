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

    /// <summary>
    /// Removes an element from one position in an array, then inserts it in another.
    /// </summary>
    public static void Move<T>(this T[] array, int from, int to)
    {
        var movedElement = array[from];
        var length = from - to;

        if (length == 0) return;

        if (length > 0)
        {
            Array.Copy(array, to, array, to + 1, length);
        }
        else 
        {
            Array.Copy(array, from + 1, array, from, -length);
        }

        array[to] = movedElement;
    }
}