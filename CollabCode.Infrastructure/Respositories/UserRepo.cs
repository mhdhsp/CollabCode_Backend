using CollabCode.CollabCode.Application.DTO.ResDto;
using CollabCode.CollabCode.Application.Interfaces.Repositories;
using CollabCode.CollabCode.Domain.Entities;
using CollabCode.CollabCode.Infrastructure.Persistense;
using Microsoft.EntityFrameworkCore;

namespace CollabCode.CollabCode.Infrastructure.Respositories
{
    public class UserRepo : IUserRepo
    {
        private readonly AppDbContext _context;
        private readonly DbSet<User> _dbSet;

        public UserRepo(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Users;
        }

        public async Task<User?> GetById(int id)
        {
            return await _dbSet
                .Include(u => u.Rooms)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<UserRoomsDto?> GetAllUserRooms(int userId)
        {
            return await _context.Users
                .Where(u => u.Id == userId)
                .Select(u => new UserRoomsDto
                {
                    UserName = u.UserName,
                    OwnedRooms = u.Rooms
                        .Select(r => new RoomDto
                        {
                            Id = r.Id,
                            RoomName = r.RoomName,
                            JoinCode=r.JoinCode
                        }).ToList(),
                    JoinedRooms = u.MemberShips
                        .Select(ms => new RoomDto
                        {
                            Id = ms.Room.Id,
                            RoomName = ms.Room.RoomName,
                        }).ToList()
                })
                .FirstOrDefaultAsync();
        }
    }
}
