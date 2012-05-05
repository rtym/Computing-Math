using System;
using System.Collections.Generic;
using System.Globalization;

using ComputingMath;
using ComputingMath.SolvingSLAE;

namespace ComputingMath.LinearAlgebra
{
    public class SquareMatrix : Matrix, ISquareMatrix
    {
        #region Private Declarations

        private double detMultiplicator = 1;

        #endregion

        #region Properties Declarations

        public double DET
        {
            get
            {
                double result = 1;
                SquareMatrix temp = this.Clone() as SquareMatrix;
                temp.Triangulate();

                foreach (double counter in temp.Diag())
                {
                    result *= counter;
                }

                return result * this.detMultiplicator;
            }
        }

        public double Cond
        {
            get
            {
                SquareMatrix inverse = this ^ (-1);
                return this.Norm() * inverse.Norm();
            }
        }

        public bool IsSymetric
        {
            get
            {
                bool result = true;

                for (int counter1 = 0; ((counter1 < this.Count) && (result)); counter1++)
                {
                    for (int counter2 = 0; ((counter2 < this.Count) && (result)); counter2++)
                    {
                        result = Math.Abs(this[counter1][counter2] - this[counter2][counter1]) < this.eps;
                    }
                }

                return result;
            }
        }

        public bool IsAsymetric
        {
            get
            {
                bool result = true;

                for (int counter1 = 0; ((counter1 < this.Count) && (result)); counter1++)
                {
                    for (int counter2 = 0; ((counter2 < this.Count) && (result)); counter2++)
                    {
                        result = Math.Abs(this[counter1][counter2] + this[counter2][counter1]) < this.eps;
                    }
                }

                return result;
            }
        }

        public bool IsOrthogonal
        {
            get
            {
                SquareMatrix temp = new SquareMatrix(this.Count);
                SquareMatrix A = this;
                SquareMatrix At = this.Clone() as SquareMatrix;
                At.Transpose();

                for (int counter = 0; counter < temp.Count; counter++)
                {
                    temp[counter][counter] = 1;
                }

                return A * At == temp;
            }
        }

        #endregion

        #region Constructors

        public SquareMatrix()
            : base() { }

        public SquareMatrix(string elements)
            : this(elements.Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture)
        {  
        }

        public SquareMatrix(string elements, NumberStyles style, IFormatProvider provider) :
            base(elements, style, provider)
        {
            if (this.Height != this.Width)
            {
                throw new IncorrectMatrixSizeError("Error parsing matrix. Input matrix is not a square matrix.");
            }
        }

        public SquareMatrix(string elements, MatrixNormConstants defaultNorm)
            : this(elements)
        {
            this.defaultNorm = defaultNorm;
        }

        public SquareMatrix(double[][] elements)
            : base(elements)
        {
            if (this.Height != this.Width)
            {
                throw new IncorrectMatrixSizeError("Error parsing matrix. Input matrix is not a square matrix.");
            }
        }

        public SquareMatrix(double[][] elements, MatrixNormConstants defaultNorm)
            : this(elements)
        {
            this.defaultNorm = defaultNorm;
        }

        public SquareMatrix(Vector[] elements)
            : base(elements)
        {
            if (this.Height != this.Width)
            {
                throw new IncorrectMatrixSizeError("Error parsing matrix. Input matrix is not a square matrix.");
            }
        }

        public SquareMatrix(Vector[] elements, MatrixNormConstants defaultNorm)
            : this(elements)
        {
            this.defaultNorm = defaultNorm;
        }

        public SquareMatrix(List<Vector> elements)
            : base(elements)
        {
            if (this.Height != this.Width)
            {
                throw new IncorrectMatrixSizeError("Error parsing matrix. Input matrix is not a square matrix.");
            }
        }

        public SquareMatrix(List<Vector> elements, MatrixNormConstants defaultNorm)
            : this(elements)
        {
            this.defaultNorm = defaultNorm;
        }

        public SquareMatrix(int n)
            : base(n, n) { }

        public SquareMatrix(int n, MatrixNormConstants defaultNorm)
            : this(n, n)
        {
            this.defaultNorm = defaultNorm;
        }

        public SquareMatrix(int n, double number)
            : base(n, n, number) { }

