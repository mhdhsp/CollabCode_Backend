using Microsoft.AspNetCore.SignalR;

namespace CollabCode.CollabCode.WebApi.Hubs
{
    public class CustomUserIdProvider:IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connnection)
        {
            return connnection.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value!;
        }
    }
}
