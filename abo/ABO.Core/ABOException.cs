using System;

namespace ABO.Core
{
    [Serializable]
    public class ABOException : Exception
    {
        public AboExceptionType ExceptionType { get;set; }
        public ABOException() : base() { }

        public ABOException(string message) : base(message)
        {
            ExceptionType = AboExceptionType.Error;
        }

        public ABOException(string message, AboExceptionType exceptionType) : base(message)
        {
            ExceptionType = exceptionType;
        }

        public ABOException(string message, Exception innerException) : base(message, innerException) { }
    }
}
