using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace webapi_learning.Authority
{
    public static class Authenticator
    {
        public static bool Authenticate(string ClientId, string Secret)
        {
            var app = AppRepository.GetApplicationById(ClientId);
            if (app == null) { return false; }

            return (app.ClientId == ClientId && app.Secret == Secret);
        }

        public static string CreateToken(string clientId, DateTime expiresAt, string strSecretKey)
        {
            var app = AppRepository.GetApplicationById(clientId);

            var claims = new List<Claim>
            {
                new Claim("AppName", app?.ApplicationName??string.Empty),
            };

            var scopes = app?.Scope?.Split(",");
            if(scopes != null && scopes.Length > 0)
            {
                foreach (var scope in scopes)
                {
                    claims.Add(new Claim(scope.ToLower(), "true"));

                }
            }

            var secretKey = Encoding.ASCII.GetBytes(strSecretKey);

            var jwt = new JwtSecurityToken(
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256),
                claims: claims,
                expires: expiresAt,
                notBefore: DateTime.UtcNow);


            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        public static IEnumerable<Claim>? VerifyToken(string token, string secretKey)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return null;
            }

            if (token.StartsWith("Bearer"))
            {
                token = token.Substring(6).Trim();
            }

            var _secretKey = Encoding.ASCII.GetBytes(secretKey);
            SecurityToken securityToken;

            try
            {


                var tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(_secretKey),
                    ValidateLifetime = true,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ClockSkew = TimeSpan.Zero,
                },
                out securityToken);

                if (securityToken != null)
                {
                    var tokenObject = tokenHandler.ReadJwtToken(token);

                    return tokenObject.Claims ?? (new List<Claim>());
                }
                else
                    return null;

            }
            catch (SecurityTokenException)
            {

                return null;
            }

        }
    }
}
