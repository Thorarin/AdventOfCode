﻿using System.Runtime.CompilerServices;

namespace Thorarin.AdventOfCode;

public class MathEx
{
    /// <summary>
    /// Returns the greatest common divisor.
    /// </summary>
    public static long Gcd(long a, long b)
    {
        if (b == 0)
            return a;

        if (a == 0)
            return b;

        return Gcd(b, a % b);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Modulo(int a, int b)
    {
        int tmp = a % b;
        return tmp >= 0 ? tmp : tmp + b;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long Modulo(long a, long b)
    {
        long tmp = a % b;
        return tmp >= 0 ? tmp : tmp + b;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Max(int a, int b, int c)
    {
        return Math.Max(Math.Max(a, b), c);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Max(int a, int b, int c, int d)
    {
        return Math.Max(Math.Max(a, b), Math.Max(c, d));
    }

    /// <summary>
    /// Returns the least common multiplier.
    /// </summary>
    public static long Lcm(long a, long b) => a / Gcd(a, b) * b;

    /// <summary>
    /// Calculates the triangular number of the given number.
    /// For example, the termial of 5 is 1+2+3+4+5=15.
    /// https://proofwiki.org/wiki/Definition:Termial
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Termial(int number) => number * (number + 1) / 2;

    /// <summary>
    /// Calculates the triangular number of the given number.
    /// For example, the termial of 5 is 1+2+3+4+5=15.
    /// https://proofwiki.org/wiki/Definition:Termial
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long Termial(long number) => number * (number + 1) / 2;

    /// <summary>
    /// Calculates the termial of a triangular number.
    /// For example, the triangular number 15 yields 5.
    /// https://proofwiki.org/wiki/Definition:Termial
    /// https://en.wikipedia.org/wiki/Triangular_number
    /// </summary>
    public static int InverseTermialUnsafe(int number)
    {
        return (int)Math.Sqrt(number * 2);
    }

    /// <summary>
    /// Calculates the termial of a triangular number.
    /// For example, the triangular number 15 yields 5.
    /// https://proofwiki.org/wiki/Definition:Termial
    /// https://en.wikipedia.org/wiki/Triangular_number
    /// </summary>
    /// <exception cref="ArgumentException">
    /// Thrown if the input number is not a triangular number.
    /// </exception>
    public static int InverseTermial(int number)
    {
        int result = InverseTermialUnsafe(number);
        if (Termial(result) != number)
        {
            throw new ArgumentException("Number is not a triangular number", nameof(number));
        }

        return result;
    }

    public static int Pow(int x, int pow)
    {
        if (pow < 0) throw new ArgumentOutOfRangeException(nameof(pow), pow, "Negative exponents are not supported.");
        
        int result = 1;
        while (true)
        {
            if ((pow & 1) == 1) result *= x;
            
            pow >>= 1;
            if (pow == 0) break;
            x *= x;
        }
        return result;
    }

    public static long Pow(long x, int pow)
    {
        if (pow < 0) throw new ArgumentOutOfRangeException(nameof(pow), pow, "Negative exponents are not supported.");
        
        long result = 1;
        while (true)
        {
            if ((pow & 1) == 1) result *= x;

            pow >>= 1;
            if (pow == 0) break;
            x *= x;
        }
        return result;
    }
}