namespace ftrip.io.framework.ExceptionHandling.Exceptions
{
    public class AuthorizationException : HandlableException
    {
        public AuthorizationException()
        {
        }

        public AuthorizationException(string message) :
            base(message)
        {
        }
    }
}