using System.Collections.Generic;

namespace CollabCode.CollabCode.Application.DTO.ResDto
{
    public class UserProjectsDto
    {
        public string? UserName { get; set; }
        public List<ProjectDto> OwnedRooms { get; set; } = new();
        public List<ProjectDto> JoinedRooms { get; set; } = new();
    }

    public class ProjectDto
    {
        public int Id { get; set; }
        public string? ProjectName { get; set; }
        public string? JoinCode { get; set; } = null;
    }
}
