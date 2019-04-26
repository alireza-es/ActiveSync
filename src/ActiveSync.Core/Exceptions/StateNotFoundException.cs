using System;

namespace ActiveSync.Core.Exceptions
{
    public class StateNotFoundException : ApplicationException
    {
        public StateNotFoundException(string message):base(message)
        {
            
        }

        public StateNotFoundException(string message, Exception innerException):base(message, innerException)
        {
            
        }
    }
}