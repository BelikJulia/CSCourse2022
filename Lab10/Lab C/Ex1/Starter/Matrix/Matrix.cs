using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace MatrixOperators
{
    public class Matrix
    {
        int[,] data;
        public Matrix(int size)
        {
            data = new int[size, size];
        }
        public int this[int RowIndex, int ColumnIndex]
        {
            get
            {
                if (RowIndex > data.GetUpperBound(0) ||
                ColumnIndex > data.GetUpperBound(0))
                {
                    throw new IndexOutOfRangeException();
                }
                else
                {
                    return data[RowIndex, ColumnIndex];
                }
            }
            set
            {
                if (RowIndex > data.GetUpperBound(0) ||
                ColumnIndex > data.GetUpperBound(0))
                {
                    throw new IndexOutOfRangeException();
                }
                else
                {
                    data[RowIndex, ColumnIndex] = value;
                }
            }
        }
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            // Iterate over every row in the matrix.
            for (int x = 0; x < data.GetLength(0); x++)
            {
                // Iterate over every column in the matrix.
                for (int y = 0; y < data.GetLength(1); y++)
                {
                    builder.AppendFormat("{0}\t", data[x, y]);
                }
                builder.Append(Environment.NewLine);
            }
            return builder.ToString();
        }
    }

    public class MatrixNotCompatibleException : Exception
    {
        Matrix firstMatrix = null;
        Matrix secondMatrix = null;
        public Matrix FirstMatrix
        {
            get
            {
                return firstMatrix;
            }
        }
        public Matrix SecondMatrix
        {
            get
            {
                return secondMatrix;
            }
        }
        public MatrixNotCompatibleException()
            : base()
        {
        }
        public MatrixNotCompatibleException(string message)
            : base(message)
        {
        }
        public MatrixNotCompatibleException(string message,
         Exception innerException)
            : base(message, innerException)
        {
        }
        public MatrixNotCompatibleException(SerializationInfo info,
         StreamingContext context)
            : base(info, context)
        {
        }
        public MatrixNotCompatibleException(Matrix matrix1, Matrix matrix2, string message) : base(message)
        {
            firstMatrix = matrix1;
            secondMatrix = matrix2;
        }
    }
}
