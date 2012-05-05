using System;

namespace ComputingMath
{
    public class NotDiagonallyDominantException : Exception
    {
        public NotDiagonallyDominantException() { }

        public NotDiagonallyDominantException(String message)
            : base(message) { }

        public NotDiagonallyDominantException(String message, Exception innerException)
            : base(message, innerException) { }
    }
}
