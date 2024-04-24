using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using webapi_learning.Models;
using webapi_learning.Models.Core;

namespace webapi_learning.Filters
{
    public class Shirt_ValidateShirtIdFilterAsyncAttribute: ActionFilterAttribute
    {
        private readonly IShirtRepository _shirtRepository;

        public Shirt_ValidateShirtIdFilterAsyncAttribute(IShirtRepository shirtRepository)
        {
            _shirtRepository = shirtRepository;
        }
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            var id = context.ActionArguments["id"] as int?;
            if (id.HasValue)
            {
                if (id.Value < 0)
                {
                    context.ModelState.AddModelError("Shirt", "Shirt Id is Invalid");
                    var problemValidationDetail = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status400BadRequest
                    };

                    context.Result = new BadRequestObjectResult(problemValidationDetail);
                }
                else
                {
                    Shirt? shirt = await _shirtRepository.GetShirtById(id);

                    if (shirt != null)
                    {
                        context.HttpContext.Items["shirt"] = shirt;

                        await next();
                    }
                    else
                    {
                        context.ModelState.AddModelError("Shirt", "Shirt Id does not exists");
                        var validationProblemDetails = new ValidationProblemDetails(context.ModelState)
                        {
                            Status = StatusCodes.Status404NotFound
                        };
                        context.Result = new NotFoundObjectResult(validationProblemDetails);
                    }
                }
            }
        }
    }
}
