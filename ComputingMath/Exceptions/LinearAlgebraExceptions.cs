using System;

namespace ComputingMath
{
    #region Vector Exceptions

    public class VectorFormatErrorException : Exception
    {
        public VectorFormatErrorException() { }

        public VectorFormatErrorException(String message)
            : base(message) { }

        public VectorFormatErrorException(String message, Exception innerException)
            : base(message, innerException) { }
    }

    public class VectorOperationsError : Exception
    {
        public VectorOperationsError() { }

        public VectorOperationsError(String message)
            : base(message) { }

        public VectorOperationsError(String message, Exception innerException)
            : base(message, innerException) { }
    }

    #endregion

    #region Matrix Exceptions

    public class MatrixOperationsError : Exception
    {
        public MatrixOperationsError() { }

        public MatrixOperationsError(String message)
            : base(message) { }

        public MatrixOperationsError(String message, Exception innerException)
            : base(message, innerException) { }
    }
    public class IncorrectMatrixSizeError : Exception
    {
        public IncorrectMatrixSizeError() { }

        public IncorrectMatrixSizeError(String message)
            : base(message) { }

        public IncorrectMatrixSizeError(String message, Exception innerException)
            : base(message, innerException) { }
    }

    public class MatrixFormatErrorException : Exception
    {
        public MatrixFormatErrorException() { }

        public MatrixFormatErrorException(String message)
            : base(message) { }

        public MatrixFormatErrorException(String message, Exception innerException)
            : base(message, innerException) { }
    }

    #endregion

    #region Explicit Exceptions

    public class ExplicitException : Exception
    {
        public ExplicitException() { }

        public ExplicitException(String message)
            : base(message) { }

        public ExplicitException(String message, Exception innerException)
            : base(message, innerException) { }
    }

    public class VectorExplicitException : ExplicitException
    {
        public VectorExplicitException() { }

        public VectorExplicitException(String message)
            : base(message) { }

        public VectorExplicitException(String message, Exception innerException)
            : base(message, innerException) { }
    }

    public class MatrixExplicitException : ExplicitException
    {
        public MatrixExplicitException() { }

        public MatrixExplicitException(String message)
            : base(message) { }

        public MatrixExplicitException(String message, Exception innerException)
            : base(message, innerException) { }
    }

    #endregion
}
