using AutoMapper;
using CollabCode.CollabCode.Application.DTO.ReqDto;
using CollabCode.CollabCode.Application.DTO.ResDto;
using CollabCode.CollabCode.Application.Exceptions;
using CollabCode.CollabCode.Application.Interfaces.Repositories;
using CollabCode.CollabCode.Application.Interfaces.Services;
using CollabCode.CollabCode.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Xml;

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
            if (await _projectGRepo.AnyAsync(u => u.Id == item.ProjectId && u.IsDeleted == true))
                throw new NotFoundException("Project is not unavailable");
            if (!await _fileGRepo.AnyAsync(u => u.Project.OwnerId == userId))
                throw new UnauthorizedAccessException("only owner can create a file");

            var file = _mapper.Map<ProjectFile>(item);
            file.CreatedAt = DateTime.Now;
            file.CreatedBy = userId;
            file.Content = "  // Start coding here ";

            var res= await  _fileGRepo.AddAsync(file);
            return _mapper.Map<NewFileResDto>(res);
        }

        public async Task<bool> DeleteFile(int FileId,int userId)
        {
            if (!await _fileGRepo.AnyAsync(u => u.Id == FileId && !u.IsDeleted))
                throw new NotFoundException("Such a file not found");
           
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
            var item = await _fileGRepo.FirstOrDefaultAsync(u => u.Id == dto.Id && u.ProjectId == dto.ProjectId && !u.IsDeleted);

            if (item == null)
                throw new NotFoundException("Such a file not found");

            item.Content = dto.Content;
            item.FileName = dto.FileName;
            item.ModifiedAt = DateTime.Now;
            item.ModifiedBy = userId;
            await _fileGRepo.UpdateAsync(item);
            return true;
        }

        public async Task<ProjectFile> Assign(FileAssignReqDto dto,int userId)
        {
            var item = await _fileGRepo.GetByIdAsync(dto.FileId);
            if (item == null || item.IsDeleted)
                throw new NotFoundException("Such a file not found");
          
            if(item.Project.OwnerId!= userId)
                throw new UnauthorizedAccessException("Onlu owner can manage files");
            if (item.AssignedTo != null)
                throw new BadHttpRequestException("This file is alraedy assigned to somone");

            item.AssignedTo = dto.TargetUserId;
            item.AssignedAt = DateTime.UtcNow;

            item.ModifiedAt = DateTime.UtcNow;
            item.ModifiedBy = userId;
            await _fileGRepo.UpdateAsync(item);
            return item;
        }

        public async Task<ProjectFile>  UnAssign(int FileId,int userId)
        {
            var item = await _fileGRepo.Query()
                .Where(u => u.Id == FileId)
                .Include(u => u.Project)
                .FirstOrDefaultAsync();
            if (item == null || item.IsDeleted)
                throw new NotFoundException("Such a file not found");

            if (item.Project.OwnerId != userId)
                throw new UnauthorizedAccessException("Onlu owner can manage files");
            item.AssignedTo = null;
            item.AssignedAt = null;

            item.ModifiedAt = DateTime.UtcNow;
            item.ModifiedBy = userId;
            await _fileGRepo.UpdateAsync(item);
            return item;

        }


    }
}
