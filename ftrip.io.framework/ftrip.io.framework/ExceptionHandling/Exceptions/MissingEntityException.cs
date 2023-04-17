namespace ftrip.io.framework.ExceptionHandling.Exceptions
{
    public class MissingEntityException : HandlableException
    {
        public MissingEntityException()
        {
        }

        public MissingEntityException(string message) :
            base(message)
        {
        }
    }
}