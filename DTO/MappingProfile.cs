using AutoMapper;
using CollabCode.DTO.ResDto;
using CollabCode.Model;

namespace CollabCode.DTO
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserModel, UserResDto>().ReverseMap();
        }
    }
}
