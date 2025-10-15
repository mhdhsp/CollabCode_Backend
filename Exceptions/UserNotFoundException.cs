namespace CollabCode.Exceptions
{
    public class UserNotFoundException:Exception
    {
        public UserNotFoundException(string Message) : base(Message) { }
    }
}
