using GameStoreMVC.Contexts;
using GameStoreMVC.Models;
using GameStoreMVC.ViewModels.ProductViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameStoreMVC.Controllers
{
    public class ShopController : Controller
    {
        readonly AppDbContext _context;

        public ShopController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Product> product = await _context.Products.ToListAsync();
            return View(product);
        }

        public async Task<IActionResult> Details(int Id)
        {
            Product? product = await _context.Products.Include(g=>g.Game).Include(p => p.Reviews).FirstOrDefaultAsync(p => p.Id == Id);
            if (product == null || !product.isActive)
            {
                return NotFound("not found");
            }
            ProductReview productReviewVM = new ProductReview
            {
                Product = product,
                Review = new Review()
            };
            return View(productReviewVM);
        }

        public async Task<IActionResult> SendReview(ProductReview productReviewVM)
        {
            string? username = User.Identity?.Name;

            if (string.IsNullOrEmpty(username))
            {
                return Unauthorized("You need to log in to submit a review.");
            }

            Product? product = await _context.Products
                .Include(p => p.Reviews)
                .FirstOrDefaultAsync(p => p.Id == productReviewVM.Product.Id);

            if (product == null)
            {
                return NotFound("Product not found.");
            }

            if (string.IsNullOrWhiteSpace(productReviewVM.Review.Message))
            {
                ModelState.AddModelError("Review.Message", "Message cannot be empty.");

                ProductReview productReviewWithErrors = new ProductReview
                {
                    Product = product,
                    Review = productReviewVM.Review
                };

                return View("Details", productReviewWithErrors);
            }

            Review review = new Review
            {
                ProductId = productReviewVM.Product.Id,
                Message = productReviewVM.Review.Message,
                Username = username
            };
            await _context.Reviews.AddAsync(review);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { Id = productReviewVM.Product.Id});
        }

    }
}
