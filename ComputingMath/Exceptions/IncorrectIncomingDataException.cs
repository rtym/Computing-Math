using System;

namespace ComputingMath
{
    public class IncorrectIncomingDataException : Exception
    {
        public IncorrectIncomingDataException() { }

        public IncorrectIncomingDataException(String message)
            : base(message) { }

        public IncorrectIncomingDataException(String message, Exception innerException)
            : base(message, innerException) { }
    }
}
