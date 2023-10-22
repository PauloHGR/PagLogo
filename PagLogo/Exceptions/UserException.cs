using System.Xml.Linq;

namespace PagLogo.Exceptions
{
    public class UserException : Exception
    {
        public UserException() { }
        public UserException(string message) : base(String.Format(message)) { }
    }
}
