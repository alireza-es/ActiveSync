using System;

namespace ActiveSync.Core.Exceptions
{
    public class InvalidSyncKeyException : ApplicationException
    {
        public InvalidSyncKeyException(string message):base(message)
        {
            
        }

        public InvalidSyncKeyException(string message, Exception innerException):base(message, innerException)
        {
            
        }
    }
}
