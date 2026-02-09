using Microsoft.AspNetCore.Mvc.Filters;

namespace HealthApp.Api.FIlters
{
    public class LoggingActionFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            //Run before the action
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            //Run after the action
        }


    }
}
