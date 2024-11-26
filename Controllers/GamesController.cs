using Battleship.Data;
using Microsoft.AspNetCore.Mvc;
using System.Data.Entity;
using Microsoft.EntityFrameworkCore;
using Battleship.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using EFCoreInclude = Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions;
using System.Data.Entity;

namespace Battleship.Controllers
{
    public class GamesController : Controller
    {
        private readonly AppDbContext _context;

        public GamesController(AppDbContext context)
        {
            _context = context;
        }

        // Acción para mostrar las partidas de un jugador
        public IActionResult Index(int id, string name)
        {
            var playerGames = _context.Games
                .Where(g => g.PlayerID == id)
                .Select(g => new
                {
                    g.GameID,
                    PlayerName = _context.Players.FirstOrDefault(p => p.PlayerID == g.PlayerID).PlayerName, // Obtener el nombre del jugador basado en PlayerID
                    g.DifficultyLevel,
                    g.StartTime
                })
                .ToList();

            if (playerGames == null)
            {
                return NotFound();
            }

            ViewBag.PlayerName = name;
            ViewBag.PlayerID = id;
            ViewBag.DifficultyLevels = new List<SelectListItem>
    {
        new SelectListItem { Value = "Easy", Text = "Fácil" },
        new SelectListItem { Value = "Medium", Text = "Medio" },
        new SelectListItem { Value = "Hard", Text = "Difícil" }
    };

            return View(playerGames);
        }

        [HttpPost]
        public IActionResult Create([FromForm] Games newGame)
        {
            if (ModelState.IsValid)
            {
                _context.Games.Add(newGame);
                _context.SaveChanges();
                return Json(newGame);
            }
            else
            {
                Console.WriteLine("ModelState is invalid");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }
            return BadRequest("Invalid game data");
        }

        public IActionResult Play(int id)
        {
            var game = _context.Games
                .Include(g => g.Boards)
                .ThenInclude(b => b.Cells)
                .FirstOrDefault(g => g.GameID == id);

            if (game == null)
            {
                return NotFound("El juego no existe.");
            }

            var viewModel = new PlayViewModel
            {
                GameID = game.GameID,
                PlayerID = game.PlayerID,
                PlayerName = game.Player.PlayerName,
                PlayerBoard = ConvertCellsToStringArray(game.Boards.First(b => b.PlayerID == game.PlayerID).Cells.ToList()),
                AIBoard = ConvertCellsToStringArray(game.Boards.First(b => b.PlayerID != game.PlayerID).Cells.ToList())
            };

            return View(viewModel);
        }

        private string[][] ConvertCellsToStringArray(List<Cells> cells)
        {
            var result = new string[10][];
            for (int i = 0; i < 10; i++)
            {
                result[i] = new string[10];
                for (int j = 0; j < 10; j++)
                {
                    var cell = cells.FirstOrDefault(c => c.Row == i && c.ColumnNumber == j);
                    result[i][j] = cell != null ? (cell.HasShip ? "S" : "E") : "E"; // "S" para Ship, "E" para Empty
                }
            }
            return result;
        }

    }
}
