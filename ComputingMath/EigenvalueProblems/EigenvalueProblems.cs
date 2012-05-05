using System;
using System.Collections.Generic;
using ComputingMath.LinearAlgebra;

namespace ComputingMath.Eigenvalues
{
    public class EigenvalueProblems
    {
        #region PowerMethod

        public static double PowerMethod(SquareMatrix A, double precision)
        {
            Vector x = new Vector(A.Count);
            Vector y;
            x[0] = 1;
            bool key = true;
            Vector lambda = new Vector(A.Count);
            Vector candidate = new Vector(A.Count);
            int effectiveCount = 0;

            while (key)
            {
                y = A * x;
                effectiveCount = x.Length;

                for (int counter = 0; counter < x.Length; counter++)
                {
                    if (Math.Abs(x[counter]) > precision)
                    {
                        candidate[counter] = y[counter] / x[counter];
                    }
                    else
                    {
                        candidate[counter] = 0;
                        effectiveCount--;
                    }
                }

                x = y / y.Norm();

                key = (candidate - lambda).Norm() > precision;

                if (key)
                {
                    lambda = candidate;
                    candidate = new Vector(x.Length);
                }
            }

            double result;

            if (effectiveCount > 0)
            {
                result = lambda.Sum() / (double)effectiveCount;
            }
            else
            {
                result = 0;
            }

            return result;
        }

        #endregion

        #region JacobiRotationMethod

        public static Vector JacobiRotationMethod(SquareMatrix incomingMatrix)
        {
            Vector result = null;

            if (incomingMatrix.IsSymetric == true)
            {
                SquareMatrix A = incomingMatrix.Clone() as SquareMatrix;
                bool key = true;

                while (key)
                {
                    SquareMatrix B = A.Clone() as SquareMatrix;
                    double max = 0;
                    int i = 0, j = 0;

                    key = false;

                    for (int counter1 = 0; counter1 < A.Count; counter1++)
                    {
                        for (int counter2 = 0; counter2 < A.Count; counter2++)
                        {
                            if ((counter1 != counter2) && (Math.Abs(A[counter1][counter2]) > max))
                            {
                                max = Math.Abs(A[counter1][counter2]);
                                i = counter1;
                                j = counter2;
                                key = true;
                            }
                        }
                    }

                    if (key)
                    {
                        double p = 2 * A[i][j];
                        double q = A[i][i] - A[j][j];
                        double d = Math.Sqrt(Math.Pow(p, 2) + Math.Pow(q, 2));
                        double c, s;

                        if (q == 0)
                        {
                            c = s = 1 / Math.Sqrt(2);
                        }
                        else
                        {
                            double r = Math.Abs(q) / (2 * d);
                            c = Math.Sqrt(0.5 + r);
                            s = Math.Sqrt(0.5 - r) * Math.Sign(p * q);
                        }

                        B[i][i] = Math.Pow(c, 2) * A[i][i] + Math.Pow(s, 2) * A[j][j] + 2 * c * s * A[i][j];
                        B[j][j] = Math.Pow(s, 2) * A[i][i] + Math.Pow(c, 2) * A[j][j] - 2 * c * s * A[i][j];
                        B[i][j] = B[j][i] = 0;

                        for (int m = 0; m < A.Count; m++)
                        {
                            if ((m != i) && (m != j))
                            {
                                B[i][m] = B[m][i] = c * A[m][i] + s * A[m][j];
                                B[j][m] = B[m][j] = -s * A[m][i] + c * A[m][j];
                            }
                        }

                        A = B;
                    }
                    else
                    {
                        result = B.Diag();
                    }
                }
            }
            else
            {
                throw new IncorrectIncomingDataException("Incorrect incoming data.");
            }

            return result;
        }

        #endregion
    }
}
