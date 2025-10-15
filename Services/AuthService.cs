using AutoMapper;
using CollabCode.Data;
using CollabCode.DTO.ReqDto;
using CollabCode.DTO.ResDto;
using CollabCode.Exceptions;
using CollabCode.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Data.Common;
namespace CollabCode.Services
{
    public interface IAuthService
    {
        Task<UserResDto?> Register(UserModel newUser);
        Task<UserResDto> Login(LoginReqDto ReqDto);
    }
    public class AuthService:IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public AuthService(AppDbContext context, IMapper mapper) 
        { 
            _context = context;
            _mapper = mapper;
        }
        public async Task<UserResDto?> Register(UserModel newUser)
        {
                var existing = await _context.Users.FirstOrDefaultAsync(u => u.UserName == newUser.UserName);
                if (existing == null)
                {
                    newUser.PassWord = BCrypt.Net.BCrypt.HashPassword(newUser.PassWord);
                    await _context.AddAsync(newUser);
                    await _context.SaveChangesAsync();
                    var res = _mapper.Map<UserResDto>(newUser);
                    return res;
                }
                throw new UserAlreadyExistsException("user Alredy Exist ");
        }


        public async Task<UserResDto> Login(LoginReqDto ReqDto)
        {
            var existing = await _context.Users.FirstOrDefaultAsync(u => u.UserName == ReqDto.UserName);
            if (existing == null)
                throw new UserNotFoundException( $"User with UserName= {ReqDto.UserName} not found");
            if (!BCrypt.Net.BCrypt.Verify(ReqDto.PassWord, existing.PassWord))
                throw new MismatchException($"Invalid Password");
            var res = _mapper.Map<UserResDto>(existing);
            return res;
        }
    }
}
