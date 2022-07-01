using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MatrixOperators;

namespace Exercise3TestHarness
{
    class Program
    {
        static void Main(string[] args)
        {
            Matrix matrix1 = new Matrix(3);
            matrix1[0, 0] = 1;
            matrix1[0, 1] = 2;
            matrix1[0, 2] = 3;

            matrix1[1, 0] = 4;
            matrix1[1, 1] = 5;
            matrix1[1, 2] = 6;

            matrix1[2, 0] = 7;
            matrix1[2, 1] = 8;
            matrix1[2, 2] = 9;

            Matrix matrix2 = new Matrix(3);
            matrix2[0, 0] = 9;
            matrix2[0, 1] = 8;
            matrix2[0, 2] = 7;

            matrix2[1, 0] = 6;
            matrix2[1, 1] = 5;
            matrix2[1, 2] = 4;

            matrix2[2, 0] = 3;
            matrix2[2, 1] = 2;
            matrix2[2, 2] = 1;

            Console.WriteLine("Matrix 1:");
            Console.WriteLine(matrix1.ToString());
            Console.WriteLine();

            Console.WriteLine("Matrix 2:");
            Console.WriteLine(matrix2.ToString());
            Console.WriteLine();

            Console.WriteLine("Matrix 1 + Matrix 2:");
            Matrix matrix3 = matrix1 + matrix2;
            Console.WriteLine(matrix3.ToString());
            Console.WriteLine();

            Console.WriteLine("Matrix 1 - Matrix 2:");
            Matrix matrix4 = matrix1 - matrix2;
            Console.WriteLine(matrix4.ToString());
            Console.WriteLine();

            Console.WriteLine("Matrix 1 × Matrix 2:");
            Matrix matrix5 = matrix1 * matrix2;
            Console.WriteLine(matrix5.ToString());
            Console.WriteLine();

            Console.ReadLine();
        }
    }
}
