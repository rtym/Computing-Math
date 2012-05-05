using System;
using System.Collections.Generic;

namespace ComputingMath.LinearAlgebra
{
    public interface IMatrix : IEnumerable<Vector>, ICloneable, IToString
    {
        #region Properties

        int Count
        {
            get;
        }

        int Height
        {
            get;
        }

        int Width
        {
            get;
        }

        MatrixNormConstants DefaultNorm
        {
            get;
        }

        int Rank
        {
            get;
        }

        double Sum
        {
            get;
        }

        double Max
        {
            get;
        }

        double AbsoluteMax
        {
            get;
        }

        double Min
        {
            get;
        }

        double AbsoluteMin
        {
            get;
        }

        #endregion

        #region Indexation

        Vector this[int index]
        {
            get;
            set;
        }

        #endregion

        #region Norms

        double Norm ();

        double Norm (MatrixNormConstants c);

        #endregion

        #region Mathematical Methods

        void Transpose();

        void Triangulate();

        void swapRows(int i, int j);

        void swapColumns(int i, int j);

        #endregion

    }
}
