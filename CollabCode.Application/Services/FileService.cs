using AutoMapper;
using CollabCode.API.Hubs;
using CollabCode.CollabCode.Application.DTO.ReqDto;
using CollabCode.CollabCode.Application.DTO.ResDto;
using CollabCode.CollabCode.Application.Exceptions;
using CollabCode.CollabCode.Application.Interfaces.Repositories;
using CollabCode.CollabCode.Application.Interfaces.Services;
using CollabCode.CollabCode.Domain.Entities;
using CollabCode.CollabCode.Domain.Enums;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Xml;

namespace CollabCode.CollabCode.Application.Services
{
    public class FileService : IFileService
    {
        private readonly IGenericRepository<ProjectFile> _fileGRepo;
        private readonly IGenericRepository<Project> _projectGRepo;
        private readonly IGenericRepository<FileVersion> _VersionGRepo;
        private readonly IMapper _mapper;
        private readonly IHubContext<NotificationHub>  _notify;
        public FileService(
            IGenericRepository<ProjectFile> FileRepo,
            IGenericRepository<Project> ProRepo,
            IGenericRepository<FileVersion> versionRepo,
            IGenericRepository<User> userRepo,
            IMapper Mapper,
            IHubContext<NotificationHub> noyify
            )
        {
            _fileGRepo = FileRepo;
            _projectGRepo = ProRepo;
            _mapper = Mapper;
            _VersionGRepo = versionRepo;
            _notify = noyify;
        }

        public async Task<NewFileResDto> CreateFile(NewFileReqDto item, int userId)
        {
            if (!await _projectGRepo.AnyAsync(u => u.Id == item.ProjectId))
                throw new NotFoundException("Such a project not found");
            if (await _projectGRepo.AnyAsync(u => u.Id == item.ProjectId && u.IsDeleted == true))
                throw new NotFoundException("Project is not unavailable");
            if (!await _fileGRepo.AnyAsync(u => u.Project.OwnerId == userId))
                throw new UnauthorizedAccessException("only owner can create a file");

            var file = _mapper.Map<ProjectFile>(item);
            file.CreatedAt = DateTime.UtcNow;
            file.CreatedBy = userId;
            file.Content = "  // Start coding here ";
            file.AssignedTo = userId;
            file.AssignedAt = DateTime.UtcNow;

           
          
            var res = await _fileGRepo.AddAsync(file);
            await _notify.Clients.Group(Convert.ToString(item.ProjectId)).SendAsync("ReceiveNotification", new
            {
                Title = "File created",
                Message = $"owner created a file called {item.FileName} ",
                Time = DateTime.UtcNow
            });
            return _mapper.Map<NewFileResDto>(res);
        }

        public async Task<bool> DeleteFile(int FileId, int userId)
        {
            var item = await _fileGRepo.GetByIdAsync(FileId);

            if (item == null)
                throw new NotFoundException("Such a file not found");

            if (!await _fileGRepo.AnyAsync(u => u.ProjectId == item.ProjectId && u.Project.OwnerId == userId && !u.IsDeleted))
                throw new UnauthorizedAccessException("only owner can delete  file");

            if (item.AssignedTo != userId)
                throw new Exception("Couldnt deleted , this is assigned to someone");

            item.IsDeleted = true;
            item.DeletdBy = userId;
            item.DeletedAt = DateTime.UtcNow;

            await _fileGRepo.UpdateAsync(item);
            await _notify.Clients.Group(Convert.ToString(item.ProjectId)).SendAsync("ReceiveNotification", new
            {
                Title = "File Deleted",
                Message = $"owner deleted a file called {item.FileName} ",
                Time = DateTime.UtcNow
            });
            return true;
        }
        public async Task<ProjectFile> SaveFile(SaveFileReqDto dto, int userId)
        {
            var context = _fileGRepo.GetDbContext();

            var item = await context.ProjectFiles
                .FirstOrDefaultAsync(u => u.Id == dto.FileId && u.ProjectId == dto.ProjectId && !u.IsDeleted);

            if (item == null)
                throw new NotFoundException("Such a file not found");

            if (item.AssignedTo != userId)
                throw new UnauthorizedAccessException("This file not assigned to you");
            item.Content = dto.Content;
            item.ModifiedAt = DateTime.UtcNow;
            item.ModifiedBy = userId;
            item.Status = FileStatus.Saved;

            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                throw new NotFoundException("User not found");

            var version = new FileVersion
            {
                FileId = item.Id,
                FileName = item.FileName,
                Content = item.Content,
                SavedBy = user.UserName,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = userId
            };

            // Ensure transaction safety
            using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                await context.Versions.AddAsync(version);
                context.ProjectFiles.Update(item);
                await context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
            await _notify.Clients.User(Convert.ToString(dto.ProjectId)).SendAsync("RecieveNotification", new
            {
                Title = "Saved a file",
                Message = $"{item.FileName} is saved or updated  now",
                Time = DateTime.UtcNow
            });
            return item;
        }



