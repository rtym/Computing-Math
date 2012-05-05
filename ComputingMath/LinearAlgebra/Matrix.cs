using System;
using System.Collections.Generic;
using System.Collections;
using System.Globalization;
using System.Linq;
using ComputingMath;

namespace ComputingMath.LinearAlgebra
{
    public class Matrix : IMatrix, IComparable, IComparable<Matrix>, IEquatable<Matrix>
    {
        #region Matrix Enumeration class

        private class MatrixEnumerator : IEnumerator<Vector>
        {
            #region Private declarations

            private int position = -1;
            private List<Vector> matrix;

            #endregion

            public MatrixEnumerator(List<Vector> matrix)
            {
                this.matrix = matrix;
            }

            public bool MoveNext()
            {
                bool key = this.position < this.matrix.Count - 1;

                if (key)
                {
                    this.position++;
                }

                return key;
            }

            public void Reset()
            {
                this.position = -1;
            }

            public Vector Current
            {
                get
                {
                    return this.matrix[this.position];
                }
            }

            object IEnumerator.Current
            {
                get
                {
                    return this.matrix[this.position];
                }
            }

            public void Dispose() { }
        }

        #endregion

        #region Private Declarations

        private int width = 0;

        #endregion

        #region Protected Declarations

        protected List<Vector> elements = new List<Vector>();
        protected MatrixNormConstants defaultNorm = MatrixNormConstants.Euqlid;
        protected double eps = 1e-8;
        
        #endregion

        #region Properties Declarations

        public int Count
        {
            get
            {
                return this.elements.Count;
            }
        }

        public int Height
        {
            get
            {
                return this.elements.Count;
            }
        }

        public int Width
        {
            get
            {
                return this.width;
            }
        }

        public MatrixNormConstants DefaultNorm
        {
            get
            {
                return this.defaultNorm;
            }
        }

        public int Rank
        {
            get
            {
                int result = 0;
                Matrix temp = this.Clone() as Matrix;
                temp.Triangulate();

                foreach (Vector counter in temp)
                {
                    if (counter.Norm() != 0)
                    {
                        result++;
                    }
                }

                return result;
            }
        }

        public double Sum
        {
            get
            {
                double sum = 0;

                foreach (Vector counter in this.elements)
                {
                    sum += counter.Sum();
                }

                return sum;
            }
        }

        public double Max
        {
            get
            {
                double max = this.elements[0].Max();

                for (int counter = 1; counter < this.elements.Count; counter++)
                {
                    if (this.elements[counter].Max() > max)
                    {
                        max = this.elements[counter].Max();
                    }
                }

                return max;
            }
        }

        public double AbsoluteMax
        {
            get
            {
                double max = 0;
                double candidate;

                foreach (Vector vector in this.elements)
                {
                    foreach (double counter in vector)
                    {
                        candidate = Math.Abs(counter);
                        if (candidate > max)
                        {
                            max = candidate;
                        }
                    }
                }

                return max;
            }
        }

        public double Min
        {
            get
            {
                double min = this.elements[0].Min();

                for (int counter = 1; counter < this.elements.Count; counter++)
                {
                    if (this.elements[counter].Min() < min)
                    {
                        min = this.elements[counter].Min();
                    }
                }

                return min;
            }
        }

        public double AbsoluteMin
        {
            get
            {
                double min = Math.Abs(this.elements[0][0]);
                double candidate;

                foreach (Vector vector in this.elements)
                {
                    foreach (double counter in vector)
                    {
                        candidate = Math.Abs(counter);
                        if (candidate < min)
                        {
                            min = candidate;
                        }
                    }
                }

                return min;
            }
        }

        #endregion

        #region Indexation

        public Vector this[int index]
        {
            get
            {
                if ((index >= 0) && (index < this.elements.Count))
                {
                    return (this.elements[index]);
                }
                else
                {
                    throw new IndexOutOfRangeException("Accessing to undefined element of matrix object");
                }
            }

            set
            {
                if ((index >= 0) && (index < this.elements.Count))
                {
                    this.elements[index] = value;
                }
                else
                {
                    throw new IndexOutOfRangeException("Accessing to undefined element of matrix object");
                }
            }
        }

