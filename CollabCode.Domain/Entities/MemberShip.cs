using Microsoft.Identity.Client;

namespace CollabCode.CollabCode.Domain.Entities
{
    public class MemberShip:BaseEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; } 

        public int ProjectId { get; set; }
        public Project? Project { get; set; }

    }
}
