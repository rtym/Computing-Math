using System;

namespace ComputingMath.LinearAlgebra
{
    public interface IToString
    {
        string ToString();

        string ToString(string format);

        string ToString(IFormatProvider provider);

        string ToString(string format, IFormatProvider provider);
    }
}
