namespace CollabCode.CollabCode.Domain.Entities
{
    public class Chat:BaseEntity
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int ProjectId{ get; set; }
        public string? Content  { get; set; }

        public Project? Project { get; set; }
        public User? User { get; set; }
    }
}
