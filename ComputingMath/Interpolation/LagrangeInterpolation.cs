using System;

using ComputingMath;
using ComputingMath.LinearAlgebra;

namespace ComputingMath.Interpolation
{
    public class LagrangeInterpolation
    {
        #region Private Fields

        private Vector y;
        private Vector x;

        #endregion

        #region Constructors

        public LagrangeInterpolation(Vector x, Vector y)
        {
            if (x.Length == y.Length)
            {
                if (x.Length != 0)
                {
                    this.x = x.Clone() as Vector;
                    this.y = y.Clone() as Vector;
                }
                else
                {
                    throw new IncorrectIncomingDataException("Dimensions of incoming vectors should be greater than 0");
                }
            }
            else
            {
                throw new IncorrectIncomingDataException("Length of incoming vwctors are not equal.");
            }
        }

        public LagrangeInterpolation(double[] x, double[] y) :
            this(new Vector(x), new Vector(y))
        {
        }

        #endregion

        public double Solution(double x)
        {
            double result = 0;

            for (int counter = 0; counter < this.y.Length; counter++)
            {
                result += this.y[counter] * this.L(counter, x);
            }

            return result;
        }

        private double L(int k, double x)
        {
            double result = 1;

            for (int counter = 0; counter < this.x.Length; counter++)
            {
                if (k != counter)
                {
                    result *= (x - this.x[counter]) / (this.x[k] - this.x[counter]);
                }
            }

            return result;
        }
    }
}
