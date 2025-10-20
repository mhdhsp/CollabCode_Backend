using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CollabCode.CollabCode.WebApi.MiddleWare
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
            try
            {
                var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();

                if (!string.IsNullOrEmpty(authHeader) && authHeader.Contains("Bearer", StringComparison.OrdinalIgnoreCase))
                {
                    // Remove any "Bearer" prefixes and trim
                    var token = authHeader.Replace("Bearer", "", StringComparison.OrdinalIgnoreCase).Trim();

                    if (!string.IsNullOrEmpty(token))
                    {
                        var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);

                        // Try reading NameIdentifier first, fallback to "UserId" if you used custom claim
                        var userIdClaim = jwtToken.Claims.FirstOrDefault(c =>
                            c.Type == ClaimTypes.NameIdentifier || c.Type == "UserId");

                        if (userIdClaim != null)
                        {
                            context.Items["UserId"] = userIdClaim.Value;
                            _logger.LogInformation($"UserContextMiddleware: UserId extracted - {userIdClaim.Value}");
                        }
                        else
                        {
                            _logger.LogWarning("UserContextMiddleware: UserId claim not found in JWT.");
                        }
                    }
                    else
                    {
                        _logger.LogWarning("UserContextMiddleware: Token is empty after removing Bearer prefix.");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error but do NOT throw; let request continue
                _logger.LogError(ex, "UserContextMiddleware: Failed to extract user from JWT.");
            }

            // Call the next middleware
            await _next(context);
        }
    }
}
