using System;

namespace ComputingMath
{
    public class ParsingErrorException : Exception
    {
        public ParsingErrorException() { }

        public ParsingErrorException(String message)
            : base(message) { }

        public ParsingErrorException(String message, Exception innerException)
            : base(message, innerException) { }
    }
}
