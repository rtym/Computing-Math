using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ComputingMath;

namespace ComputingMath.LinearAlgebra
{
    public class Vector: IVector, IComparable, IComparable<Vector>, IEquatable<Vector>
    {
        #region Vector Enumeration class

        private class VectorEnumerator : IEnumerator<double>
        {
            private int position = -1;
            private List<double> vector;

            public VectorEnumerator(List<double> vector)
            {
                this.vector = vector;
            }

            public bool MoveNext()
            {
                bool key = this.position < this.vector.Count - 1;

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

            public double Current
            {
                get
                {
                    return this.vector[this.position];
                }
            }

            object IEnumerator.Current
            {
                get
                {
                    return this.vector[this.position];
                }
            }

            public void Dispose() { }
        }

        #endregion

        #region Private Declarations

        private List<double> elements = new List<double>();
        private VectorNormConstants defaultNorm = VectorNormConstants.Euqlid;

        #endregion

        #region Properties Declarations

        public int Length
        {
            get
            {
                return this.elements.Count;
            }
        }

        public VectorNormConstants DefaultNorm
        {
            get
            {
                return this.defaultNorm;
            }
        }

        #endregion

        #region Indexation

        public double this[int index]
        {
            get
            {
                if ((index >= 0) && (index < this.elements.Count))
                {
                    return (this.elements[index]);
                }
                else
                {
                    throw new IndexOutOfRangeException("Accessing to undefined element of vector object");
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
                    throw new IndexOutOfRangeException("Accessing to undefined element of vector object");
                }
            }
        }

        #endregion

        #region Constructors

        public Vector() {}

        public Vector(string elements) :
            this(elements.Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture)
        {
        }

        public Vector(string elements, NumberStyles style, IFormatProvider provider)
        {
            this.elements = Vector.ParseList(elements, ' ', style, provider);
        }

        public Vector(string elements, VectorNormConstants defaultNorm) :
            this(elements)
        {
            this.defaultNorm = defaultNorm;
        }

        public Vector(double[] elements)
        {
            foreach (double element in elements)
            {
                this.elements.Add(element);
            }
        }

        public Vector(double[] elements, VectorNormConstants defaultNorm) :
            this(elements)
        {
            this.defaultNorm = defaultNorm;
        }

        public Vector(List<double> elements)
        {
            this.elements = elements;
        }

        public Vector(List<double> elements, VectorNormConstants defaultNorm) :
            this(elements)
        {
            this.defaultNorm = defaultNorm;
        }

        public Vector(int n, double number)
        {
            if (n > 0)
            {
                for (int counter = 0; counter < n; counter++)
                {
                    this.elements.Add(number);
                }
            }
            else
            {
                throw new VectorFormatErrorException("Vector must have positive dimension.");
            }
        }

        public Vector(int n) :
            this(n, 0)
        {
        }

        public Vector(int n, double number, VectorNormConstants defaultNorm) :
            this(n, number)
        {
            this.defaultNorm = defaultNorm;
        }

        #endregion

        #region Parsers