        #endregion

        #region Constructors

        public Matrix() {}
        
        public Matrix(string elements) :
            this(elements.Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture)
        {
        }

        public Matrix(string elements, NumberStyles style, IFormatProvider provider)
        {
            this.elements = Matrix.ParseList(elements, ' ', style, provider);

            int width = this.elements[0].Length;
            bool key = true;

            for (int counter = 1; (counter < this.elements.Count) && (key); counter++)
            {
                key = width == this.elements[counter].Length;
            }

            if (key)
            {
                this.width = width;
            }
            else
            {
                throw new IncorrectMatrixSizeError("Rows of matrix has different lenghts.");
            }
        }

        public Matrix(string elements, MatrixNormConstants defaultNorm) :
            this(elements)
        {
            this.defaultNorm = defaultNorm;
        }

        public Matrix(double[][] elements)
        {
            this.width = elements[0].Length;

            foreach (double[] element in elements)
            {
                if (this.width != element.Length)
                {
                    throw new IncorrectMatrixSizeError("Incorrect size of vectors pushed in matrix.");
                }
                else
                {
                    this.elements.Add(new Vector(element));
                }
            }
        }

        public Matrix(double[][] elements, MatrixNormConstants defaultNorm) :
            this(elements)
        {
            this.defaultNorm = defaultNorm;
        }

        public Matrix(Vector[] elements)
        {
            this.width = elements[0].Length;

            foreach (Vector element in elements)
            {
                this.elements.Add(element.Clone() as Vector);

                if (this.width != element.Length)
                {
                    throw new IncorrectMatrixSizeError("Incorrect size of vectors pushed in matrix.");
                }
            }
        }

        public Matrix(Vector[] elements, MatrixNormConstants defaultNorm) :
            this(elements)
        {
            this.defaultNorm = defaultNorm;
        }

        public Matrix(List<Vector> elements)
        {
            this.width = elements[0].Length;

            foreach (Vector counter in elements)
            {
                if (this.width != counter.Length)
                {
                    throw new IncorrectMatrixSizeError("Incorrect size of vectors pushed in matrix.");
                }
                else
                {
                    this.elements.Add(counter.Clone() as Vector);
                }
            }
        }

        public Matrix(List<Vector> elements, MatrixNormConstants defaultNorm) :
            this(elements)
        {
            this.defaultNorm = defaultNorm;
        }

        public Matrix(int height, int width) :
            this(height, width, 0)
        {
        }

        public Matrix(int height, int width, double number)
        {
            if ((height > 0) && (width > 0))
            {
                this.width = width;

                for (int counter = 0; counter < height; counter++)
                {
                    this.elements.Add(new Vector(width, number));
                }
            }
            else
            {
                throw new IncorrectMatrixSizeError("Size of matrix could not be equal to zero.");
            }
        }

        public Matrix(int n, int m, double number, MatrixNormConstants defaultNorm) :
            this(n, m, number)
        {
            this.defaultNorm = defaultNorm;
        }

        #endregion      

        #region Parsers

        private static List<Vector> ParseList(string elements, char separator, NumberStyles styles, IFormatProvider provider)
        {
            List<Vector> result = new List<Vector>();

            if (elements != "")
            {
                string[] exploadedElements = elements.Split(new Char[] { '\n' });

                foreach (string workingElement in exploadedElements)
                {
                    if (workingElement != "")
                    {
                        try
                        {
                            result.Add(Vector.Parse(workingElement.Trim(), separator, styles, provider));
                        }
                        catch (VectorFormatErrorException e)
                        {
                            throw new MatrixFormatErrorException("Error Parsing matrix.", e);
                        }
                    }
                }

                return result;
            }
            else
            {
                throw new VectorFormatErrorException("Error parsing empty string.");
            }
        }

        public static Matrix Parse(string elements, char separator)
        {
            return Matrix.Parse(elements, separator, NumberStyles.Float, CultureInfo.InvariantCulture);
        }

