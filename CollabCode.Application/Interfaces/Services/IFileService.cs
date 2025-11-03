using CollabCode.CollabCode.Application.DTO.ReqDto;
using CollabCode.CollabCode.Application.DTO.ResDto;
using CollabCode.CollabCode.Domain.Entities;

namespace CollabCode.CollabCode.Application.Interfaces.Services
{
    public interface IFileService
    {
        Task<NewFileResDto> CreateFile(NewFileReqDto item, int userId);
        Task<bool> DeleteFile(int FileId, int userId);
        Task<ProjectFile> SaveFile(SaveFileReqDto dto, int userId);
        Task<bool> UpdateFile(FileUpdateReqDto dto, int userId);
        Task<ProjectFile> Assign(FileAssignReqDto dto, int userId);
        Task<ProjectFile> UnAssign(int FileId, int userId);
    }
}
