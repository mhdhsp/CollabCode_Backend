using AutoMapper;
using CollabCode.CollabCode.Application.DTO.ReqDto;
using CollabCode.CollabCode.Application.DTO.ResDto;
using CollabCode.CollabCode.Domain.Model;

namespace CollabCode.CollabCode.Application.DTO
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserModel, UserResDto>().ReverseMap();
            CreateMap<RoomModel, NewRoomReqDto>().ReverseMap();
            CreateMap<RoomModel, RoomResDto>().ReverseMap();
        }
    }
}
