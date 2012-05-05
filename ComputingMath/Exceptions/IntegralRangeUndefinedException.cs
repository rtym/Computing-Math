using System;

namespace ComputingMath
{
    public class IntegralRangeUndefinedException : Exception
    {
        public IntegralRangeUndefinedException() { }

        public IntegralRangeUndefinedException(String message)
            : base(message) { }

        public IntegralRangeUndefinedException(String message, Exception innerException)
            : base(message, innerException) { }
    }
}
