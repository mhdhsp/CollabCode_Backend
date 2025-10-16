using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CollabCode.MiddleWare
{
    public class UserContextMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<UserContextMiddleWare> _logger; 
        public UserContextMiddleWare(RequestDelegate next, ILogger<UserContextMiddleWare> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
            {
                var token = authHeader.Substring("Bearer ".Length).Trim();

                try
                {
                    var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);

                    var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);


                    if (userIdClaim != null)
                    {
                        context.Items["UserId"] = userIdClaim.Value;
                        _logger.LogInformation($"UserId from middleware{userIdClaim.Value}");
                    }
                }
                catch
                {
                    _logger.LogError("Cannot fing the user Id");
                    throw new Exception("Cannot fing the user Id");

                }
            }

            await _next(context);
        }

    }

    }

