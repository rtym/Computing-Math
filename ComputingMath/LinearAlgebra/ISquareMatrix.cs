using System;
using System.Collections.Generic;

namespace ComputingMath.LinearAlgebra
{
    public interface ISquareMatrix : IMatrix
    {
        #region Properties Declarations

        double DET
        {
            get;
        }

        #endregion

        #region Mathematical Methods

        double Trace();

        Vector Diag();

        #endregion
    }
}
