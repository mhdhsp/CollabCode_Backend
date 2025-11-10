namespace CollabCode.CollabCode.Domain.Entities
{
    public class JoinMap: BaseEntity
    {
        public int Id { get; set; }
        public  string? JoinCode { get; set; }
        public int userId { get; set; }
        public User? User { get; set; }
    }
}
