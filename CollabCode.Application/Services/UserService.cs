using AutoMapper;
using CollabCode.CollabCode.Application.DTO.ResDto;
using CollabCode.CollabCode.Application.Exceptions;
using CollabCode.CollabCode.Application.Interfaces.Repositories;
using CollabCode.CollabCode.Application.Interfaces.Services;
using CollabCode.CollabCode.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CollabCode.CollabCode.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IGenericRepository<User> _userGRepo;
        private readonly IMapper _mapper;

        public UserService(IGenericRepository<User> Repo, IMapper mapper)
        {
            _userGRepo = Repo;
            _mapper = mapper;
        }



        public async Task<UserProjectsDto?> GetAllUserRooms(int userId)
        {
            var res = await _userGRepo.Query()
                .Where(u => u.Id == userId)
                .Include(u => u.Projects)
                .Include(u => u.MemberShips)
                    .ThenInclude(u => u.Project)
                .FirstOrDefaultAsync();

            if (res == null)
                throw new NotFoundException("No rooms available");

            var dto = new UserProjectsDto
            {
                UserName = res.UserName,
                OwnedRooms = res.Projects.Select(
                    u => new ProjectDto 
                    {
                        Id=u.Id,
                        ProjectName=u.ProjectName,
                        JoinCode=u.JoinCode
                    }
                    ).ToList(),
                JoinedRooms = res.MemberShips.Select(
                    u => new ProjectDto
                    {
                        Id=u.Id,
                        ProjectName=u.Project?.ProjectName
                    }
                    ).ToList()
            };
            
            return dto;
        }


        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _userGRepo.GetByIdAsync(id);
            if (user == null)
                return false;

            await _userGRepo.DeleteAsync(user);
            return true;
        }

    }
}
