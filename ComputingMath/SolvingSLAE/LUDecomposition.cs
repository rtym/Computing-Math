using System;

using ComputingMath;
using ComputingMath.LinearAlgebra;

namespace ComputingMath.SolvingSLAE
{
    public class LUDecomposition
    {
        #region Private Fields

        private SquareMatrix L = null;
        private SquareMatrix U = null;
        private Vector b = new Vector();
        private double det = 1;

        #endregion

        public double DET
        {
            get
            {
                return this.det;
            }
        }

        public LUDecomposition()
        {
        }

        public LUDecomposition(SquareMatrix incomingMatrix, Vector incomingVector) :
            this(incomingMatrix)
        {
            this.b = incomingVector;
        }

        public LUDecomposition(SquareMatrix incomingMatrix)
        {
            this.L = new SquareMatrix(incomingMatrix.Count);
            this.U = new SquareMatrix(incomingMatrix.Count);

            for (int counter = 0; counter < incomingMatrix.Count; counter++)
            {
                for (int counter1 = 0; counter1 < incomingMatrix.Count; counter1++)
                {
                    if (counter1 == counter)
                    {
                        this.L[counter][counter1] = 1;
                    }
                    if (counter1 >= counter)
                    {
                        this.U[counter][counter1] = incomingMatrix[counter][counter1] - this.sum(counter - 1, counter, counter1);

                        if (counter == counter1)
                        {
                            this.det *= this.U[counter][counter1];
                        }
                    }
                    else
                    {
                        this.L[counter][counter1] = (incomingMatrix[counter][counter1] - this.sum(counter1 - 1, counter, counter1)) / this.U[counter1][counter1];
                    }
                }
            }
        }

        public Vector Solve()
        {
            return this.Solve(this.b);
        }

        public Vector Solve(Vector vectorColumn)
        {
            Vector result;
            double sum = 0;

            if (vectorColumn.Length > 0)
            {
                if (vectorColumn.Length == L.Count)
                {
                    if (this.DET != 0)
                    {
                        result = new Vector(vectorColumn.Length);

                        for (int counter = 0; counter < result.Length; counter++)
                        {
                            if (counter == 0)
                            {
                                result[counter] = vectorColumn[counter] / this.L[counter][counter];
                            }
                            else
                            {
                                sum = 0;

                                for (int counter1 = 0; counter1 < counter; counter1++)
                                {
                                    sum += this.L[counter][counter1] * result[counter1];
                                }

                                result[counter] = (vectorColumn[counter] - sum) / this.L[counter][counter];
                            }
                        }

                        for (int counter = result.Length - 1; counter >= 0; counter--)
                        {
                            if (counter == result.Length - 1)
                            {
                                result[counter] = result[counter] / this.U[counter][counter];
                            }
                            else
                            {
                                sum = 0;

                                for (int counter1 = result.Length - 1; counter1 > counter; counter1--)
                                {
                                    sum += this.U[counter][counter1] * result[counter1];
                                }

                                result[counter] = (result[counter] - sum) / this.U[counter][counter];
                            }
                        }
                    }
                    else
                    {
                        throw new NoSolutionException("Determinant of input matrix equals to zero. No solution could be found.");
                    }
                }
                else
                {
                    throw new IncorrectIncomingDataException("Incorrect size of vector-column is set.");
                }
            }
            else
            {
                throw new IncorrectIncomingDataException("Column vector is not defined.");
            }

            return result;
        }

        private double sum(int height, int i, int j)
        {
            double sum = 0;

            for (int counter = 0; counter <= height; counter++)
            {
                sum += L[i][counter] * U[counter][j];
            }

            return sum;
        }
    }
}
