namespace Homework1_ASP.Exceptions
{
    public class UserExistsException : BusinessException
    {
        public UserExistsException() : base("Նման անունով օգտատեր արդեն գոյություն ունի։") { }
    }
}
