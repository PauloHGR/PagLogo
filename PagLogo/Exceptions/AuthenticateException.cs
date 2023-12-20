namespace PagLogo.Exceptions
{
    public class AuthenticateException : Exception
    {
        public AuthenticateException() { }

        public AuthenticateException(string message) : base(message) { }
    }
}
