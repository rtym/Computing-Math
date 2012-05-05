using System;

using ComputingMath;
using ComputingMath.LinearAlgebra;
using ComputingMath.SolvingSLAE;

namespace ComputingMath.Interpolation
{
    public class BestInterpolationLeastSquares
    {
        #region Private Fields

        private Vector alpha;

        #endregion

        #region Constructors

        public BestInterpolationLeastSquares(Vector x, Vector y, int power)
        {
            if (power > 0)
            {
                if (x.Length == y.Length)
                {
                    if (x.Length != 0)
                    {
                        Matrix A = new Matrix(x.Length, power + 1);

                        for (int counter = 0; counter < x.Length; counter++)
                        {
                            for (int counter1 = 0; counter1 < power + 1; counter1++)
                            {
                                A[counter][counter1] = Math.Pow(x[counter], power - counter1);
                            }
                        }

                        try
                        {
                            this.alpha = DirectMethods.LeastSquares(A, y, power + 1);
                        }
                        catch (NoSolutionException error)
                        {
                            throw new IncorrectIncomingDataException("Error searching solutions for incoming vectors", error);
                        }
                    }
                    else
                    {
                        throw new IncorrectIncomingDataException("Length of incoming vectors could not be equal to zero.");
                    }
                }
                else
                {
                    throw new IncorrectIncomingDataException("Length of incoming vectors are not equal.");
                }
            }
            else
            {
                throw new IncorrectIncomingDataException("Incorrect power for polynomial was set.");
            }
        }

        public BestInterpolationLeastSquares(double[] x, double[] y, int power) :
            this(new Vector(x), new Vector(y), power)
        {
        }

        #endregion

        public double Solve(double x)
        {
            double result = 0;

            for (int counter = 0; counter < this.alpha.Length; counter++)
            {
                result += this.alpha[counter] * Math.Pow(x, 2 - counter);
            }

            return result;
        }
    }
}
