namespace ftrip.io.framework.ExceptionHandling.Exceptions
{
    public class ForbiddenException : HandlableException
    {
        public ForbiddenException()
        {
        }

        public ForbiddenException(string message) :
            base(message)
        {
        }
    }
}