        public async Task<bool> UpdateFile(FileUpdateReqDto dto, int userId)
        {
            var item = await _fileGRepo.FirstOrDefaultAsync(u => u.Id == dto.Id && u.ProjectId == dto.ProjectId && !u.IsDeleted);

            if (item == null)
                throw new NotFoundException("Such a file not found");
            if (item.AssignedTo != userId)
                throw new UnauthorizedAccessException("This file not assigned to you");
            item.Content = dto.Content;
            item.FileName = dto.FileName;
            item.ModifiedAt = DateTime.UtcNow;
            item.ModifiedBy = userId;
            item.Status = FileStatus.Saved;
            await _fileGRepo.UpdateAsync(item);
            return true;
        }


        public async Task<ProjectFile> Assign(FileAssignReqDto dto, int userId)
        {
            var item = await _fileGRepo.Query()
                .Where(u => u.Id == dto.FileId)
                .Include(u => u.Project)
                    .ThenInclude(u => u.Members)
                .FirstOrDefaultAsync();
            if (item == null || item.IsDeleted)
                throw new NotFoundException("Such a file not found");

            if (item.Project.OwnerId != userId)
                throw new UnauthorizedAccessException("Only owner can manage files");
            if (item.AssignedTo != userId)
                throw new BadHttpRequestException("This file is alraedy assigned to somone");

            if (!item.Project.Members.Any(u => u.UserId == dto.TargetUserId))
                throw new NotFoundException("there is no such member  in this project");
            item.AssignedTo = dto.TargetUserId;
            item.AssignedAt = DateTime.UtcNow;
            item.Status = FileStatus.Assigned;

            item.ModifiedAt = DateTime.UtcNow;
            item.ModifiedBy = userId;
            await _fileGRepo.UpdateAsync(item);

            await _notify.Clients.User(Convert.ToString(dto.TargetUserId)).SendAsync("ReceiveNotification", new
            {
                Title = "File Assigned ",
                Message = $"You are assigned a file {item.FileName}",
                Time = DateTime.UtcNow

            });
            Console.WriteLine($"notification send to {dto.TargetUserId}");
            return item;
        }

        public async Task<ProjectFile> UnAssign(int FileId, int userId)
        {
            var item = await _fileGRepo.Query()
                .Where(u => u.Id == FileId)
                .Include(u => u.Project)
                .FirstOrDefaultAsync();
            if (item == null || item.IsDeleted)
                throw new NotFoundException("Such a file not found");

            if (item.Project.OwnerId != userId)
                throw new UnauthorizedAccessException("Onlu owner can manage files");
            if (item.Status == FileStatus.Progress)
                throw new Exception("This file is still in progress");

            item.AssignedTo = userId;
            item.AssignedAt = DateTime.UtcNow;
            item.Status = FileStatus.UnAssigned;

            item.ModifiedAt = DateTime.UtcNow;
            item.ModifiedBy = userId;
            await _fileGRepo.UpdateAsync(item);
            await _notify.Clients.User(Convert.ToString(userId)).SendAsync("RecieveNotification", new
            {
                Title = "File Unassigned",
                Message = $"Owner  unassigned your  file {item.FileName}",
                Time = DateTime.UtcNow
            });
            return item;

        }

        public async Task<bool> StartEdit(int fileId, int userId)
        {
            var file = await _fileGRepo.GetByIdAsync(fileId);
            if (file == null || file.IsDeleted)
                throw new NotFoundException("File not found");
            if (file.AssignedTo != userId)
                throw new UnauthorizedAccessException("You can not edit the file");
            file.Status = FileStatus.Progress;
            file.ModifiedAt = DateTime.UtcNow;
            file.ModifiedBy = userId;
             await _fileGRepo.UpdateAsync(file);
            return true;
        }

        public async Task<List<FileVersion>> GetAllVersions(int FileId, int userId)
        {
            var item = await _VersionGRepo.GetAllByCondition(u => u.FileId == FileId && !u.IsDeleted);
            return item.OrderBy(u => u.CreatedAt).ToList();              
        }

      

    }
}
