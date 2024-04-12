using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using webapi_learning.Models.Core;

namespace webapi_learning.Filters.ExceptionFilters
{
    public class Shirt_HandleUpdateExceptionFilterAttribute: ExceptionFilterAttribute
    {
        private readonly IShirtRepository _shirtRepository;

        public Shirt_HandleUpdateExceptionFilterAttribute(IShirtRepository shirtRepository)
        {
            _shirtRepository = shirtRepository;
        }

        public override void OnException(ExceptionContext context)
        { 
            base.OnException(context);

            // var shirtRepository = context.HttpContext.RequestServices.GetRequiredService<IShirtRepository>();

            var id = context.RouteData.Values["id"] as string;

            if (int.TryParse(id, out int shirtId))
            {
                if (!_shirtRepository.Exists(shirtId).GetAwaiter().GetResult())
                {
                    context.ModelState.AddModelError("Shirt", "The shirt does not exist anymore");
                    var problemDatils = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status404NotFound
                    };

                    context.Result = new NotFoundObjectResult(problemDatils);
                }
            }
        }
    }
}
