using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;
using System.Linq;
using System.Threading.Tasks;

namespace CollabCode.CollabCode.WebApi.MiddleWare
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Get the current endpoint
            var endpoint = context.GetEndpoint();

            if (endpoint != null)
            {
                // Check if the endpoint has [Authorize] attribute
                var authorizeMetadata = endpoint.Metadata.GetOrderedMetadata<IAuthorizeData>();
                if (authorizeMetadata.Any())
                {
                    // Endpoint requires authorization
                    if (!context.User.Identity?.IsAuthenticated ?? true)
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsJsonAsync(new
                        {
                            Success = false,
                            Message = "Unauthorized: You need to login to access this resource."
                        });
                        return;
                    }

                    // Optional: Check roles/policies
                    // var roles = authorizeMetadata.Select(a => a.Roles).Where(r => !string.IsNullOrEmpty(r));
                    // if (roles.Any() && !roles.Any(role => context.User.IsInRole(role)))
                    // {
                    //     context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    //     context.Response.ContentType = "application/json";
                    //     await context.Response.WriteAsJsonAsync(new
                    //     {
                    //         Success = false,
                    //         Message = $"Forbidden: You do not have the required role."
                    //     });
                    //     return;
                    // }
                }
            }

            // Continue to next middleware
            await _next(context);
        }
    }
}
