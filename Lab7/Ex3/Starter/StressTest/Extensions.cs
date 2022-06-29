using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StressTest
{
    public static class Extensions
    {
        public static string ToBinaryString(this long i)
        {
            long remainder = 0;
            StringBuilder binary = new StringBuilder("");
            while (i > 0)
            {
                remainder = i % 2;
                i = i / 2;
                binary.Insert(0, remainder);
            }
            return binary.ToString();
        }
    }
}
