using Microsoft.AspNetCore.SignalR;

public class ProjectHub : Hub
{
    public async Task JoinGroup(string projectId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, projectId);
        Console.WriteLine($"Hub: client {Context.ConnectionId} joined {projectId}");
    }

    public async Task UpdateProject(string projectId)
    {
        try
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, projectId);
            await Clients.Group(projectId)
                .SendAsync("Receive");
            Console.WriteLine($"Hub: broadcast update for {projectId}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ SignalR error: {ex.Message}");
        }
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
    }
}
