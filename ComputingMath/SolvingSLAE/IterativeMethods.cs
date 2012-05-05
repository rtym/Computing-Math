using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ComputingMath;
using ComputingMath.LinearAlgebra;

namespace ComputingMath.SolvingSLAE
{
    public class IterativeMethods
    {
        private const double eps = 1e-6;

        #region JacobiMethod

        public static Vector JacobiMethod(SquareMatrix incomingMatrix, Vector incomingVector)
        {
            return JacobiRealization(incomingMatrix, incomingVector, eps);
        }

        public static Vector JacobiMethod(SquareMatrix incomingMatrix, Vector incomingVector, double precision)
        {
            return JacobiRealization(incomingMatrix, incomingVector, precision);
        }

        private static Vector JacobiRealization(SquareMatrix incomingMatrix, Vector incomingVector, double precision)
        {
            SquareMatrix A = incomingMatrix;
            Vector b = incomingVector;

            if (DiagonalDomination(A))
            {
                Vector x = new Vector(A.Count);
                Vector xCounter = new Vector(A.Count, 1);

                while ((x - xCounter).Norm() > precision)
                {
                    x = xCounter;
                    xCounter = new Vector(A.Count);

                    for (int counter = 0; counter < A.Count; counter++)
                    {
                        for (int counter1 = 0; counter1 < A.Count; counter1++)
                        {
                            if (counter != counter1)
                            {
                                xCounter[counter] += A[counter][counter1] * x[counter1];
                            }
                        }

                        xCounter[counter] -= b[counter];
                        xCounter[counter] /= -A[counter][counter];
                    }
                }

                return x;
            }
            else
            {
                throw new NotDiagonallyDominantException("Input matrix has not diagonal domination. Jakobi method could not work with this matrices.");
            }
        }

        #endregion

        #region SeidelMethod

        public static Vector SeidelMethod(SquareMatrix incomingMatrix, Vector incomingVector)
        {
            return SeidelRealization(incomingMatrix, incomingVector, eps);
        }

        public static Vector SeidelMethod(SquareMatrix incomingMatrix, Vector incomingVector, double precision)
        {
            return SeidelRealization(incomingMatrix, incomingVector, precision);
        }

        private static Vector SeidelRealization(SquareMatrix incomingMatrix, Vector incomingVector, double precision)
        {
            SquareMatrix A = incomingMatrix;
            Vector b = incomingVector;

            if (DiagonalDomination(A))
            {
                Vector x = new Vector(A.Count);
                Vector xCounter = new Vector(A.Count, 1);

                while ((x - xCounter).Norm() > precision)
                {
                    x = xCounter;
                    xCounter = new Vector(A.Count);

                    for (int counter = 0; counter < A.Count; counter++)
                    {
                        for (int counter1 = 0; counter1 < A.Count; counter1++)
                        {
                            if (counter != counter1)
                            {
                                if (counter1 < counter)
                                {
                                    xCounter[counter] += A[counter][counter1] * xCounter[counter1];
                                }
                                else
                                {
                                    xCounter[counter] += A[counter][counter1] * x[counter1];
                                }
                            }
                        }

                        xCounter[counter] -= b[counter];
                        xCounter[counter] /= -A[counter][counter];
                    }
                }

                return x;
            }
            else
            {
                throw new NotDiagonallyDominantException("Input matrix has not diagonal domination. Jakobi method could not work with this matrices.");
            }
        }

        #endregion

        #region RelaxationMethod

        public static Vector RelaxationMethod(SquareMatrix incomingMatrix, Vector incomingVector, double relaxationParameter)
        {
            return RelaxationRealisation(incomingMatrix, incomingVector, relaxationParameter, eps);
        }

        public static Vector RelaxationMethod(SquareMatrix incomingMatrix, Vector incomingVector, double relaxationParameter, double precision)
        {
            return RelaxationRealisation(incomingMatrix, incomingVector, relaxationParameter, precision);
        }

        private static Vector RelaxationRealisation(SquareMatrix incomingMatrix, Vector incomingVector, double relaxationParameter, double precision)
        {
            SquareMatrix A = incomingMatrix;
            Vector b = incomingVector;
            Vector x = new Vector(A.Count);
            Vector xCounter = new Vector(A.Count, 1);

            if ((relaxationParameter > 0.0) && (relaxationParameter < 2.0))
            {
                while ((x - xCounter).Norm() > precision)
                {
                    x = xCounter;
                    xCounter = new Vector(A.Count);

                    for (int counter = 0; counter < xCounter.Length; counter++)
                    {
                        xCounter[counter] = b[counter];
                           
                        for (int counter1 = 0; counter1 < counter; counter1++)
                        {
                            xCounter[counter] -= A[counter][counter1] * xCounter[counter1];
                        }
                        for (int counter1 = counter + 1; counter1 < x.Length; counter1++)
                        {
                            xCounter[counter] -= A[counter][counter1] * x[counter1];
                        }

                        xCounter[counter] *= (relaxationParameter / A[counter][counter]);
                        xCounter[counter] += (1 - relaxationParameter) * x[counter];
                    }
                }
            }
            else
            {
                throw new InvalidRelaxationParameterException("Relaxation method is valid for relaxation parameters between 0 and 2.");
            }
            return x;
        }

        #endregion

        private static bool DiagonalDomination(SquareMatrix A)
        {
            bool key = true;

            for (int counter = 0; ((counter < A.Count) && (key)); counter++)
            {
                key = A[counter].Norm(VectorNormConstants.First) <= 2 * Math.Abs(A[counter][counter]);
            }

            return key;
        }
    }
}
