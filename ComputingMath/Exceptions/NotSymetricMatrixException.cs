using System;

namespace ComputingMath
{
    public class NotSymetricMatrixException : Exception
    {
        public NotSymetricMatrixException() { }

        public NotSymetricMatrixException(String message)
            : base(message) { }

        public NotSymetricMatrixException(String message, Exception innerException)
            : base(message, innerException) { }
    }
}
