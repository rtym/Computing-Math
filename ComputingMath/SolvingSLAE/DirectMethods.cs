using System;
using ComputingMath;
using ComputingMath.LinearAlgebra;

namespace ComputingMath.SolvingSLAE
{
    public static class DirectMethods
    {
        #region GaussMethod

        public static Vector GaussMethod(SquareMatrix incomingMatrix, Vector incomingVector)
        {
            if ((incomingMatrix.Height == incomingMatrix.Width) && (incomingVector.Length == incomingMatrix.Height))
            {
                SquareMatrix A = incomingMatrix.Clone() as SquareMatrix;
                Vector b = incomingVector.Clone() as Vector;
                double DET = 1;

                for (int counter = 0; counter < A.Count; counter++)
                {
                    if (A[counter][counter] == 0)
                    {
                        int position = GetPosition(counter, A);
                        A.swapRows(counter, position);
                        b.swap(counter, position);
                        DET *= -1;
                    }

                    DET *= A[counter][counter];

                    for (int counter1 = counter + 1; counter1 < A.Count; counter1++)
                    {
                        if (A[counter][counter] != 0)
                        {
                            double divisor = A[counter1][counter] / A[counter][counter];
                            A[counter1] -= (divisor * A[counter]);
                            b[counter1] -= (divisor * b[counter]);
                        }
                    }
                }

                if (DET == 0)
                {
                    throw new NoSolutionException("Determinant of input matrix equals to zero. No solution could be found.");
                }
                else
                {
                    Vector result = new Vector(A.Count);

                    for (int counter = A.Count - 1; counter >= 0; counter--)
                    {
                        double diff = 0;

                        for (int counter1 = A.Count - 1; counter1 > counter; counter1--)
                        {
                            diff += A[counter][counter1] * result[counter1];
                        }

                        result[counter] = (b[counter] - diff) / A[counter][counter];
                    }

                    return result;
                }
            }
            else
            {
                throw new IncorrectMatrixSizeError("Gauss method works only with square matrices and vectors with same size.");
            }
        }

        private static int GetPosition(int current, SquareMatrix A)
        {
            int result = current;

            bool key = false;

            for (int counter = current + 1; ((counter < A.Count) && (!key)); counter++)
            {
                key = A[counter][current] != 0;

                if (key)
                {
                    result = counter;
                }
            }

            return result;
        }

        #endregion

        #region Least Squares Method

        public static Vector LeastSquares(Matrix incomingMatrix, Vector incomingVector, int power)
        {
            if (incomingMatrix.Height == incomingVector.Length)
            {
                SquareMatrix A = new SquareMatrix(power);
                Vector b = new Vector(power);

                for (int counter1 = 0; counter1 < A.Count; counter1++)
                {
                    for (int counter2 = 0; counter2 < A.Count; counter2++)
                    {
                        for (int counter = 0; counter < incomingMatrix.Height; counter++)
                        {
                            A[counter1][counter2] += incomingMatrix[counter][counter1] * incomingMatrix[counter][counter2];
                        }
                    }

                    for (int counter = 0; counter < incomingVector.Length; counter++)
                    {
                        b[counter1] += incomingMatrix[counter][counter1] * incomingVector[counter];
                    }
                }

                return GaussMethod(A, b);
            }
            else
            {
                throw new IncorrectMatrixSizeError("Height of matrix and Length of Vector are different.");
            }
        }

        #endregion

        #region Cholesky decomposition

        public static Vector CholeskyDecomposition(SquareMatrix incomingMatrix, Vector incomingVector)
        {
            if (incomingMatrix.IsSymetric)
            {
                Vector x = new Vector(incomingVector.Length);
                Vector y = new Vector(incomingVector.Length);
                SquareMatrix L = new SquareMatrix(incomingMatrix.Count);
                bool key = true;

                for (int i = 0; i < incomingMatrix.Count; i++)
                {
                    for (int k = 0; k <= i; k++)
                    {
                        double sum = 0;

                        if (i != k)
                        {
                            for (int counter = 0; counter <= k - 1; counter++)
                            {
                                sum += L[i][counter] * L[k][counter];
                            }

                            L[i][k] = (incomingMatrix[i][k] - sum) / L[k][k];
                        }
                        else
                        {
                            for (int counter = 0; counter <= k - 1; counter++)
                            {
                                sum += Math.Pow(L[k][counter], 2);
                            }

                            L[k][k] = Math.Sqrt(incomingMatrix[k][k] - sum);
                        }
                    }
                }

                for (int counter = 0; (counter < y.Length) && (key); counter++)
                {
                    double sum = 0;

                    for (int i = 0; i < counter; i++)
                    {
                        sum += y[i] * L[counter][i];
                    }

                    y[counter] = (incomingVector[counter] - sum) / L[counter][counter];
                    key = y[counter].ToString() != "NaN";
                }

                for (int counter = y.Length - 1; (counter >= 0) && (key); counter--)
                {
                    double sum = 0;

                    for (int i = y.Length - 1; i > counter; i--)
                    {
                        sum += x[i] * L[i][counter];
                    }

                    x[counter] = (y[counter] - sum) / L[counter][counter];
                    key = y[counter].ToString() != "NaN";
                }

                if (!key)
                {
                    throw new NoSolutionException("Single solution for this matrix does not exist.");
                }

                return x;
            }
            else
            {
                throw new NotSymetricMatrixException("CholeskyDecomposition couldn't solve not symetric matrices.");
            }
        }

        #endregion

        #region LU Method

        public static Vector LUDecomposition(SquareMatrix incomingMatrix, Vector incomingVector)
        {
            LUDecomposition lu = new LUDecomposition (incomingMatrix, incomingVector);

            return lu.Solve ();
        }

        #endregion
    }
}
