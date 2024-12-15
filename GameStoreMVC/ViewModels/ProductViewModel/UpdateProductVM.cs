using GameStoreMVC.Models;

namespace GameStoreMVC.ViewModels.ProductViewModel
{
    public class UpdateProductVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public string Desc { get; set; }
        public string ImagePath { get; set; }
        public int GameId { get; set; }
        public Game Games { get; set; }
    }
}
