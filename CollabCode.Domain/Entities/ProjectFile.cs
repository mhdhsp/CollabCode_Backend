namespace CollabCode.CollabCode.Domain.Entities
{
    public class ProjectFile:BaseEntity
    {
        public int Id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;

        public int? AssignedTo { get; set; } // Null = unlocked
        public DateTime? AssignedAt { get; set; }

        public int ProjectId { get; set; }
        public Project Project { get; set; } = null!;
    }
}
