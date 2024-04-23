using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace webapi_learning.Filters
{
    public class Shirt_ValidatedPagedImputParamsAttribute: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            if(context.ActionArguments.TryGetValue("pageNumber", out var _pageNumber) && context.ActionArguments.TryGetValue("pageSize", out var _pageSize))
            {
                var pageNumber = _pageNumber as int?;
                var pageSize = _pageSize as int?;

                if (pageNumber == null || pageNumber <= 0 || pageSize == null || pageSize <= 0)
                {
                    context.ModelState.AddModelError("Shirt", "Page Number and Page Size must be grather than 0 if present");
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status400BadRequest,
                    };

                    context.Result = new BadRequestObjectResult(problemDetails);
                }

            }
        }
    }
}
