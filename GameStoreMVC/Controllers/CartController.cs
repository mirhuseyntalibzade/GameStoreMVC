using GameStoreMVC.Contexts;
using GameStoreMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using GameStoreMVC.DTO.CartItemDTO;

namespace GameStoreMVC.Controllers
{
    public class CartController : Controller
    {
        readonly AppDbContext _context;
        public CartController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List<CartItem> cartItems = new List<CartItem>();
            string? cookie = Request.Cookies["Cart"];
            if (cookie != null)
            {
                cartItems = JsonConvert.DeserializeObject<List<CartItem>>(cookie);
            }
            return View(cartItems);
        }


        public async Task<IActionResult> AddToCart(int Id)
        {
            Product? product = await _context.Products.Include(g => g.Game).FirstOrDefaultAsync(p => p.Id == Id);
            if (product is null)
            {
                return NotFound("not found");
            }

            List<CartItem> cart = new List<CartItem>();
            string? cookie = Request.Cookies["Cart"];
            if (cookie != null)
            {
                cart = JsonConvert.DeserializeObject<List<CartItem>>(cookie);
            }

            CartItem cartItem = new CartItem
            {
                Id = product.Id,
                Title = product.Title,
                GameName = product.Game.Title,
                Price = product.Price,
                ImagePath = product.ImagePath,
                Quantity = 1
            };


            CartItem? existingCartItem = cart.FirstOrDefault(p => p.Id == cartItem.Id);
            if (existingCartItem == null)
            {
                cart.Add(cartItem);
            }
            else
            {
                existingCartItem.Quantity += 1;
            }


            string serializedObject = JsonConvert.SerializeObject(cart);
            Response.Cookies.Append("Cart", serializedObject, new CookieOptions
            {
                Expires = DateTime.Now.AddDays(7),
                Path = "/"
            });
            return RedirectToAction("Index", "Shop");
        }
        [HttpPost]
        public IActionResult RemoveItem([FromBody] RemoveItemDTO request)
        {
            int Id = request.Id;
            Console.WriteLine($"Received Id: {Id}");
            List<CartItem> cartItems = new List<CartItem>();
            string? cookie = Request.Cookies["Cart"];
            if (cookie != null)
            {
                cartItems = JsonConvert.DeserializeObject<List<CartItem>>(cookie) ?? new List<CartItem>();
            }

            CartItem? removedItem = cartItems.FirstOrDefault(i => i.Id == Id);
            if (removedItem == null)
            {
                return Json(new { success = false, message = "Item not found" });
            }
            cartItems.Remove(removedItem);
            string serializedObject = JsonConvert.SerializeObject(cartItems);
            Response.Cookies.Append("Cart", serializedObject);
            return Json(new { success = true });
        }


        public async Task<IActionResult> ShowCart()
        {
            List<CartItem> cartItems = new List<CartItem>();
            var cookie = Request.Cookies["Cart"];
            if (cookie != null)
            {
                cartItems = cartItems = JsonConvert.DeserializeObject<List<CartItem>>(cookie);
            }
            return Json(cartItems);
        }
    }
}