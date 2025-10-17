using AutoMapper;
using CollabCode.CollabCode.Application.DTO.ReqDto;
using CollabCode.CollabCode.Application.DTO.ResDto;
using CollabCode.CollabCode.Domain.Model;
using CollabCode.CollabCode.Infrastructure.Persistense;
using CollabCode.Common.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace CollabCode.CollabCode.Application.Services
{
    public interface IRoomService
    {
        Task<RoomResDto> CreateNewRoom(NewRoomReqDto reqDto, int userId);
    }
    public class RoomService:IRoomService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

       public RoomService(AppDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<RoomResDto> CreateNewRoom(NewRoomReqDto reqDto,int userId)
        {
            var room = _mapper.Map<RoomModel>(reqDto);
            room.OwnerId = userId;
            room.JoinCode = GenerateJoinCode();
            if (!room.IsPublic && string.IsNullOrEmpty(reqDto.PassWordHash))
                room.PassWordHash = BCrypt.Net.BCrypt.HashPassword(reqDto.PassWordHash);
            await  _context.AddAsync(room);
            await _context.SaveChangesAsync();
            var ResDto = _mapper.Map<RoomResDto>(room);
            return ResDto;
        }

        public async Task<RoomResDto> JoinRoom(RoomJoinReqDto reqDto)
        {
            var existing = await _context.Rooms.FirstOrDefaultAsync(u => u.JoinCode == reqDto.JoinCode);
            if (existing == null)
                throw new NotFoundException("Room not found");

        }
        private string GenerateJoinCode()
        {
            string code;
            var existingCodes = _context.Rooms.Select(u => u.JoinCode).ToHashSet();
            do
            {
                code = RandomNumberGenerator.GetInt32(0, 100000).ToString("D6");
            }
            while (existingCodes.Contains(code));
            return code;
        }
    }
}
