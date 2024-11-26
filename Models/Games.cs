using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Battleship.Models
{
    public class Games
    {
        [Key]
        public int GameID { get; set; }

        [Required]
        public int PlayerID { get; set; }

        [Required]
        [StringLength(20)]
        public string DifficultyLevel { get; set; }

        public DateTime StartTime { get; set; } = DateTime.Now;

        public DateTime? EndTime { get; set; }

        [StringLength(50)]
        public string Winner { get; set; } = string.Empty; // Inicializar Winner como vacío

        // Relación: una partida tiene un jugador
        public ICollection<Players> Players { get; set; } = new List<Players>();

        // Relación: una partida puede tener muchos tableros
        public ICollection<Boards> Boards { get; set; } = new List<Boards>();

        // Relación: una partida puede tener muchos movimientos
        public ICollection<GameMoves> GameMoves { get; set; } = new List<GameMoves>();

        // Relación: una partida tiene estadísticas de IA
        public ICollection<AIStats> AIStats { get; set; } = new List<AIStats>();
    }

    public enum DifficultyLevel
    {
        Easy,
        Medium,
        Hard
    }
}