        private static List<double> ParseList(string elements, char separator, NumberStyles styles, IFormatProvider provider)
        {
            List<double> result = new List<double>();

            if (elements != "")
            {
                String[] exploadedElements = elements.Split(new Char[] { separator });

                foreach (string workingElement in exploadedElements)
                {
                    if (workingElement != "")
                    {
                        string temp = workingElement.Trim();
                        try
                        {
                            result.Add(Double.Parse(temp, styles, provider));
                        }
                        catch (Exception error)
                        {
                            throw new VectorFormatErrorException(error.Message, error);
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

        public static Vector Parse(string elements, char separator)
        {
            return Vector.Parse(elements, separator, NumberStyles.Float, CultureInfo.InvariantCulture);
        }

        public static Vector Parse(string elements)
        {
            return Vector.Parse(elements, NumberStyles.Float, CultureInfo.InvariantCulture);
        }

        public static Vector Parse(string elements, IFormatProvider provider)
        {
            return Vector.Parse(elements, NumberStyles.Float, provider);
        }

        public static Vector Parse(string elements, NumberStyles styles)
        {
            return Vector.Parse(elements, styles, CultureInfo.InvariantCulture.NumberFormat);
        }

        public static Vector Parse(string elements, NumberStyles styles, IFormatProvider provider)
        {
           return Vector.Parse(elements, ' ', styles, provider);
        }

        public static Vector Parse(string elements, char separator, NumberStyles styles, IFormatProvider provider)
        {
            return new Vector(Vector.ParseList(elements, separator, styles, provider));
        }

        #endregion

        #region Explicits

        public static explicit operator double[](Vector vector)
        {
            double[] result = new double[vector.Length];

            for (int counter = 0; counter < vector.Length; counter++)
            {
                result[counter] = vector[counter];
            }

            return result;
        }

        #endregion

        #region IClonable Interface realisation

        public object Clone()
        {
            double[] workingArray = new double[this.elements.Count];

            for (int counter = 0; counter < this.elements.Count; counter++)
            {
                workingArray[counter] = this.elements[counter];
            }

            return new Vector(workingArray, this.defaultNorm);
        }

        #endregion

        #region IEnumerable Intrerface realisation

        public IEnumerator<double> GetEnumerator()
        {
            return new VectorEnumerator(this.elements);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new VectorEnumerator(this.elements);
        }

        #endregion

        #region IComparable interface realisation

        public int CompareTo(Vector vector)
        {
            if (this < vector)
            {
                return -1;
            }
            else if (this == vector)
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
            if (obj is Vector)
            {
                Vector temp = (Vector)obj;
                return this.CompareTo(temp);
            }
            else
            {
                throw new ArgumentException("Argument object is not a Vector");
            }    
        }

        #endregion

        #region IEquatable interface realisation

        public bool Equals(Vector vector)
        {
            bool key = this.Length == vector.Length;

            for (int counter = 0; ((counter < this.Length) && (key)); counter++)
            {
                key = this[counter] == vector[counter];
            }

            return key;
        }

        public override bool Equals(object obj)
        {
            if (obj is Vector)
            {
                Vector temp = (Vector)obj;
                return this.Equals(temp);
            }
            else
            {
                throw new ArgumentException("Argument object is not a Vector");
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

            foreach (double counter in this.elements)
            {
                s += counter.ToString(format, provider) + " ";
            }

            return s;
        }

        #endregion

        #region Simple Operations overloading

        public static bool operator == (Vector left, Vector right)
        {
            return left.Equals(right);
        }

        public static bool operator != (Vector left, Vector right)
        {
            return !left.Equals(right);
        }

        public static bool operator < (Vector left, Vector right)
        {
            if (left.DefaultNorm == right.DefaultNorm)
            {
                return left.Norm() < right.Norm();
            }
            else
            {
                throw new VectorOperationsError("Could not compare two vectors which defined in different norms.");
            }
        }

        public static bool operator > (Vector left, Vector right)
        {
            if (left.DefaultNorm == right.DefaultNorm)
            {
                return left.Norm() > right.Norm();
            }
            else
            {
                throw new VectorOperationsError("Could not compare two vectors which defined in different norms.");
            }
        }

        public static bool operator <= (Vector left, Vector right)
        {
            if (left.DefaultNorm == right.DefaultNorm)
            {
                return left.Norm() <= right.Norm();
            }
            else
            {
                throw new VectorOperationsError("Could not compare two vectors which defined in different norms.");
            }
        }

        public static bool operator >= (Vector left, Vector right)
        {
            if (left.DefaultNorm == right.DefaultNorm)
            {
                return left.Norm() >= right.Norm();
            }
            else
            {
                throw new VectorOperationsError("Could not compare two vectors which defined in different norms.");
            }
        }

        public static Vector operator + (Vector left, Vector right)
        {
            List<double> temp = new List<double>();

            if (left.Length == right.Length)
            {
                try
                {
                    for (int counter = 0; counter < left.Length; counter++)
                    {
                        temp.Add(left[counter] + right[counter]);
                    }
                }
                catch (IndexOutOfRangeException e)
                {
                    throw new VectorOperationsError("Accessing to undefined vector element.", e);
                }
            }
            else
            {
                throw new VectorOperationsError("Error adding vectors. Vectors size are not equal!");
            }

            return new Vector(temp);
        }

        public static Vector operator - (Vector left, Vector right)
        {
            if (left.Length == right.Length)
            {
                Vector temp;

                try
                {
                     temp = left + ((-1) * right);
                }
                catch (Exception e)
                {
                    throw new VectorOperationsError("Error subtracting vectors.", e);
                }

                return temp;
            }
            else
            {
                throw new VectorOperationsError("Error subtracting vectors. Vectors size are not equal!");
            }
        }

        public static Vector operator * (double multiplicator, Vector vector)
        {
            List<double> temp = new List<double>();

            foreach(double counter in vector)
            {
                temp.Add(multiplicator * counter);
            }

            return new Vector(temp);
        }

        public static Vector operator * (Vector vector, double multiplicator)
        {
            List<double> temp = new List<double>();

            foreach (double counter in vector)
            {
                temp.Add(multiplicator * counter);
            }

            return new Vector(temp);
        }

        public static double operator * (Vector left, Vector right)
        {
            if (left.Length == right.Length)
            {
                double sum = 0;

                for (int counter = 0; counter < left.Length; counter++)
                {
                    sum += left[counter] * right[counter];
                }

                return sum;
            }
            else
            {
                throw new VectorOperationsError("Error adding vectors. Vectors size are not equal!");
            }
        }

        public static Vector operator / (Vector vector, double divisioner)
        {
            if (divisioner != 0)
            {
                List<double> temp = new List<double>();

                foreach (double counter in vector)
                {
                    temp.Add(counter / divisioner);
                }

                return new Vector(temp);
            }
            else
            {
                throw new VectorOperationsError("Division by zero!");
            }
        } 

        #endregion

        #region Norms

        public Vector Normalize ()
        {
            List<double> temp = new List<double>();
            double norm = this.Norm ();

            if (norm != 0)
            {
                foreach (double counter in this.elements)
                {
                    temp.Add (counter / norm);
                }
            }
            else
            {
                throw new VectorOperationsError("Normalizing vector with norm which equals to zero");
            }

            return new Vector (temp, this.defaultNorm);
        }

        public double Norm ()
        {
            return this.Norm (this.defaultNorm);
        }

        public double Norm (VectorNormConstants c)
        {
            double result;

            switch (c)
            {
                case VectorNormConstants.Euqlid:
                case VectorNormConstants.Second:
                    result = this.EuqlidNorm();
                    break;
                case VectorNormConstants.First:
                    result = this.FirstNorm();
                    break;
                case VectorNormConstants.Max:
                    result = this.InfNorm();
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

            foreach (double counter in this.elements)
            {
                sum += Math.Pow(Math.Abs(counter), 2);
            }

            return Math.Pow(sum, 0.5);
        }

        private double FirstNorm ()
        {
            double sum = 0;

            foreach (double counter in this.elements)
            {
                sum += Math.Abs(counter);
            }

            return sum;
        }

        private double InfNorm ()
        {
            double max = 0;

            foreach (double counter in this.elements)
            {
                if (Math.Abs(counter) > max)
                {
                    max = Math.Abs(counter);
                }
            }

            return max;
        }

        #endregion

        #region Default Methods

        public double Sum()
        {
            return this.elements.Sum();
        }

        public double Avarage()
        {
            return this.elements.Average();
        }

        public double Max()
        {
            return this.elements.Max();
        }

        public double Min()
        {
            return this.elements.Min();
        }

        public void swap(int i, int j)
        {
            if ((i < this.Length) && (j < this.Length) && (i >= 0) && (j >= 0))
            {
                double temp = this.elements[i];
                this.elements[i] = this.elements[j];
                this.elements[j] = temp;
            }
            else
            {
                throw new IndexOutOfRangeException("Error swaping rows in matrix. Indexes are out of range.");
            }
        }

        #endregion
    }
}
