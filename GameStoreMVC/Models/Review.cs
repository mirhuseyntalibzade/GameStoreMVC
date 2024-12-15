namespace GameStoreMVC.Models
{
    public class Review : BaseAuditableEntity
    {
        public string Message { get; set; }
        public int ProductId { get; set; }
        public string Username { get; set; }
        public Product Product { get; set; }
    }
}
