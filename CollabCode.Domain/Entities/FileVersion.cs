using CollabCode.CollabCode.Domain.Enums;

namespace CollabCode.CollabCode.Domain.Entities
{
    public class FileVersion:BaseEntity
    {
        public int Id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;

        public string SavedBy { get; set; } = string.Empty;
        public int FileId { get; set; }
        public ProjectFile? File { get; set; }
        
    }
}
