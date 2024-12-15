using GameStoreMVC.Contexts;
using GameStoreMVC.Models;
using GameStoreMVC.ViewModels.GameViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameStoreMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class GameController : Controller
    {
        readonly AppDbContext _context;
        public GameController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Game> games = await _context.Games.ToListAsync();
            return View(games);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateGameVM createdGame)
        {
            Game? gameDB = await _context.Games.FirstOrDefaultAsync(g => g.Title == createdGame.Title);
            if (gameDB != null)
            {
                return BadRequest("Game has already been created");
            }
            Game game = new Game
            {
                CreatedAt = DateTime.Now,
                Title = createdGame.Title
            };
            await _context.Games.AddAsync(game);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Game");
        }

        public async Task<IActionResult> Update(int Id)
        {
            Game? game = await _context.Games.FirstOrDefaultAsync(g => g.Id == Id);
            if (game == null)
            {
                return NotFound("not found my bröder");
            }
            UpdateGameVM updateGame = new UpdateGameVM
            {
                Id = game.Id,
                Title = game.Title
            };
            return View(updateGame);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateGameVM updatedGame)
        {
            Game? gameDB = await _context.Games.FirstOrDefaultAsync(g => g.Id == updatedGame.Id);
            if (gameDB == null)
            {
                return NotFound("not found my bröder");
            }
            Game game = new Game
            {
                Id = updatedGame.Id,
                Title = updatedGame.Title,
                UpdatedAt = DateTime.Now,
                CreatedAt = gameDB.CreatedAt
            };
            _context.Games.Update(game);
            _context.SaveChanges();
            return RedirectToAction("Index", "Game");
        }

        public async Task<IActionResult> SoftDelete(int Id)
        {
            Game? game = await _context.Games.FirstOrDefaultAsync(g => g.Id == Id);
            if (game == null)
            {
                return NotFound("not found my bröder");
            }
            if (!game.isActive)
            {
                return BadRequest("Item has already been soft deleted.");
            }
            game.DeletedAt = DateTime.Now;
            game.isActive = false;
            _context.Games.Update(game);
            _context.SaveChanges();
            return RedirectToAction("Index", "Game");
        }

        public async Task<IActionResult> RevertSoftDelete(int Id)
        {
            Game? game = await _context.Games.FirstOrDefaultAsync(g => g.Id == Id);
            if (game == null)
            {
                return NotFound("not found my bröder");
            }
            if (game.isActive)
            {
                return BadRequest("Item has already been restored.");
            }
            game.isActive = true;
            _context.Games.Update(game);
            _context.SaveChanges();
            return RedirectToAction("Index", "Game");
        }

        public async Task<IActionResult> HardDelete(int Id)
        {
            Game? game = await _context.Games.FirstOrDefaultAsync(g => g.Id == Id);
            if (game == null)
            {
                return NotFound("not found my bröder");
            }
            if (game.isActive)
            {
                return BadRequest("Item hasn't been soft deleted yet.");
            }
            _context.Games.Remove(game);
            _context.SaveChanges();
            return RedirectToAction("Index", "Game");
        }
    }
}
