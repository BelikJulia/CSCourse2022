using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace GreatestCommonDivisor
{
    static class GCDAlgorithms
    {
        public static int FindGCDEuclid(int a, int b)
        {
            if (a == 0)
                return b;
            else
            {
                while (b != 0)
                {
                    if (a > b)
                        a -= b;
                    else
                        b -= a;
                }
                return a;
            }
        }

    }
}
