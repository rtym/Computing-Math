using System;

namespace ComputingMath
{
    public class IncorrectIntegrationMethodException : Exception
    {
        public IncorrectIntegrationMethodException() { }

        public IncorrectIntegrationMethodException(String message)
            : base(message) { }

        public IncorrectIntegrationMethodException(String message, Exception innerException)
            : base(message, innerException) { }
    }
}
