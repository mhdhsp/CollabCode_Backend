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


        public async Task<Room?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(r=>r.Members)
                  .FirstOrDefaultAsync(r => r.Id == id);
        }
    }
}
