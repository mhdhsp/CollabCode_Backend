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

namespace CollabCode.CollabCode.Application.Services
{
   
    public class RoomService:IRoomService
    {
        private readonly IGenericRepository<Room> _roomRepo;
        private readonly IGenericRepository<RoomMembers> _roomMemberRepo;
        private readonly IMapper _mapper;

       public RoomService(IGenericRepository<Room> Repo, IGenericRepository<RoomMembers> roomMemberRepo,IMapper mapper)
        {
            _roomMemberRepo = roomMemberRepo;
            _roomRepo = Repo;
            _mapper = mapper;
        }

        public async Task<RoomResDto> CreateNewRoom(NewRoomReqDto reqDto,int userId)
        {
            var room = _mapper.Map<Room>(reqDto);
            room.OwnerId = userId;
            room.JoinCode = await GenerateJoinCode();
            if (!room.IsPublic && !string.IsNullOrEmpty(reqDto.PassWordHash))
                room.PassWordHash = BCrypt.Net.BCrypt.HashPassword(reqDto.PassWordHash);
            await _roomRepo.AddAsync(room);
            var ResDto = _mapper.Map<RoomResDto>(room);
            return ResDto;
        }

        public async Task<RoomResDto> JoinRoom(RoomJoinReqDto reqDto,int userId)
        {
            var existing = await _roomRepo.FirstOrDefaultAsync(u=>u.JoinCode==reqDto.JoinCode);
            if (existing == null)
                throw new NotFoundException("Room not found");
            if(!existing.IsPublic)
            {
                if (!BCrypt.Net.BCrypt.Verify(reqDto.PassWord,existing.PassWordHash))
                    throw new  MismatchException("Invalid room password");
            }
            var roomMember = new RoomMembers
            {
                UserId = userId,
                RoomId = existing.Id,
                IsOwner =  existing.OwnerId == userId
            };
            await _roomMemberRepo.AddAsync(roomMember);
            var res = _mapper.Map<RoomResDto>(existing);
            return res;
        }

        private async Task<string> GenerateJoinCode()
        {
            string code;
            do
            {
                code = RandomNumberGenerator.GetInt32(0, 100000).ToString("D6");
            }
            while (await _roomRepo.AnyAsync(u=>u.JoinCode==code));
            return code;
        }
    }
}
