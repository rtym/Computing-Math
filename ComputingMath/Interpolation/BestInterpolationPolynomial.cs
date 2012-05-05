using System;

using ComputingMath;
using ComputingMath.LinearAlgebra;
using ComputingMath.SolvingSLAE;

namespace ComputingMath.Interpolation
{
    public class BestInterpolationPolynomial
    {
        #region Private Fields

        private Vector alpha;

        #endregion

        #region Constructors

        public BestInterpolationPolynomial(Vector x, Vector y, int power)
        {
            if (power > 0)
            {
                if (x.Length == y.Length)
                {
                    if (x.Length != 0)
                    {
                        SquareMatrix A = new SquareMatrix(power);
                        Vector b = new Vector(power);

                        for (int counter = 0; counter < power; counter++)
                        {
                            for (int counter1 = 0; counter1 < power; counter1++)
                            {
                                for (int counter2 = 0; counter2 < x.Length; counter2++)
                                {
                                    A[counter][counter1] += Math.Pow(x[counter2], counter + counter1);
                                }
                            }

                            for (int counter2 = 0; counter2 < x.Length; counter2++)
                            {
                                b[counter] += Math.Pow(x[counter2], counter) * y[counter2];
                            }
                        }
                        try
                        {
                            this.alpha = DirectMethods.GaussMethod(A, b);
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

        public BestInterpolationPolynomial(double[] x, double[] y, int power) :
            this(new Vector(x), new Vector(y), power)
        {
        }

        #endregion

        public double Solve(double x)
        {
            double result = 0;

            for (int counter = 0; counter < this.alpha.Length; counter++)
            {
                result += this.alpha[counter] * Math.Pow(x, counter);
            }

            return result;
        }
    }
}
