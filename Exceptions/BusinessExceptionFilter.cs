using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Homework1_ASP.Exceptions
{
    public class BusinessExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is BusinessException)
            {
                context.Result = new BadRequestObjectResult(new { Message = context.Exception.Message });
                context.ExceptionHandled = true;
            }
        }
    }
}
