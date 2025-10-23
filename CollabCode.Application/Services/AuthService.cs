using AutoMapper;
using CollabCode.CollabCode.Application.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.Data.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CollabCode.CollabCode.Application.DTO.ReqDto;
using CollabCode.CollabCode.Application.DTO.ResDto;
using CollabCode.CollabCode.Domain.Entities;
using CollabCode.CollabCode.Infrastructure.Persistense;
using CollabCode.CollabCode.Application.Interfaces.Repositories;
using CollabCode.CollabCode.Application.Interfaces.Services;
namespace CollabCode.CollabCode.Application.Services
{

    public class AuthService : IAuthService
    {
        private readonly IGenericRepository<User> _repo;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        public AuthService(IGenericRepository<User> Repo, IMapper mapper, IConfiguration config)
        {
            _repo = Repo;
            _mapper = mapper;
            _config = config;
        }

        private string GenerateJwtToken(User user)
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
        public async Task<NewUserResDto?> Register(NewUserReqDto user)
        {
            var newUser = _mapper.Map<User>(user);
            newUser.UserName = newUser?.UserName?.Trim().ToLower();

            if (await _repo.AnyAsync(u => u.UserName.ToLower() == newUser.UserName))
                throw new AlreadyExistsException("UserName UnAvilable ");
            if (await _repo.AnyAsync(u => u.Email == newUser.Email))
                throw new AlreadyExistsException("Email Alredy Exist ");

            newUser.PassWord = BCrypt.Net.BCrypt.HashPassword(newUser.PassWord);
            newUser.CreatedAt = DateTime.Now;
            await _repo.AddAsync(newUser);

            var res = _mapper.Map<NewUserResDto>(newUser);
            
            return res;
        }


        public async Task<NewUserResDto> Login(LoginReqDto ReqDto)
        {
            var existing = await _repo.FirstOrDefaultAsync(u => u.UserName == ReqDto.UserName.ToLower());
            if (existing == null)
                throw new NotFoundException($"User with UserName= {ReqDto.UserName} not found");
            if (!BCrypt.Net.BCrypt.Verify(ReqDto.PassWord, existing.PassWord))
                throw new MismatchException($"Invalid Password");
            var res = _mapper.Map<NewUserResDto>(existing);
            res.Token = GenerateJwtToken(existing);
            return res;
        }
    }
}
