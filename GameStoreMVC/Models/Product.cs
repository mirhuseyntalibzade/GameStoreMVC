namespace GameStoreMVC.Models
{
    public class Product : BaseAuditableEntity
    {
        public string Title { get; set; }
        public double Price { get; set; }
        public string Desc { get; set; }
        public string ImagePath { get; set; }
        public int GameId { get; set; }
        public Game Game { get; set; }
        public ICollection<Review>? Reviews { get; set; }
    }
}
