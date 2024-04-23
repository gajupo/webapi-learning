using Microsoft.AspNetCore.Mvc;
using webapi_learning.Helpers.Core;
using webapi_learning.Responses;

namespace webapi_learning.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ICustomLogger<ErrorHandlerMiddleware> _logger;
        private readonly IDictionary<string, string[]> errorList = new Dictionary<string, string[]>();
        public ErrorHandlerMiddleware(RequestDelegate next, ICustomLogger<ErrorHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError(ex, "Unauthorized access attempt.");

                errorList.Add("GlobalUnauthorizedAccessException", ["Unauthorized access attempt."]);

                var problemDatails = new ValidationProblemDetails(errorList)
                {
                    Status = StatusCodes.Status401Unauthorized
                };
                await context.Response.WriteAsJsonAsync(problemDatails);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred.");

                errorList.Add("GlobalUnHandledException", ["An error occurred while processing your request."]);

                var problemDatails = new ValidationProblemDetails(errorList)
                {
                    Status = StatusCodes.Status500InternalServerError
                };

                await context.Response.WriteAsJsonAsync(problemDatails);
            }
        }
    }
}
