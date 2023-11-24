namespace PagLogo.Exceptions
{
    public class TransactionException : Exception
    {
        public TransactionException() { }
        public TransactionException(string message) : base(String.Format(message)) { }
    }
}
