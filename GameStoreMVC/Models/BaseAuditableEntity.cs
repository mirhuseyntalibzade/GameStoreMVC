namespace GameStoreMVC.Models
{
    public class BaseAuditableEntity : BaseEntity
    {
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool isActive { get; set; } = true;
    }
}
