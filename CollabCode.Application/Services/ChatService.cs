using AutoMapper;
using CollabCode.CollabCode.Application.DTO.ReqDto;
using CollabCode.CollabCode.Application.DTO.ResDto;
using CollabCode.CollabCode.Application.Exceptions;
using CollabCode.CollabCode.Application.Interfaces.Repositories;
using CollabCode.CollabCode.Domain.Entities;
using CollabCode.API.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using CollabCode.CollabCode.Application.Interfaces.Services;

namespace CollabCode.CollabCode.Application.Services
{
    public class ChatService : IChatService
    {
        private readonly IGenericRepository<Chat> _repo;
        private readonly IMapper _mapper;
        private readonly IHubContext<ChatHub> _hubContext;

        public ChatService(
            IGenericRepository<Chat> repo,
            IMapper mapper,
            IHubContext<ChatHub> hubContext)
        {
            _repo = repo;
            _mapper = mapper;
            _hubContext = hubContext;
        }

        public async Task<List<ChatResDto>> GetAllMsg(int projectId, int userId, int limit = 10)
        {
            var query = _repo.Query()
                .Where(u => u.ProjectId == projectId && !u.IsDeleted)
                .Include(u => u.Project)
                .Include(u => u.User)
                .OrderByDescending(u => u.CreatedAt)
                .Take(limit)
                .Select(u => new ChatResDto
                {
                    senderId = u.SenderId,
                    SenderName = u.User.UserName,
                    Content = u.Content,
                    Time = u.CreatedAt,
                    ProjectId = u.ProjectId
                });

            var msg = await query.ToListAsync();
            if (!msg.Any())
                return new List<ChatResDto>();


            return msg;
        }

        public async Task<Chat> AddMsg(ChatReqDto newMsg, int userId)
        {
            var item = _mapper.Map<Chat>(newMsg);
            item.SenderId = userId;
            item.CreatedAt = DateTime.UtcNow;
            item.CreatedBy = userId;
            await _repo.AddAsync(item);

            // Fetch sender name
            var user = await _repo.GetDbContext().Users
                .FirstOrDefaultAsync(u => u.Id == userId);

            var broadcastMsg = new ChatResDto
            {
                senderId = userId,
                SenderName = user?.UserName ?? "Unknown",
                Content = item.Content,
                Time = item.CreatedAt,
                ProjectId = item.ProjectId
            };

            await _hubContext.Clients
                .Group(item.ProjectId.ToString())
                .SendAsync("ReceiveMsg", broadcastMsg);

            Console.WriteLine("Message broadcasted via SignalR");
            return item;
        }
    }
}