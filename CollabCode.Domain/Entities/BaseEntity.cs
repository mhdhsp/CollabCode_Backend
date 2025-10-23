namespace CollabCode.CollabCode.Domain.Entities
{
    public class BaseEntity
    {
        public int CreatedBy { get; set; } 
        public DateTime CreatedAt { get; set; }
        public int? ModifiedBy { get; set; } = null;
        public DateTime? ModifiedAt { get; set; } = null;
        public int DeletdBy { get; set; } = 0;
        public DateTime? DeletedAt { get; set; } = null;
        public bool IsDeleted { get; set; } = false;
    }
}
