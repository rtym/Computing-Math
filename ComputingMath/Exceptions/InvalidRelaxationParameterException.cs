using System;

namespace ComputingMath
{
    public class InvalidRelaxationParameterException : Exception
    {
        public InvalidRelaxationParameterException() { }

        public InvalidRelaxationParameterException(String message)
            : base(message) { }

        public InvalidRelaxationParameterException(String message, Exception innerException)
            : base(message, innerException) { }
    }
}
