using CollabCode.CollabCode.Application.DTO.ResDto;
using CollabCode.CollabCode.Application.Interfaces.Repositories;
using CollabCode.CollabCode.Domain.Entities;
using CollabCode.CollabCode.Infrastructure.Persistense;
using Microsoft.EntityFrameworkCore;

namespace CollabCode.CollabCode.Infrastructure.Respositories
{
    public class RoomRepo:IRoomRepo
    {
        private readonly AppDbContext _context;
        private readonly DbSet<Room> _dbSet ;
        public RoomRepo(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Rooms;
        }


        public async Task<RoomResDto?> GetByIdAsync(int id)
        {
            return await _dbSet.Where(u => u.Id == id)
                .Select(u => new RoomResDto
                {
                    RoomName = u.RoomName,
                    OwnerId=u.OwnerId,
                    Members = u.Members
                    .Select(s => new MemberDto
                    {
                        id = s.Id,
                        UserName = s.User.UserName,
                        isOwner = s.IsOwner
                    }).ToList()
                }).FirstOrDefaultAsync();
        }
    }
}
