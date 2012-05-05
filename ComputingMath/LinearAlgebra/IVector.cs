using System;
using System.Collections.Generic;

namespace ComputingMath.LinearAlgebra
{
    public interface IVector : IEnumerable<double>, ICloneable, IToString
    {
        #region Properties

        int Length
        {
            get;
        }

        VectorNormConstants DefaultNorm
        {
            get;
        }

        #endregion

        #region Indexation

        double this[int index]
        {
            get;
            set;
        }

        #endregion

        #region Norms

        Vector Normalize ();

        double Norm ();

        double Norm (VectorNormConstants c);

        #endregion

        #region Defalt Methods

        double Sum();

        double Avarage();

        double Max();

        double Min();

        void swap(int i, int j);

        #endregion
    }
}
