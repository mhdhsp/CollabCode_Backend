using AutoMapper;
using CollabCode.CollabCode.Application.DTO.ReqDto;
using CollabCode.CollabCode.Application.DTO.ResDto;
using CollabCode.CollabCode.Domain.Entities;
using CollabCode.CollabCode.Infrastructure.Persistense;
using CollabCode.CollabCode.Application.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using CollabCode.CollabCode.Application.Interfaces.Repositories;
using CollabCode.CollabCode.Application.Interfaces.Services;
using System.Runtime.InteropServices;
using CollabCode.CollabCode.Domain.Enums;

namespace CollabCode.CollabCode.Application.Services
{

    public class ProjectService : IProjectService
    {
        private readonly IGenericRepository<Project> _projectGRepo;
        private readonly IGenericRepository<ProjectFile> _fileGRepo;
        private readonly IGenericRepository<MemberShip> _memberGRepo;
        private readonly IGenericRepository<JoinMap> _mapRepo;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<ProjectService> _logger;

        public ProjectService(
            IGenericRepository<Project> Repo,
            IGenericRepository<ProjectFile> FRepo,
            IGenericRepository<MemberShip> MRepo,
            IGenericRepository<JoinMap> JRepo,

            ILogger<ProjectService> logger,
            AppDbContext context,
            IMapper Mapper
            )
        {

            _projectGRepo = Repo;
            _fileGRepo = FRepo;
            _memberGRepo = MRepo;
            _mapRepo = JRepo;
            _mapper = Mapper;
            _logger = logger;
            _context = context;

        }

        public async Task<NewProjectResDto> CreateNewProject(NewProjectReqDto reqDto, int userId)
        {
            var project = _mapper.Map<Project>(reqDto);
            project.OwnerId = userId;
            project.JoinCode = await GenerateJoinCode();

            if (!project.IsPublic && !string.IsNullOrEmpty(reqDto.PassWordHash))
                project.PassWordHash = BCrypt.Net.BCrypt.HashPassword(reqDto.PassWordHash);
            project.CreatedAt = DateTime.Now;
            project.CreatedBy = userId;

           
            
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                await _projectGRepo.AddAsync(project);

                var startingFile = new ProjectFile
                {
                    FileName = "index.html",
                    Content = "start coding here !",
                    AssignedTo = userId,
                    AssignedAt = DateTime.Now,
                    ProjectId = project.Id,
                    CreatedAt = DateTime.Now,
                    CreatedBy = userId
                };

                _logger.LogInformation($" hello {startingFile.FileName}");
               
                await _fileGRepo.AddAsync(startingFile);
               
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw new Exception("Couldnt create project now,Try agian !");
            }


            var ResDto = _mapper.Map<NewProjectResDto>(project);
            return ResDto;
        }

        public async Task<NewProjectResDto> JoinProject(ProjectJoinReqDto reqDto, int userId)
        {
            

            var existing = await _projectGRepo.FirstOrDefaultAsync(u => u.JoinCode == reqDto.JoinCode && !u.IsDeleted);
            if (existing == null)
                throw new NotFoundException("Project  not found");
            if (await _memberGRepo.AnyAsync(u => u.UserId == userId && u.ProjectId == existing.Id && !u.IsDeleted))
                throw new AlreadyExistsException("You alraedy a member of this project");
            if (!existing.IsPublic)
            {
                if (!BCrypt.Net.BCrypt.Verify(reqDto.PassWord, existing.PassWordHash))
                    throw new MismatchException("Invalid project password");
            }
            if (existing.OwnerId == userId)
                throw new Exception("You are the owner of the project");
            var member = new MemberShip
            {
                UserId = userId,
                ProjectId = existing.Id,
                CreatedAt = DateTime.Now,
                CreatedBy = userId
            };

            var map = new JoinMap
            {
                JoinCode = existing.JoinCode,
                userId = userId
            };
            using var trans =await  _context.Database.BeginTransactionAsync();
            try
            {
                await _memberGRepo.AddAsync(member);
                await _mapRepo.AddAsync(map);
                await trans.CommitAsync();
            }
            catch(Exception e)
            {
                _logger.LogError($"Failed to add to map with{e.Message}");
               await  trans.RollbackAsync();
            }
            
            var res = _mapper.Map<NewProjectResDto>(existing);
            return res;
        }