        public static Matrix Parse(string elements)
        {
            return Matrix.Parse(elements, NumberStyles.Float, CultureInfo.InvariantCulture);
        }

        public static Matrix Parse(string elements, IFormatProvider provider)
        {
            return Matrix.Parse(elements, NumberStyles.Float, provider);
        }

        public static Matrix Parse(string elements, NumberStyles styles)
        {
            return Matrix.Parse(elements, styles, CultureInfo.InvariantCulture.NumberFormat);
        }

        public static Matrix Parse(string elements, NumberStyles styles, IFormatProvider provider)
        {
            return Matrix.Parse(elements, ' ', styles, provider);
        }

        public static Matrix Parse(string elements, char separator, NumberStyles styles, IFormatProvider provider)
        {
            return new Matrix(Matrix.ParseList(elements, separator, styles, provider));
        }

        #endregion

        #region Explicits

        public static explicit operator double[][](Matrix matrix)
        {
            double[][] result = new double[matrix.Width][];

            for (int counter = 0; counter < matrix.Width; counter++)
            {
                result[counter] = (double[])matrix[counter];
            }

            return result;
        }

        #endregion

        #region IClonable Interface realisation

        public virtual object Clone()
        {
            return new Matrix(this.elements, this.defaultNorm);
        }

        #endregion

        #region IEnumerable Intrerface realisation

        public IEnumerator<Vector> GetEnumerator()
        {
            return new MatrixEnumerator(this.elements);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new MatrixEnumerator(this.elements);
        }

        #endregion

        #region IComparable interface realisation

        public int CompareTo(Matrix matrix)
        {
            if (this < matrix)
            {
                return -1;
            }
            else if (this == matrix)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }

        public int CompareTo(object obj)
        {
            if (obj is Matrix)
            {
                Matrix temp = (Matrix)obj;
                return this.CompareTo(temp);
            }
            else
            {
                throw new ArgumentException("Argument object is not a Matrix");
            }
        }

        #endregion

        #region IEquatable interface realisation

        public bool Equals(Matrix matrix)
        {
            bool key = (this.Width == matrix.Width) && (this.Height == matrix.Height);

            for (int counter = 0; ((counter < this.Count) && (key)); counter++)
            {
                key = this[counter] == matrix[counter];
            }

            return key;
        }

        public override bool Equals(object obj)
        {
            if (obj is Matrix)
            {
                Matrix temp = (Matrix)obj;
                return this.Equals(temp);
            }
            else
            {
                throw new ArgumentException("Argument object is not a Matrix");
            }
        }

        public override int GetHashCode()
        {
            return this.elements.GetHashCode();
        }

        #endregion

        #region IToString interface realisation

        public override string ToString()
        {
            return this.ToString("", CultureInfo.InvariantCulture);
        }

        public string ToString(string format)
        {
            return this.ToString(format, CultureInfo.InvariantCulture);
        }

        public string ToString(IFormatProvider provider)
        {
            return this.ToString("", provider);
        }

        public string ToString(string format, IFormatProvider provider)
        {
            string s = "";

            foreach (Vector counter in this.elements)
            {
                s += counter.ToString(format, provider) + '\n';
            }

            return s;
        }

        #endregion

        #region Simple Operations overloading

        public static bool operator == (Matrix left, Matrix right)
        {
            return left.Equals(right);
        }

        public static bool operator < (Matrix left, Matrix right)
        {
            if (left.DefaultNorm == right.DefaultNorm)
            {
                return left.Norm() < right.Norm();
            }
            else
            {
                throw new MatrixOperationsError("Could not compare two matrice which defined in different norms.");
            }
        }

        public static bool operator > (Matrix left, Matrix right)
        {
            if (left.DefaultNorm == right.DefaultNorm)
            {
                return left.Norm() > right.Norm();
            }
            else
            {
                throw new MatrixOperationsError("Could not compare two matrice which defined in different norms.");
            }
        }

