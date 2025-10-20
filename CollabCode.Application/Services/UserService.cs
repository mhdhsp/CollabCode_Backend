using AutoMapper;
using CollabCode.CollabCode.Application.DTO.ResDto;
using CollabCode.CollabCode.Application.Exceptions;
using CollabCode.CollabCode.Application.Interfaces.Repositories;
using CollabCode.CollabCode.Application.Interfaces.Services;

namespace CollabCode.CollabCode.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _repo;
        private readonly IMapper _mapper;

        public UserService(IUserRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

      

        public async Task<UserRoomsDto?> GetAllUserRooms(int userId)
        {
            var res= await _repo.GetAllUserRooms(userId);
            if (res == null)
                throw new NotFoundException("No rooms available");
            return res;
        }
    }
}