        public async Task<ProjectResDto> EnterProject(int projectId, int userId)
        {
            _logger.LogInformation($" from roomservice{projectId}+{userId}");
            var project = await _projectGRepo.Query()
                .Where(u => u.Id == projectId && !u.IsDeleted)
                .Include(u => u.Members.Where(u=> !u.IsDeleted))
                    .ThenInclude(u => u.User)
                .Include(u => u.Files.Where(u=>!u.IsDeleted))
                .FirstOrDefaultAsync();

            if (project == null)
                throw new NotFoundException("Project  not found");
            foreach (var file in project.Files)
            {
                if (file.AssignedAt == null)
                    file.AssignedAt = DateTime.Now;
                TimeSpan day = DateTime.Now - file.AssignedAt.Value;
                if (day.TotalDays > 2)
                    file.Status = FileStatus.InActive;
            }
            var projectDto = new ProjectResDto
            {
                ProjectName = project.ProjectName,
                OwnerId = project.OwnerId,
                joinCode=project.JoinCode,
                Members = project.Members.Select(u => new MemberDto
                {
                    UserName = u.User?.UserName,
                    id = u.User.Id
                }).ToList(),
                Files = project.Files.Select(
                    u => new FileDto
                    {
                        Id = u.Id,
                        FileName = u.FileName,
                        ProjectId = u.ProjectId,
                        AssignedTo = u.AssignedTo,
                        Content = u.Content,
                        Status=u.Status.ToString()
                    }
                    ).ToList()
            };

            return projectDto;
        }

        public async Task<bool?> LeaveProject(int userId, int projectId)
        {
            if (!await _projectGRepo.AnyAsync(u => u.Id == projectId && !u.IsDeleted))
                throw new NotFoundException(" Project not found");
            else if (await _projectGRepo.AnyAsync(u =>u.Id==projectId && u.OwnerId == userId && !u.IsDeleted))
                throw new UnauthorizedAccessException("Owner canot leave the project");



            var membership = await _memberGRepo.Query()
                .Where(u => u.UserId == userId && u.ProjectId == projectId && !u.IsDeleted)
                .Include(u => u.Project)
                    .ThenInclude(u => u.Files)
                .FirstOrDefaultAsync();


            if (membership == null)
                throw new NotFoundException("You are not member of the project");
            if (membership.Project.Files.Any(u => u.AssignedTo == userId && u.Status == FileStatus.Progress))
                throw new Exception("You can not leave the project until all files are saved ");
            foreach(var file in membership.Project.Files.Where(u=>u.AssignedTo==userId))
            {
                file.Status = FileStatus.UnAssigned;
            }

            membership.DeletdBy = userId;
            membership.DeletedAt = DateTime.Now;
            membership.IsDeleted = true;

            await _memberGRepo.UpdateAsync(membership);
            return true;
        }

        public async Task<bool> DestroyProject(int userId, int projectid)
        {
            var project = await _projectGRepo.Query()
                .Where(u => u.Id == projectid && !u.IsDeleted)
                .Include(u => u.Members)
                .FirstOrDefaultAsync();

            if (project == null || project.IsDeleted)
                throw new NotFoundException("project not found");


            if (project.OwnerId != userId)
                throw new UnauthorizedAccessException("Only the project owner can delete this room");

            project.IsDeleted = true;
            project.DeletdBy = userId;
            project.DeletedAt = DateTime.Now;

            foreach(var m in project.Members)
            {
                m.IsDeleted = true;
                m.DeletdBy = userId;
                m.DeletedAt = DateTime.Now;
            }

            await _projectGRepo.UpdateAsync(project);
            return true;
        }


        public async Task RemoveMember(int ownerId, int projectId, int memberId)
        {
            Console.WriteLine("Remove member");
            var project = await _projectGRepo.Query()
                .Where(u => u.Id == projectId&& !u.IsDeleted)
                .Include(u => u.Files)
                .FirstOrDefaultAsync();

            if (project == null || project.IsDeleted)
                throw new NotFoundException("Project not found");

            if (project.OwnerId != ownerId)
                throw new UnauthorizedAccessException("Only the project owner can remove members");

            if (project.Files.Any(u => u.AssignedTo == memberId && u.Status==FileStatus.Progress))
                throw new UnauthorizedAccessException("Couldnt remove user , Some files assigned to this member are in progress  ");
            var membership = await _memberGRepo.FirstOrDefaultAsync(m => m.UserId == memberId && m.ProjectId == projectId && !m.IsDeleted);
            if (membership == null)
                throw new NotFoundException("Member not found");

            foreach(var file in project.Files.Where(u=>u.AssignedTo==memberId))
            {
                if(file.Status==FileStatus.InActive|| file.Status==FileStatus.Saved )
                {
                    file.Status = FileStatus.UnAssigned;
                    file.AssignedTo = ownerId;
                    file.AssignedAt = null;

                    file.ModifiedAt = DateTime.Now;
                    file.ModifiedBy = ownerId;
                   await  _fileGRepo.UpdateAsync(file);
                }
            }

            membership.DeletdBy = ownerId;
            membership.DeletedAt = DateTime.Now;
            membership.IsDeleted = true;
            await _memberGRepo.UpdateAsync(membership);
        }

        private async Task<string> GenerateJoinCode()
        {
            string code;
            do
            {
                code = RandomNumberGenerator.GetInt32(0, 100000).ToString("D6");
            }
            while (await _projectGRepo.AnyAsync(u => u.JoinCode == code));
            return code;
        }
    }
}