        public SquareMatrix(int n, double number, MatrixNormConstants defaultNorm)
            : base(n, n, number, defaultNorm) { }

        public SquareMatrix(Matrix M) :
            this(M.Height, 0, M.DefaultNorm)
        {
            if ((M.Height == M.Width) && (M.Height > 0))
            {
                for (int counter = 0; counter < this.Count; counter++)
                {
                    for (int counter1 = 0; counter1 < this.Count; counter1++)
                    {
                        this[counter][counter1] = M[counter][counter1];
                    }
                }
            }
            else
            {
                throw new IncorrectIncomingDataException("Incoming motrix is not squared.");
            }
        }

        #endregion

        #region Simple Operations overloading

        public static SquareMatrix operator * (double multiplicator, SquareMatrix matrix)
        {
            List<Vector> temp = new List<Vector>();

            foreach (Vector counter in matrix)
            {
                temp.Add(multiplicator * counter);
            }

            return new SquareMatrix(temp);
        }

        public static SquareMatrix operator * (SquareMatrix matrix, double multiplicator)
        {
            List<Vector> temp = new List<Vector>();

            foreach (Vector counter in matrix)
            {
                temp.Add(multiplicator * counter);
            }

            return new SquareMatrix(temp);
        }

        public static SquareMatrix operator ++ (SquareMatrix matrix)
        {
            for (int counter = 0; counter < matrix.Count; counter++)
            {
                matrix[counter][counter] += 1;
            }

            return matrix;
        }

        public static SquareMatrix operator -- (SquareMatrix matrix)
        {
            for (int counter = 0; counter < matrix.Count; counter++)
            {
                matrix[counter][counter] -= 1;
            }

            return matrix;
        }

        public static SquareMatrix operator * (SquareMatrix left, SquareMatrix right)
        {
            return new SquareMatrix(multList(left, right));
        }

        public static SquareMatrix operator ^ (SquareMatrix matrix, int power)
        {
            SquareMatrix result = new SquareMatrix(matrix.Count);

            result++;

            if (power > 0)
            {
                for (int counter = 0; counter < power; counter++)
                {
                    result *= matrix;
                }
            }
            else if (power < 0)
                {
                    LUDecomposition matrixLU = new LUDecomposition(matrix);    

                    if (matrixLU.DET != 0)
                    {
                        Vector b = new Vector(matrix.Count);
                        SquareMatrix inverse = new SquareMatrix(matrix.Count);

                        for (int counter = 0; counter < matrix.Count; counter++)
                        {
                            b[counter] = 1;
                            inverse[counter] = matrixLU.Solve(b);
                            b[counter] = 0;
                        }

                        inverse.Transpose();

                        for (int counter = 0; counter < Math.Abs(power); counter++)
                        {
                            result = inverse;
                        }

                    }
                    else
                    {
                        throw new MatrixOperationsError("Couldn't find inverse matrix with DET = 0.");
                    }                    
                }

            return result;
        }

        public static SquareMatrix operator + (SquareMatrix left, SquareMatrix right)
        {
            return new SquareMatrix(addLists(left, right));
        }

        public static SquareMatrix operator - (SquareMatrix left, SquareMatrix right)
        {
            if ((left.Width == right.Width) && (left.Height == right.Height))
            {
                SquareMatrix temp;

                try
                {
                    temp = left + ((-1) * right);
                }
                catch (Exception e)
                {
                    throw new MatrixOperationsError("Error subtracting matices.", e);
                }

                return temp;
            }
            else
            {
                throw new IncorrectMatrixSizeError("Error adding two matices with different sizes.");
            }
        }

        #endregion

        #region IClonable Interface realisation

        public override object Clone()
        {
            return new SquareMatrix(this.elements, this.defaultNorm);
        }

        #endregion

        #region Mathematical Methods

        public double Trace()
        {
            double sum = 0;

            for (int counter = 0; counter < this.Count; counter++)
            {
                sum += this.elements[counter][counter];
            }

            return sum;
        }

        public Vector Diag()
        {
            Vector result = new Vector(this.Count);

            for (int counter = 0; counter < this.Count; counter++)
            {
                result[counter] = this.elements[counter][counter];
            }

            return result;
        }

        #endregion
    }
}
