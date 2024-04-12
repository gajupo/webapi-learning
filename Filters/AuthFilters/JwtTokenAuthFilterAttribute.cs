using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.ComponentModel;
using webapi_learning.Attributes;
using webapi_learning.Authority;

namespace webapi_learning.Filters.AuthFilters
{
    public class JwtTokenAuthFilterAttribute : Attribute,IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue("Authorization", out var token))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var configuration = context.HttpContext.RequestServices.GetService<IConfiguration>();

            var claims = Authenticator.VerifyToken(token, configuration?.GetValue<string>("SecretKey") ?? string.Empty);

            if(claims == null)
            {
                context.Result = new UnauthorizedResult();
            }
            else
            {
                var requiredClaims = context.ActionDescriptor.EndpointMetadata.OfType<RequiredClaimAttribute>().ToList();

                if( requiredClaims != null && !requiredClaims.TrueForAll(_ => claims.Any(c => c.Type.ToLower() == _.ClaimType.ToLower() && 
                c.Value.ToLower() == _.ClaimValue.ToLower())))
                {
                    context.Result = new StatusCodeResult(403);
                }
            }

        }
    }
}
