using CollabCode.CollabCode.Domain.Entities;
using System.Text.Json.Serialization;

namespace CollabCode.CollabCode.Application.DTO.ResDto
{
  
    
        public class NewProjectResDto
        {
            public string? ProjectName { get; set; }
            public string? JoinCode { get; set; }

            public int OwnerId { get; set; }
            public bool IsPublic { get; set; } = false;
        }
    




}
