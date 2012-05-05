using System;

namespace ComputingMath
{
    public class NoSolutionException : Exception
    {
        public NoSolutionException() { }

        public NoSolutionException(String message)
            : base(message) { }

        public NoSolutionException(String message, Exception innerException)
            : base(message, innerException) { }
    }
}
