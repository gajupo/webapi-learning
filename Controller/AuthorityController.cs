using Microsoft.AspNetCore.Mvc;
using webapi_learning.Authority;
using webapi_learning.Filters.AuthFilters;

namespace webapi_learning.Controller
{
    [ApiController]
    public class AuthorityController: ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthorityController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("auth")]
        public IActionResult Authenticate([FromBody]AppCredentials appCredentials)
        {
            if (Authenticator.Authenticate(appCredentials.ClientId, appCredentials.Secret))
            {
                var expiredAt = DateTime.UtcNow.AddMinutes(5);
                var secretKey = _configuration.GetValue<string>("SecretKey");

                if (string.IsNullOrWhiteSpace(secretKey)) return BadRequest();

                return Ok(new
                {
                    access_token = Authenticator.CreateToken(appCredentials.ClientId, expiredAt, secretKey),
                    expires_at = DateTime.UtcNow.AddMinutes(5)
                }) ;
            }
            else
            {

                ModelState.AddModelError("Unauthorized", "You are not authorized");
                var validationProblemDetails = new ValidationProblemDetails(ModelState)
                {
                    Status = StatusCodes.Status401Unauthorized,
                };

                return new UnauthorizedObjectResult(validationProblemDetails);
            }
        }
    }
}
