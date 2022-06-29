using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace GreatestCommonDivisor
{
    static class GCDAlgorithms
    {
        /// <summary>
        /// Find the lowest common divisor of two numbers using Euclid's Algorithm
        /// see: http://en.wikipedia.org/wiki/Euclidean_algorithm
        /// </summary>
        /// <param name="a">First number</param>
        /// <param name="b">Second number</param>
        /// <param name="time">Output time taken (in ticks)</param>
        /// <returns>Lowest common divisor</returns>
        static public int FindGCDEuclid(int a, int b, out long time)
        {
            // Initialize timing
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

            // Stop the timer and record execution time
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
        /// <param name="time">Output time taken (in ticks)</param>
        /// <returns>Lowest common divisor</returns>
        static public int FindGCDEuclid(int a, int b, int c, out long time)
        {
            // Find the LCD of the first two numbers, then find the LCD of the result and the third
            long t1;
            int d = FindGCDEuclid(a, b, out t1);
            long t2;
            int e = FindGCDEuclid(d, c, out t2);
            time = t1 + t2;
            return e;
        }

        /// <summary>
        /// Find the lowest common divisor of four numbers using Euclid's Algorithm
        /// </summary>
        /// <param name="a">First number</param>
        /// <param name="b">Second number</param>
        /// <param name="c">Third number</param>
        /// <param name="d">Fourth number</param>
        /// <param name="time">Output time taken (in ticks)</param>
        /// <returns>Lowest common divisor</returns>
        static public int FindGCDEuclid(int a, int b, int c, int d, out long time)
        {
            // Find the LCD of the first three numbers, then find the LCD of the result and the fourth
            long t1;
            int e = FindGCDEuclid(a, b, c, out t1);
            long t2;
            int f = FindGCDEuclid(e, d, out t2);
            time = t1 + t2;
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
        /// <param name="time">Output time taken (in ticks)</param>
        /// <returns>Lowest common divisor</returns>
        static public int FindGCDEuclid(int a, int b, int c, int d, int e, out long time)
        {
            // Find the LCD of the first four numbers, then find the LCD of the result and the fifth
            long t1;
            int f = FindGCDEuclid(a, b, c, d, out t1);
            long t2;
            int g = FindGCDEuclid(f, e, out t2);
            time = t1 + t2;
            return g;
        }

        /// <summary>
        /// Find the lowest common divisor of two numbers using Stein's Algorithm
        /// see: http://en.wikipedia.org/wiki/Binary_GCD_algorithm
        /// </summary>
        /// <param name="a">First number</param>
        /// <param name="b">Second number</param>
        /// <param name="time">Output time taken (in ticks)</param>
        /// <returns>Lowest common divisor</returns>
        static public int FindGCDStein(int u, int v, out long time)
        {
            // Initialize timing
            time = 0;
            Stopwatch sw = new Stopwatch();
            sw.Start();

            int k;

            // Step 1.
            // gcd(0, v) = v, because everything divides zero, and v is the largest number that divides v. 
            // Similarly, gcd(u, 0) = u. gcd(0, 0) is not typically defined, but it is convenient to set gcd(0, 0) = 0.
            if (u == 0 || v == 0)
            {
                sw.Stop();
                time = sw.ElapsedTicks;
                return u | v;
            }

            // Step 2.
            // If u and v are both even, then gcd(u, v) = 2·gcd(u/2, v/2), because 2 is a common divisor. 
            for (k = 0; ((u | v) & 1) == 0; ++k)
            {
                u >>= 1;
                v >>= 1;
            }

            // Step 3.
            // If u is even and v is odd, then gcd(u, v) = gcd(u/2, v), because 2 is not a common divisor. 
            // Similarly, if u is odd and v is even, then gcd(u, v) = gcd(u, v/2). 
            while ((u & 1) == 0)
                u >>= 1;

            // Step 4.
            // If u and v are both odd, and u ≥ v, then gcd(u, v) = gcd((u − v)/2, v). 
            // If both are odd and u < v, then gcd(u, v) = gcd((v − u)/2, u). 
            // These are combinations of one step of the simple Euclidean algorithm, 
            // which uses subtraction at each step, and an application of step 3 above. 
            // The division by 2 results in an integer because the difference of two odd numbers is even.
            do
            {
                while ((v & 1) == 0)  // Loop x
                    v >>= 1;

                // Now u and v are both odd, so diff(u, v) is even.
                //   Let u = min(u, v), v = diff(u, v)/2. 
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
                // Step 5.
                // Repeat steps 3–4 until u = v, or (one more step) until u = 0.
                // In either case, the result is (2^k) * v, where k is the number of common factors of 2 found in step 2. 
            } while (v != 0);

            u <<= k;

            // Stop timer
            sw.Stop();
            time = sw.ElapsedTicks;

            return u;
        }
    }
}
