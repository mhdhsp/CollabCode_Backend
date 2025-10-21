using AutoMapper;
using CollabCode.CollabCode.Application.DTO.ReqDto;
using CollabCode.CollabCode.Application.DTO.ResDto;
using CollabCode.CollabCode.Domain.Entities;
using CollabCode.CollabCode.Infrastructure.Persistense;
using CollabCode.CollabCode.Application.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using CollabCode.CollabCode.Application.Interfaces.Repositories;
using CollabCode.CollabCode.Application.Interfaces.Services;
using System.Runtime.InteropServices;

namespace CollabCode.CollabCode.Application.Services
{
   
    public class RoomService:IRoomService
    {
        private readonly IGenericRepository<Room> _roomGRepo;
        private readonly AppDbContext _context;
        private readonly IGenericRepository<RoomMember> _roomMemberRepo;
        private readonly IRoomRepo _roomRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<RoomService> _logger;

       public RoomService(IGenericRepository<Room> Repo, IGenericRepository<RoomMember> roomMemberRepo,
           IMapper mapper, IRoomRepo roomRepo,ILogger<RoomService> logger,AppDbContext context)
        {

            _roomMemberRepo = roomMemberRepo;
            _roomGRepo = Repo;
            _roomRepo = roomRepo;
            _mapper = mapper;
            _logger = logger;
            _context = context;

        }

        public async Task<NewRoomResDto> CreateNewRoom(NewRoomReqDto reqDto,int userId)
        {
            var room = _mapper.Map<Room>(reqDto);
            room.OwnerId = userId;
            room.JoinCode = await GenerateJoinCode();
            if (!room.IsPublic && !string.IsNullOrEmpty(reqDto.PassWordHash))
                room.PassWordHash = BCrypt.Net.BCrypt.HashPassword(reqDto.PassWordHash);
            await _roomGRepo.AddAsync(room);
            var ResDto = _mapper.Map<NewRoomResDto>(room);
            return ResDto;
        }

        public async Task<NewRoomResDto> JoinRoom(RoomJoinReqDto reqDto,int userId)
        {
            var existing = await _roomGRepo.FirstOrDefaultAsync(u=>u.JoinCode==reqDto.JoinCode);
            if (existing == null)
                throw new NotFoundException("Room not found");
            if (await _roomMemberRepo.AnyAsync(u => u.UserId == userId && u.RoomId == existing.Id))
                throw new AlreadyExistsException("You alraedy a member of this room");
            if(!existing.IsPublic)
            {
                if (!BCrypt.Net.BCrypt.Verify(reqDto.PassWord,existing.PassWordHash))
                    throw new  MismatchException("Invalid room password");
            }
            var roomMember = new RoomMember
            {
                UserId = userId,
                RoomId = existing.Id,
                IsOwner =  existing.OwnerId == userId
            };
            await _roomMemberRepo.AddAsync(roomMember);
            var res = _mapper.Map<NewRoomResDto>(existing);
            return res;
        }


        public async Task<RoomResDto> EnterRoom(int Roomid,int userId)
        {
            _logger.LogInformation($" from roomservice{Roomid}+{userId}");
            var room = await _roomRepo.GetByIdAsync(Roomid);

            if (room ==null)
                throw new NotFoundException("Room is not found");
            else if(!room.IsActive)
                throw new NotFoundException("Room is not active");

            //if (!room.Members.Any(u => u.id == userId))
            //    throw new UnauthorizedAccessException("You are not a member of this room");

            var res = _mapper.Map<RoomResDto>(room);
            return res;
        }

        public async Task<bool?> LeaveRoom(int userId,int roomId)
        {
            
            if (! await _roomGRepo.AnyAsync(u => u.Id == roomId))
                throw new NotFoundException("No room found");
            if (await _roomGRepo.AnyAsync(u => u.OwnerId== userId))
                throw new NotFoundException("Owner canot leave the room");
            var membership = await _roomMemberRepo.FirstOrDefaultAsync(u => u.UserId == userId && u.RoomId == roomId);
            if (membership == null)
                throw new NotFoundException("You are not member of the room");
            await _roomMemberRepo.DeleteAsync(membership);
            return true;
        }
        public async Task<bool> DestroyRoom(int userId, int roomId)
        {
            var room = await _roomGRepo.GetByIdAsync(roomId);
            if (room == null)
                throw new NotFoundException("Room not found");

            
            if (room.OwnerId != userId)
                throw new UnauthorizedAccessException("Only the room owner can delete this room");

            
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                
                await _roomGRepo.DeleteAsync(room);

                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        private async Task<string> GenerateJoinCode()
        {
            string code;
            do
            {
                code = RandomNumberGenerator.GetInt32(0, 100000).ToString("D6");
            }
            while (await _roomGRepo.AnyAsync(u=>u.JoinCode==code));
            return code;
        }
    }
}
