namespace Homework1_ASP.Exceptions
{
    public class BusinessException : Exception
    {
        public BusinessException(string message) : base(message) { }
    }
}