        public static bool operator >= (Matrix left, Matrix right)
        {
            if (left.DefaultNorm == right.DefaultNorm)
            {
                return left.Norm() >= right.Norm();
            }
            else
            {
                throw new MatrixOperationsError("Could not compare two matrice which defined in different norms.");
            }
        }

        public static bool operator <= (Matrix left, Matrix right)
        {
            if (left.DefaultNorm == right.DefaultNorm)
            {
                return left.Norm() <= right.Norm();
            }
            else
            {
                throw new MatrixOperationsError("Could not compare two matrice which defined in different norms.");
            }
        }

        public static bool operator != (Matrix left, Matrix right)
        {
            return !left.Equals(right);
        }

        public static Matrix operator + (Matrix left, Matrix right)
        {
            return new Matrix(addLists(left, right));   
        }

        public static Matrix operator - (Matrix left, Matrix right)
        {
            if ((left.Width == right.Width) && (left.Height == right.Height))
            {
                Matrix temp;

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

        public static Matrix operator * (double multiplicator, Matrix matrix)
        {
            List<Vector> temp = new List<Vector>();

            foreach (Vector counter in matrix)
            {
                temp.Add(multiplicator * counter);
            }

            return new Matrix(temp);
        }

        public static Matrix operator * (Matrix matrix, double multiplicator)
        {
            List<Vector> temp = new List<Vector>();

            foreach (Vector counter in matrix)
            {
                temp.Add(multiplicator * counter);
            }

            return new Matrix(temp);
        }

        public static Matrix operator * (Matrix left, Matrix right)
        {
            return new Matrix(multList(left, right));
        }

        public static Vector operator * (Matrix left, Vector right)
        {
            if (left.Width == right.Length)
            {
                Vector result = new Vector(left.Height);

                for (int counter = 0; counter < result.Length; counter++)
                {
                    result[counter] = left[counter] * right;
                }

                return result;
            }
            else
            {
                throw new MatrixOperationsError("Matrix width and vector Length is not equal.");
            }
        }

        public static Vector operator * (Vector left, Matrix right)
        {
            if (left.Length == right.Height)
            {
                Vector result = new Vector(right.Width);

                for (int counter = 0; counter < result.Length; counter++)
                {
                    for (int counter1 = 0; counter1 < right.Height; counter1++ )
                    {
                        result[counter] += left[counter1] * right[counter1][counter];
                    }
                }

                return result;
            }
            else
            {
                throw new MatrixOperationsError("Matrix width and vector Length is not equal.");
            }
        }

        public static Matrix operator / (Matrix matrix, double divisioner)
        {
            if (divisioner != 0)
            {
                List<Vector> temp = new List<Vector>();

                foreach (Vector counter in matrix)
                {
                    temp.Add(counter / divisioner);
                }

                return new Matrix(temp);
            }
            else
            {
                throw new MatrixOperationsError("Division by zero!");
            }
        }

        #endregion

        #region Norms

        public double Norm ()
        {
            return this.Norm (this.defaultNorm);
        }

        public double Norm (MatrixNormConstants c)
        {
            double result;

            switch (c)
            {
                case MatrixNormConstants.Euqlid:
                    result = this.EuqlidNorm ();
                    break;
                case MatrixNormConstants.lNorm:
                    result = this.lNorm ();
                    break;
                case MatrixNormConstants.mNorm:
                    result = this.mNorm ();
                    break;
                default:
                    result = 0;
                    break;
            }

            return result;
        }

        private double EuqlidNorm ()
        {
            double sum = 0;

            foreach (Vector vector in this.elements)
            {
                foreach (double element in vector)
                {
                    sum += Math.Pow (Math.Abs (element), 2);
                }
            }

            return Math.Pow (sum, 0.5);
        }

        private double lNorm ()
        {
            List<double> rowSums = new List<double>();

            for (int counter2 = 0; counter2 < this.Width; counter2++)
            {
                double sum = 0;

                for (int counter1 = 0; counter1 < this.Count; counter1++)
                {
                    sum += Math.Abs (this.elements[counter1][counter2]);
                }

                rowSums.Add (sum);
            }

            return rowSums.Max();
        }

        private double mNorm ()
        {
            List<double> rowSums = new List<double> ();

            foreach (Vector vector in this.elements)
            {
                double sum = 0;

                foreach (double counter in vector)
                {
                    sum += Math.Abs (counter);
                }

                rowSums.Add (sum);
            }

            return rowSums.Max ();
        }
  
        #endregion

        #region Mathematical Methods

        public void Transpose()
        {
            Matrix temp = this.Clone() as Matrix;

            for (int counter1 = 0; counter1 < this.elements.Count; counter1++)
            {
                for (int counter2 = 0; counter2 < this[counter1].Length;  counter2++)
                {
                    this[counter2][counter1] = temp[counter1][counter2];
                }
            }
        }

        public void Triangulate()
        {
            for (int counter = 0; ((counter < this.Height) && (counter < this.Width)); counter++)
            {
                int result;

                if (this[counter][counter] == 0)
                {
                    result = counter;

                    bool key = false;

                    for (int counter1 = counter + 1; ((counter1 < this.Height) && (!key)); counter1++)
                    {
                        key = this[counter1][counter] != 0;

                        if (key)
                        {
                            result = counter1;
                        }
                    }
                    if (result != counter)
                    {
                        this.swapRows(counter, result);
                    }
                }

                for (int counter1 = counter + 1; counter1 < this.Height; counter1++)
                {
                    if (this[counter][counter] != 0)
                    {
                        double divisor = this[counter1][counter] / this[counter][counter];
                        this[counter1] -= (divisor * this[counter]);
                    }
                }
            }
        }

        public void swapRows(int i, int j)
        {
            if ((i < this.Count) && (j < this.Count) && (i >= 0) && (j >= 0))
            {
                Vector tempVector = this.elements[i];
                this.elements[i] = this.elements[j];
                this.elements[j] = tempVector;
            }
            else
            {
                throw new IndexOutOfRangeException("Error swaping rows in matrix. Indexes are out of range.");
            }
        }

        public void swapColumns(int i, int j)
        {
            if ((i < this.Width) && (j < this.Width) && (i >= 0) && (j >= 0))
            {
                double temp;

                for (int counter = 0; counter < this.Height; counter++)
                {
                    temp = this[counter][i];
                    this[counter][i] = this[counter][j];
                    this[counter][j] = temp;
                }                
            }
            else
            {
                throw new IndexOutOfRangeException("Error swaping rows in matrix. Indexes are out of range.");
            }
        }

        #endregion

        #region Private static methods

        protected static List<Vector> addLists(Matrix left, Matrix right)
        {
            if ((left.Width == right.Width) && (left.Height == right.Height))
            {
                List<Vector> result = new List<Vector>();

                try
                {
                    for (int counter = 0; counter < left.Count; counter++)
                    {
                        result.Add(left[counter] + right[counter]);
                    }
                }
                catch (IndexOutOfRangeException e)
                {
                    throw new MatrixOperationsError("Accessing to undefined matrix element.", e);
                }

                return result;
            }
            else
            {
                throw new IncorrectMatrixSizeError("Error adding two matices with different sizes.");
            }
        }

        protected static List<Vector> multList(Matrix left, Matrix right)
        {
            if (left.Width == right.Height)
            {
                List<Vector> temp = new List<Vector>();

                for (int counter = 0; counter < left.Height; counter++)
                {
                    temp.Add(new Vector(left.Width));
                }

                for (int counter1 = 0; counter1 < left.Height; counter1++)
                {
                    for (int counter2 = 0; counter2 < right.Width; counter2++)
                    {
                        for (int counter = 0; counter < left.Width; counter++)
                        {
                            temp[counter1][counter2] += left[counter1][counter] * right[counter][counter2];
                        }
                    }
                }

                return temp;
            }
            else
            {
                throw new IncorrectMatrixSizeError("Multiplicating two matices with different sizes.");
            }
        }

        #endregion
    }
}
