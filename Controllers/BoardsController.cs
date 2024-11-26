using Battleship.Data;
using Battleship.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

public class BoardsController : Controller
{
    private readonly AppDbContext _context;

    public BoardsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: /Boards/Play/{id}
    public IActionResult Play(int id)
    {
        var board = _context.Boards
            .Include(b => b.Cells)
            .FirstOrDefault(b => b.BoardID == id);

        if (board == null)
        {
            return NotFound("El tablero no existe.");
        }

        var game = _context.Games.FirstOrDefault(g => g.GameID == board.GameID);
        var player = _context.Players.FirstOrDefault(p => p.PlayerID == board.PlayerID);

        if (game == null || player == null)
        {
            return NotFound("El juego o el jugador no existen.");
        }

        var viewModel = new PlayViewModel
        {
            GameID = game.GameID,
            PlayerID = player.PlayerID,
            PlayerName = player.PlayerName,
            PlayerBoard = ConvertCellsToStringArray(board.Cells.ToList()),
            AIBoard = ConvertCellsToStringArray(GenerateAIBoard(board.GameID).Cells.ToList())
        };

        return View(viewModel);
    }
    // POST: /Boards/Create
    [HttpPost]
    public IActionResult CreateBoard(int gameId, int playerId)
    {
        Console.WriteLine($"CreateBoard called with gameId: {gameId}, playerId: {playerId}");

        var gameExists = _context.Games.Any(g => g.GameID == gameId);
        if (!gameExists)
        {
            Console.WriteLine("El GameID no existe en la base de datos.");
            return Json(new { success = false, message = "El GameID no existe en la base de datos." });
        }

        var playerBoard = GenerateEmptyBoard(gameId);
        playerBoard.PlayerID = playerId; // Asigna el PlayerID correspondiente
        var aiBoard = GenerateAIBoard(gameId);

        try
        {
            _context.Boards.Add(playerBoard);
            _context.SaveChanges();
            Console.WriteLine("Player board saved successfully.");

            _context.Boards.Add(aiBoard);
            _context.SaveChanges();
            Console.WriteLine("AI board saved successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al guardar en la base de datos: {ex.Message}");
            return Json(new { success = false, message = $"Error al guardar en la base de datos: {ex.Message}" });
        }

        return RedirectToAction("Play", "Games", new { id = gameId });
    }

    private Boards GenerateEmptyBoard(int gameId)
    {
        var board = new Boards
        {
            GameID = gameId,
            PlayerID = 0,
            Cells = new List<Cells>()
        };

        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                board.Cells.Add(new Cells { Row = i, ColumnNumber = j, HasShip = false, IsHit = false });
            }
        }

        // Convertir el estado del tablero a JSON y asignarlo a BoardData
        board.BoardData = JsonConvert.SerializeObject(board.Cells.Select(c => new { c.Row, c.ColumnNumber, c.HasShip, c.IsHit }));

        return board;
    }

    private Boards GenerateAIBoard(int gameId)
    {
        var board = GenerateEmptyBoard(gameId);
        var random = new Random();

        for (int i = 0; i < 5; i++)
        {
            int row = random.Next(0, 10);
            int column = random.Next(0, 10);
            var cell = board.Cells.First(c => c.Row == row && c.ColumnNumber == column);
            cell.HasShip = true;
        }

        // Convertir el estado del tablero a JSON y asignarlo a BoardData
        board.BoardData = JsonConvert.SerializeObject(board.Cells.Select(c => new { c.Row, c.ColumnNumber, c.HasShip, c.IsHit }));

        return board;
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
                result[i][j] = cell != null ? (cell.HasShip ? "S" : "E") : "E"; // "S" for Ship, "E" for Empty
            }
        }
        return result;
    }

    // DELETE: /Boards/Delete/{id}
    [HttpPost]
    public IActionResult Delete(int id)
    {
        var board = _context.Boards.FirstOrDefault(b => b.BoardID == id);

        if (board == null)
        {
            return NotFound("El tablero no existe.");
        }

        _context.Boards.Remove(board);
        _context.SaveChanges();

        return RedirectToAction("Index", "Games");
    }
}