using CollabCode.CollabCode.Application.DTO.ReqDto;
using CollabCode.CollabCode.Application.DTO.ResDto;

namespace CollabCode.CollabCode.Application.Interfaces.Services
{
    public interface IFileService
    {
        Task<NewFileResDto> CreateFile(NewFileReqDto item, int userId);
        Task<bool> DeleteFile(int FileId, int userId);
        Task<bool> UpdateFile(FileUpdateReqDto dto, int userId);
    }
}
