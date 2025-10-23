using AutoMapper;
using CollabCode.CollabCode.Application.DTO.ReqDto;
using CollabCode.CollabCode.Application.DTO.ResDto;
using CollabCode.CollabCode.Application.Exceptions;
using CollabCode.CollabCode.Application.Interfaces.Repositories;
using CollabCode.CollabCode.Application.Interfaces.Services;
using CollabCode.CollabCode.Domain.Entities;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace CollabCode.CollabCode.Application.Services
{
    public class FileService:IFileService
    {
        private readonly IGenericRepository<ProjectFile> _fileGRepo;
        private readonly IGenericRepository<Project> _projectGRepo;
        private readonly IMapper _mapper;
        public FileService(
            IGenericRepository<ProjectFile> FileRepo,
            IGenericRepository<Project> ProRepo,
            IMapper Mapper
            )
        {
            _fileGRepo = FileRepo;
            _projectGRepo = ProRepo;
            _mapper = Mapper;
        }

        public async Task<NewFileResDto> CreateFile(NewFileReqDto item,int userId)
        {
            if (!await _projectGRepo.AnyAsync(u => u.Id == item.ProjectId))
                throw new NotFoundException("Such a project not found");
            if (await _projectGRepo.AnyAsync(u => u.Id == item.ProjectId && u.IsDeleted == false))
                throw new NotFoundException("Project is not unavailable");
            if (!await _fileGRepo.AnyAsync(u => u.Project.OwnerId == userId))
                throw new UnauthorizedAccessException("only owner can create a file");

            var file = _mapper.Map<ProjectFile>(item);
            file.CreatedAt = DateTime.Now;
            file.CreatedBy = userId;

            var res= await  _fileGRepo.AddAsync(file);
            return _mapper.Map<NewFileResDto>(res);
        }

        public async Task<bool> DeleteFile(int FileId,int userId)
        {
            if (!await _fileGRepo.AnyAsync(u => u.Id == FileId))
                throw new NotFoundException("Such a file not found");
            if (await _fileGRepo.AnyAsync(u => u.Id==FileId && u.IsDeleted == false))
                throw new NotFoundException("File is not unavailable");
            if (!await _fileGRepo.AnyAsync(u => u.Project.OwnerId == userId))
                throw new UnauthorizedAccessException("only owner can delete  file");

            var item =await  _fileGRepo.GetByIdAsync(FileId);
            if(item==null)
                throw new NotFoundException("Such a file not found");
            await _fileGRepo.DeleteAsync(item);
            return true;
        }

        public async Task<bool> UpdateFile(FileUpdateReqDto dto ,int userId )
        {
            if (!await _fileGRepo.AnyAsync(u => u.Id ==dto.Id))
                throw new NotFoundException("Such a file not found");
            if (await _fileGRepo.AnyAsync(u => u.Id == dto.Id && u.IsDeleted == false))
                throw new NotFoundException("File is not unavailable");

            var item = _mapper.Map<ProjectFile>(dto);
            item.ModifiedAt = DateTime.Now;
            item.ModifiedBy = userId;
            await _fileGRepo.UpdateAsync(item);
            return true;
        }
    }
}
