using Microsoft.AspNetCore.Mvc;
using Battleship.Models;
using Battleship.Data;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace Battleship.Controllers
{
    public class PlayersController : Controller
    {
        private readonly AppDbContext _context;

        public PlayersController(AppDbContext context)
        {
            _context = context;
        }

        // Método GET: Mostrar formulario de registro
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create([Bind("PlayerID, PlayerName")] Players player)
        {
            Console.WriteLine($"Nombre del jugador recibido: {player.PlayerName}");

            if (ModelState.IsValid)
            {
                try
                {
                    Console.WriteLine("Modelo válido, verificando si el jugador ya existe...");

                    if (await _context.Players.AnyAsync(p => p.PlayerName == player.PlayerName))
                    {
                        Console.WriteLine("El jugador ya existe.");
                        ModelState.AddModelError("", "El nombre de jugador ya está registrado.");
                        return View(player);
                    }

                    Console.WriteLine("El jugador no existe, agregando a la base de datos...");
                    _context.Players.Add(player);
                    await _context.SaveChangesAsync();

                    Console.WriteLine($"Jugador registrado: {player.PlayerName}");
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al guardar el jugador: {ex.Message}");
                    Console.WriteLine($"Detalle del error: {ex.InnerException?.Message}");
                    ModelState.AddModelError("", "Hubo un problema al registrar el jugador. Intenta nuevamente.");
                    return View(player);
                }
            }

            Console.WriteLine("Modelo no válido.");
            Console.WriteLine("Errores de validación:");
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine(error.ErrorMessage);
            }

            return View(player);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Console.WriteLine($"Intentando eliminar el jugador con ID: {id}");
            var player = await _context.Players.FindAsync(id);
            if (player == null)
            {
                Console.WriteLine("Jugador no encontrado.");
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _context.Players.Remove(player);
                await _context.SaveChangesAsync();
                Console.WriteLine("Jugador eliminado exitosamente.");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Console.WriteLine($"Error de concurrencia: {ex.Message}");
                if (!PlayerExists(id))
                {
                    Console.WriteLine("Jugador no existe.");
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction(nameof(Index));
        }

        private bool PlayerExists(int id)
        {
            return _context.Players.Any(e => e.PlayerID == id);
        }

        // Método para mostrar los jugadores registrados (opcional)
        public async Task<IActionResult> Index()
        {
            var players = await _context.Players.ToListAsync();

            // Depura los jugadores obtenidos
            foreach (var player in players)
            {
                Console.WriteLine($"Jugador: {player.PlayerName}");
            }

            return View(players);
        }
    }
}
