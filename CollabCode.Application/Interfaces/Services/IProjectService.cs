using CollabCode.CollabCode.Application.DTO.ReqDto;
using CollabCode.CollabCode.Application.DTO.ResDto;
using CollabCode.CollabCode.Domain.Entities;

namespace CollabCode.CollabCode.Application.Interfaces.Services
{
    public interface IProjectService
    {
        Task<NewProjectResDto> CreateNewProject(NewProjectReqDto reqDto, int userId);
        Task<NewProjectResDto> JoinProject(ProjectJoinReqDto reqDto, int userId);

        Task<ProjectResDto> EnterProject(int projectid, int userId);
        Task<bool?> LeaveProject(int userId, int projectid);
        Task<bool> DestroyProject(int userId, int projectid);
    }
}
