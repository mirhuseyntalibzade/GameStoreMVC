namespace GameStoreMVC.Models
{
    public class Game : BaseAuditableEntity
    {
        public string Title { get; set; }
        public ICollection<Product>? Products { get; set; }
    }
}
