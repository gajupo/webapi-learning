using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using webapi_learning.Models;
using webapi_learning.Models.Core;

namespace webapi_learning.Filters
{
    public class Shirt_ValidateCreateShirtFilterAttribute: ActionFilterAttribute
    {
        private readonly IShirtRepository _shirtRepository;

        public Shirt_ValidateCreateShirtFilterAttribute(IShirtRepository shirtRepository)
        {
            _shirtRepository = shirtRepository;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            // var shirtRepository = context.HttpContext.RequestServices.GetRequiredService<IShirtRepository>();


            if (context.ActionArguments["shirt"] is not Shirt shirt)
            {
                context.ModelState.AddModelError("Shirt", "The shirt object is null");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };

                context.Result = new BadRequestObjectResult(problemDetails);
            }
            else
            {
                Shirt? existShirt = _shirtRepository.GetShirtByProperties(shirt.Model, shirt.Gender, shirt.Size).GetAwaiter().GetResult();
                if (existShirt != null)
                {
                    context.ModelState.AddModelError("Shirt", "Shirt already exist");

                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status400BadRequest
                    };

                    context.Result = new BadRequestObjectResult(problemDetails);
                }
            }

        }
    }
}
