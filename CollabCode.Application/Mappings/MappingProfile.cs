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
            CreateMap<User, NewUserReqDto>().ReverseMap();
            CreateMap<User, NewUserResDto>().ReverseMap();

            CreateMap<Project, NewProjectReqDto>().ReverseMap();

            CreateMap<Project, NewProjectResDto>().ReverseMap();
            CreateMap<Project, ProjectResDto>().ReverseMap();

            CreateMap<ProjectFile, NewFileReqDto>().ReverseMap();
            CreateMap<ProjectFile, NewFileResDto>().ReverseMap();
            
        }
    }
}
