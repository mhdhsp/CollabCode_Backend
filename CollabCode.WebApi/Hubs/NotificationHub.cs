using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CollabCode.API.Hubs
{
    [Authorize]
    public class NotificationHub : Hub
    {
        private readonly ILogger<NotificationHub> _logger;

        public NotificationHub(ILogger<NotificationHub> logger)
        {
            _logger = logger;
        }

        public override async Task OnConnectedAsync()
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            _logger.LogInformation("User connected: ConnectionId={ConnectionId}, UserId={UserId}", Context.ConnectionId, userId ?? "Anonymous");

            await base.OnConnectedAsync();
        }

        public async Task JoinProjectGroup(string projectId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, projectId);
            _logger.LogInformation($"User {Context.UserIdentifier} Joined Group ={projectId}");
        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (exception == null)
                _logger.LogInformation("User disconnected: ConnectionId={ConnectionId}, UserId={UserId}", Context.ConnectionId, userId ?? "Anonymous");
            else
                _logger.LogWarning(exception, " User disconnected with error: ConnectionId={ConnectionId}, UserId={UserId}", Context.ConnectionId, userId ?? "Anonymous");

            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendNotificationToUser(int  userId, string title, string message)
        {
            _logger.LogInformation(" Sending notification to user {UserId}: {Message}", userId, message);

            await Clients.User(Convert.ToString(userId)).SendAsync("ReceiveNotification", new
            {
                Title = title,
                Message = message,
                Time = DateTime.UtcNow
            });
        }

        public async Task SendNotificationToAll(string title, string message)
        {
            _logger.LogInformation(" Broadcasting notification: {Message}", message);

            await Clients.All.SendAsync("ReceiveNotification", new
            {
                Title = title,
                Message = message,
                Time = DateTime.UtcNow
            });
        }
    }
}
