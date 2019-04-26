using System;

namespace ActiveSync.Core.Exceptions
{
    public class InvalidRequestException : ApplicationException
    {
        public InvalidRequestException(string message):base(message)
        {
            
        }

        public InvalidRequestException(string message, Exception innerException):base(message, innerException)
        {
            
        }
    }
}