using Microsoft.AspNetCore.SignalR;

public class ProjectHub : Hub
{
    private readonly ILogger<ProjectHub> _logger;
   public ProjectHub(ILogger<ProjectHub> Logger)
    {
        _logger = Logger;
    }
    public async Task JoinGroup(string projectId)
    {
        _logger.LogInformation("from hub ,joined");
        await Groups.AddToGroupAsync(Context.ConnectionId, projectId);
        Console.WriteLine($"Hub: client {Context.ConnectionId} joined {projectId}");
    }

    public async Task UpdateProject(string projectId)
    {
        try
        {
            _logger.LogInformation("from hub+ invoked");
            await Groups.AddToGroupAsync(Context.ConnectionId, projectId);
            await Clients.Group(projectId)
                .SendAsync("Receive");
            Console.WriteLine($"Hub: broadcast update for {projectId}");
        }
        catch (Exception ex)
        {

            _logger.LogCritical($"from hub{ex.Message}");
            Console.WriteLine($"❌ SignalR error: {ex.Message}");
        }
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
    }
}
