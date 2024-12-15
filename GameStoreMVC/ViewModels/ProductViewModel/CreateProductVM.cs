using GameStoreMVC.Models;

namespace GameStoreMVC.ViewModels.ProductViewModel
{
    public class CreateProductVM
    {
        public string Title { get; set; }
        public double Price { get; set; }
        public string Desc { get; set; }
        public string ImagePath { get; set; }
        public int GameId { get; set; }
        public Game Games { get; set; }
        public IFormFile Image { get; set; }
    }
}
