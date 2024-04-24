using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using webapi_learning.Models.Core;

namespace webapi_learning.Filters.ExceptionFilters
{
    public class Shirt_HandleUpdateExceptionFilterAsyncAttribute: ExceptionFilterAttribute
    {
        private readonly IShirtRepository _shirtRepository;

        public Shirt_HandleUpdateExceptionFilterAsyncAttribute(IShirtRepository shirtRepository)
        {
            _shirtRepository = shirtRepository;
        }

        public override async Task OnExceptionAsync(ExceptionContext context)
        {
            var id = context.RouteData.Values["id"] as string;

            if (int.TryParse(id, out int shirtId))
            {
                var shirt = await _shirtRepository.Exists(shirtId);
                if (!shirt)
                {
                    context.ModelState.AddModelError("Shirt", "The shirt does not exist anymore");
                    var problemDatils = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status404NotFound
                    };

                    context.Result = new NotFoundObjectResult(problemDatils);
                }
            }
            else
            {
                context.ModelState.AddModelError("Shirt", "Failed to parse the given id, please provide a valid integer");
                var problemDatils = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };

                context.Result = new NotFoundObjectResult(problemDatils);
            }
        }
    }
}
