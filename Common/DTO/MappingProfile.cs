using AutoMapper;
using CollabCode.Common.DTO.ReqDto;
using CollabCode.Common.DTO.ResDto;
using CollabCode.Common.Model;

namespace CollabCode.Common.DTO
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
