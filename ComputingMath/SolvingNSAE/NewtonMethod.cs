using System;

using ComputingMath;
using ComputingMath.LinearAlgebra;
using ComputingMath.SolvingSLAE;
using ComputingMath.NumericalDifferentiation;

namespace ComputingMath.SolvingNSAE
{
    public class NewtonMethod
    {
        #region Private declarations

        private Vector initialVector = null;
        private DiffrerntialOperators.Function[] f;
        private double precision = 1e-6;

        #endregion

        #region Constructors

        public NewtonMethod(DiffrerntialOperators.Function[] functions, Vector initialVector, double precision)
        {
            this.f = functions.Clone() as DiffrerntialOperators.Function[];
            this.initialVector = initialVector.Clone() as Vector;
            this.precision = precision;
        }

        public NewtonMethod(DiffrerntialOperators.Function[] functions, Vector initialVector) :
            this(functions, initialVector, 1e-6)
        {
        }

        public NewtonMethod(DiffrerntialOperators.Function[] functions) :
            this(functions, new Vector(functions.Length))
        {
        }

        #endregion

        public Vector Solve(Vector initialVector)
        {
            return this.Solve(initialVector, this.precision);
        }

        public Vector Solve(double precision)
        {
            return this.Solve(this.initialVector, precision);
        }

        public Vector Solve()
        {
            return this.Solve(this.initialVector, this.precision);
        }

        public Vector Solve(Vector initialVector, double precision)
        {
            Vector x;

            if (initialVector.Length == this.f.Length)
            {
                bool key = true;
                Vector candidate = new Vector(this.f.Length);
                x = initialVector.Clone() as Vector;
                Vector f = new Vector(this.f.Length);

                while (((candidate - x).Norm() > precision) || (key))
                {
                    if (((candidate - x).Norm() > precision) && (!key))
                    {
                        x = candidate;
                    }

                    key = false;

                    for (int counter = 0; counter < this.f.Length; counter++)
                    {
                        f[counter] = -this.f[counter](x);
                    }

                    try
                    {
                        candidate = x + DirectMethods.GaussMethod(new SquareMatrix(DiffrerntialOperators.JakobiMatrix(x, this.f, precision)), f);
                    }
                    catch (NoSolutionException error)
                    {
                        throw new NoSolutionException("To many solutions or solution does not exists.", error);
                    }
                }
            }
            else
            {
                throw new IncorrectIncomingDataException("Incorrect length of incming vector is set.");
            }

            return x;
        }
    }
}
