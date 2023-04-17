using System;

namespace ftrip.io.framework.ExceptionHandling.Exceptions
{
    public abstract class HandlableException : Exception
    {
        public HandlableException()
        {
        }

        public HandlableException(string message) :
            base(message)
        {
        }
    }
}