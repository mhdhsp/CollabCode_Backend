using AutoMapper;
using CollabCode.CollabCode.Application.DTO.ReqDto;
using CollabCode.CollabCode.Application.DTO.ResDto;
using CollabCode.CollabCode.Domain.Entities;

namespace CollabCode.CollabCode.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserResDto>().ReverseMap();
            CreateMap<Room, NewRoomReqDto>().ReverseMap();
            CreateMap<Room, RoomResDto>().ReverseMap();
        }
    }
}
