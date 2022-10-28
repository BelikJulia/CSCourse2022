using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace GreatestCommonDivisor
{
    static class GCDAlgorithms
    {
        // TODO Exercise 3, Task 3
        // Modify all methods to return timing code

        /// <summary>
        /// Find the lowest common divisor of two numbers using Euclid's Algorithm
        /// see: http://en.wikipedia.org/wiki/Euclidean_algorithm
        /// </summary>
        /// <param name="a">First number</param>
        /// <param name="b">Second number</param>
        /// <returns>Lowest common divisor</returns>
        static public int FindGCDEuclid(int a, int b, out long time)
        {
            time = 0;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            // If the first number is zero, return the second
            if (a == 0) return b;

            // Calculate the LCD
            while (b != 0)
            {
                if (a > b)
                {
                    a = a - b;
                }
                else
                {
                    b = b - a;
                }
            }
            sw.Stop();
            time = sw.ElapsedTicks;
            return a;
        }

        /// <summary>
        /// Find the lowest common divisor of three numbers using Euclid's Algorithm
        /// </summary>
        /// <param name="a">First number</param>
        /// <param name="b">Second number</param>
        /// <param name="c">Third number</param>
        /// <returns>Lowest common divisor</returns>
        static public int FindGCDEuclid(int a, int b, int c)
        {
            // Find the LCD of the first two numbers, then find the LCD of the result and the third
            long time;
            int d = FindGCDEuclid(a, b, out time);
            int e = FindGCDEuclid(d, c, out time);
            return e;
        }

        /// <summary>
        /// Find the lowest common divisor of four numbers using Euclid's Algorithm
        /// </summary>
        /// <param name="a">First number</param>
        /// <param name="b">Second number</param>
        /// <param name="c">Third number</param>
        /// <param name="d">Fourth number</param>
        /// <returns>Lowest common divisor</returns>
        static public int FindGCDEuclid(int a, int b, int c, int d)
        {
            // Find the LCD of the first three numbers, then find the LCD of the result and the fourth
            long time;
            int e = FindGCDEuclid(a, b, c);
            int f = FindGCDEuclid(e, d, out time);

            return f;
        }

        /// <summary>
        /// Find the lowest common divisor of five numbers using Euclid's Algorithm
        /// </summary>
        /// <param name="a">First number</param>
        /// <param name="b">Second number</param>
        /// <param name="c">Third number</param>
        /// <param name="d">Fourth number</param>
        /// <param name="e">Fourth number</param>
        /// <returns>Lowest common divisor</returns>
        static public int FindGCDEuclid(int a, int b, int c, int d, int e)
        {
            // Find the LCD of the first four numbers, then find the LCD of the result and the fifth
            long time;
            int f = FindGCDEuclid(a, b, c, d);
            int g = FindGCDEuclid(f, e, out time);
            return g;
        }

        public static int FindGCDStein(int u, int v, out long time)
        {
            time = 0;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            int k;
            if (u == 0 || v == 0)
            {
                sw.Stop();
                time = sw.ElapsedTicks;
                return u | v;
            }
            for (k = 0; ((u | v) & 1) == 0; ++k)
            {
                u >>= 1;
                v >>= 1;
            }
            while ((u & 1) == 0)
                u >>= 1;
            do
            {
                while ((v & 1) == 0)
                    v >>= 1;
                if (u < v)
                {
                    v -= u;
                }
                else
                {
                    int diff = u - v;
                    u = v;
                    v = diff;
                }
                v >>= 1;
            } while (v != 0);
            u <<= k;
            sw.Stop();
            time = sw.ElapsedTicks;
            return u;
        }
    }
}
