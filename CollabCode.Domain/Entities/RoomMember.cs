using System.ComponentModel.DataAnnotations;

namespace CollabCode.CollabCode.Domain.Entities
{
    public class RoomMember
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }
        public  User? User { get; set; }

        public int RoomId { get; set; }
       
        public  Room? Room { get; set; }

        public bool IsOwner { get; set; } = false;
    }
}
