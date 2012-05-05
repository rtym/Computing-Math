using System;

using ComputingMath;
using ComputingMath.LinearAlgebra;

namespace ComputingMath.NumericalDifferentiation
{
    public static class DiffrerntialOperators
    {
        #region Delegates

        public delegate double Function(Vector x);

        #endregion

        #region Searching Jakobi Matrix

        public static Matrix JakobiMatrix(Vector x, Function[] f)
        {
            return JakobiMatrix(x, f, 1e-6);
        }

        public static Matrix JakobiMatrix(Vector x, Function[] f, double precision)
        {
            if ((f.Length > 0) && (x.Length > 0) && (precision > 0))
            {

                Matrix J = new Matrix(f.Length, x.Length);
                Vector temp = x.Clone() as Vector;

                for (int counter1 = 0; counter1 < J.Height; counter1++)
                {
                    for (int counter2 = 0; counter2 < J.Width; counter2++)
                    {
                        temp[counter2] += precision;

                        J[counter1][counter2] = (f[counter1](temp) - f[counter1](x)) / precision;

                        temp[counter2] -= precision;
                    }
                }

                return J;
            }
            else
            {
                throw new IncorrectIncomingDataException("Dimensions of delegates or vectors or precision are incorrect.");
            }
        }

        #endregion

        #region Searching Hesse Matrix

        public static SquareMatrix HesseMatrix(Vector x, Function f)
        {
            return HesseMatrix(x, f, 1e-6);
        }

        public static SquareMatrix HesseMatrix(Vector x, Function f, double precision)
        {
            if ((x.Length > 0) && (precision > 0))
            {
                SquareMatrix H = new SquareMatrix(x.Length);
                Vector temp = x.Clone() as Vector;

                for (int counter1 = 0; counter1 < H.Height; counter1++)
                {
                    for (int counter2 = 0; counter2 < H.Width; counter2++)
                    {
                        double sum = f(x);

                        temp[counter2] += precision;
                        temp[counter1] += precision;

                        sum += f(temp);

                        temp[counter2] -= precision;

                        sum -= f(temp);

                        temp[counter2] += precision;
                        temp[counter1] -= precision;

                        sum -= f(temp);

                        H[counter1][counter2] = sum / Math.Pow(precision, 2);

                        temp[counter2] -= precision;
                    }
                }

                return H;
            }
            else
            {
                throw new IncorrectIncomingDataException("Dimensions of delegates or vectors or precision are incorrect.");
            }
        }

        #endregion
    }
}
