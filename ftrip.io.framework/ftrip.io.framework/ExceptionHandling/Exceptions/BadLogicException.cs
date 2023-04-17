namespace ftrip.io.framework.ExceptionHandling.Exceptions
{
    public class BadLogicException : HandlableException
    {
        public BadLogicException()
        {
        }

        public BadLogicException(string message) :
            base(message)
        {
        }
    }
}