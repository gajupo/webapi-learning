using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using webapi_learning.Models;
using webapi_learning.Models.Core;

namespace webapi_learning.Filters
{
    public class Shirt_ValidateCreateShirtFilterAttributeAsync : ActionFilterAttribute
    {
        private readonly IShirtRepository _shirtRepository;

        public Shirt_ValidateCreateShirtFilterAttributeAsync(IShirtRepository shirtRepository)
        {
            _shirtRepository = shirtRepository;
        }
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.ActionArguments.TryGetValue("shirt", out var value) && value is Shirt shirt)
            {

                Shirt? existShirt = await _shirtRepository.GetShirtByProperties(shirt.Model, shirt.Gender, shirt.Size);
                if (existShirt != null)
                {
                    context.ModelState.AddModelError("Shirt", "Shirt already exist");

                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status400BadRequest
                    };

                    context.Result = new BadRequestObjectResult(problemDetails);

                    return;
                }

                await next();
            }
            else
            {
                context.ModelState.AddModelError("Shirt", "The shirt object is null or not correctly named in the action arguments");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };

                context.Result = new BadRequestObjectResult(problemDetails);
            }
        }
    }
}
