using AutoMapper;
using CollabCode.Common.DTO.ReqDto;
using CollabCode.Common.DTO.ResDto;
using CollabCode.Common.Model;
using CollabCode.Data;
using CollabCode.Common.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.Data.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
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
        private readonly IConfiguration _config;
        public AuthService(AppDbContext context, IMapper mapper,IConfiguration config) 
        { 
            _context = context;
            _mapper = mapper;
            _config = config;
        }

        private string GenerateJwtToken(UserModel user)
        {
            var jwtSettings = _config.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings["Key"]));

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(double.Parse(jwtSettings["ExpireMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public async Task<UserResDto?> Register(UserModel newUser)
        {
              if(_context.Users.Any(u=>u.UserName==newUser.UserName))
                throw new UserAlreadyExistsException("user Alredy Exist ");

                newUser.PassWord = BCrypt.Net.BCrypt.HashPassword(newUser.PassWord);
                await _context.AddAsync(newUser);
                await _context.SaveChangesAsync();
                var res = _mapper.Map<UserResDto>(newUser);
                return res;
               
                
        }


        public async Task<UserResDto> Login(LoginReqDto ReqDto)
        {
            var existing = await _context.Users.FirstOrDefaultAsync(u => u.UserName == ReqDto.UserName);
            if (existing == null)
                throw new UserNotFoundException( $"User with UserName= {ReqDto.UserName} not found");
            if (!BCrypt.Net.BCrypt.Verify(ReqDto.PassWord, existing.PassWord))
                throw new MismatchException($"Invalid Password");
            existing.LastLogin = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            var res = _mapper.Map<UserResDto>(existing);
            res.Token = GenerateJwtToken(existing);
            return res;
        }
    }
}
