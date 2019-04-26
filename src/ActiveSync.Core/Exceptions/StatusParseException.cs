using System;

namespace ActiveSync.Core.Exceptions
{
    public class StatusParseException : ApplicationException
    {
        public StatusParseException(string message):base(message)
        {
            
        }

        public StatusParseException(string message, Exception innerException):base(message, innerException)
        {
            
        }
    }
}