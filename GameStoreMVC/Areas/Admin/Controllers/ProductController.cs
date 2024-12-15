using GameStoreMVC.Contexts;
using GameStoreMVC.Models;
using GameStoreMVC.ViewModels.ProductViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GameStoreMVC.Utilities;

namespace GameStoreMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        readonly AppDbContext _context;
        readonly IWebHostEnvironment _webHostEnvironment; 
        public ProductController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Product> products = await _context.Products.ToListAsync();
            return View(products);
        }

        public IActionResult Create()
        {
            ViewBag.Games = new SelectList(_context.Games, "Id", "Title");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductVM createdProduct)
        {
            string? imagePath = null;

            if (createdProduct.Image != null)
            {
                try
                {
                    imagePath = await createdProduct.Image.AddFileAsync(_webHostEnvironment, "uploads/products", 5 * 1024 * 1024);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("ImageFile", ex.Message);
                    return View(createdProduct);
                }
            }
            createdProduct.ImagePath = imagePath;

            Product product = new Product
            {
                CreatedAt = DateTime.Now,
                Title = createdProduct.Title,
                Price = createdProduct.Price,
                Desc = createdProduct.Desc,
                ImagePath = createdProduct.ImagePath,
                GameId = createdProduct.GameId
            };
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Product");
        }

        public async Task<IActionResult> Update(int Id)
        {
            Product? product = await _context.Products.FirstOrDefaultAsync(g => g.Id == Id);
            if (product == null)
            {
                return NotFound("not found my bröder");
            }
            ViewBag.Games = new SelectList(_context.Games, "Id", "Title");
            UpdateProductVM productVM = new UpdateProductVM
            {
                Id = product.Id,
                Title = product.Title,
                Price = product.Price,
                Desc = product.Desc,
                ImagePath = product.ImagePath,
                GameId = product.GameId,
            };
            return View(productVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateProductVM updatedProduct)
        {
            Product? productDB = await _context.Products.FirstOrDefaultAsync(g => g.Id == updatedProduct.Id);
            if (productDB == null)
            {
                return NotFound("not found my bröder");
            }
            Product product = new Product
            {
                Id = updatedProduct.Id,
                Title = updatedProduct.Title,
                Price = updatedProduct.Price,
                Desc = updatedProduct.Desc,
                ImagePath = updatedProduct.ImagePath,
                GameId = updatedProduct.GameId,
                UpdatedAt = DateTime.Now,
                CreatedAt = productDB.CreatedAt
            };
            _context.Products.Update(product);
            _context.SaveChanges();
            return RedirectToAction("Index", "Product");
        }

        public async Task<IActionResult> SoftDelete(int Id)
        {
            Product? product = await _context.Products.FirstOrDefaultAsync(g => g.Id == Id);
            if (product == null)
            {
                return NotFound("not found my bröder");
            }
            if (!product.isActive)
            {
                return BadRequest("Item has already been soft deleted.");
            }
            product.DeletedAt = DateTime.Now;
            product.isActive = false;
            _context.Products.Update(product);
            _context.SaveChanges();
            return RedirectToAction("Index", "Product");
        }

        public async Task<IActionResult> RevertSoftDelete(int Id)
        {
            Product? product = await _context.Products.FirstOrDefaultAsync(g => g.Id == Id);
            if (product == null)
            {
                return NotFound("not found my bröder");
            }
            if (product.isActive)
            {
                return BadRequest("Item has already been restored.");
            }
            product.isActive = true;
            _context.Products.Update(product);
            _context.SaveChanges();
            return RedirectToAction("Index", "Product");
        }

        public async Task<IActionResult> HardDelete(int Id)
        {
            Product? product = await _context.Products.FirstOrDefaultAsync(g => g.Id == Id);
            if (product == null)
            {
                return NotFound("not found my bröder");
            }
            if (product.isActive)
            {
                return BadRequest("Item hasn't been soft deleted yet.");
            }
            _context.Products.Remove(product);
            _context.SaveChanges();
            return RedirectToAction("Index", "Product");
        }
    }
}
