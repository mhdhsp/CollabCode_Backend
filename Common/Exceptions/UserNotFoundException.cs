namespace CollabCode.Common.Exceptions
{
    public class UserNotFoundException:Exception
    {
        public UserNotFoundException(string Message) : base(Message) { }
    }
}
