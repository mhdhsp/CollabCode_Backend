using CollabCode.CollabCode.Application.Interfaces.Hub;
using Microsoft.AspNetCore.SignalR;

namespace CollabCode.CollabCode.WebApi.Hubs
{
    public class ProjectHub:Hub
    {
        public override async Task  OnConnectedAsync()
        {
           
            Console.WriteLine("connected");
            await base.OnConnectedAsync();
        }
        public async Task UpdateProject(string projectId,string data)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, projectId);
            await Clients.Groups(projectId).SendAsync("Recieve", data);
            Console.WriteLine("from hub");
        }

        public async Task JoinGroup(string projectId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, projectId);
        }
    }
 
    
}
