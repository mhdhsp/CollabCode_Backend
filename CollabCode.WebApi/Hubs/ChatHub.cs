using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace CollabCode.API.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ILogger<ChatHub> _logger;

        public ChatHub(ILogger<ChatHub> logger)
        {
            _logger = logger;
            Console.WriteLine("Hub started");
        }

        public async Task JoinGroup(string projectId)
        {
            try
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, projectId);
                _logger.LogInformation("Connection {ConnectionId} joined group {ProjectId}", Context.ConnectionId, projectId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error joining group {ProjectId}", projectId);
                throw;
            }
        }

        public async Task LeaveGroup(string projectId)
        {
            try
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, projectId);
                _logger.LogInformation("Connection {ConnectionId} left group {ProjectId}", Context.ConnectionId, projectId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error leaving group {ProjectId}", projectId);
                throw;
            }
        }

        public override async Task OnConnectedAsync()
        {
            _logger.LogInformation("Client connected: {ConnectionId}", Context.ConnectionId);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            if (exception == null)
                _logger.LogInformation("Client disconnected: {ConnectionId}", Context.ConnectionId);
            else
                _logger.LogWarning(exception, "Client disconnected with error: {ConnectionId}", Context.ConnectionId);

            await base.OnDisconnectedAsync(exception);
        }
    }
}