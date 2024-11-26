using System.ComponentModel.DataAnnotations;

namespace Battleship.Models
{
    public class AIStats
    {
        [Key]
        public int StatID { get; set; }

        public int GameID { get; set; }

        [Required]
        [StringLength(20)]
        public string DifficultyLevel { get; set; }

        public int MovesMade { get; set; } = 0;

        public int Hits { get; set; } = 0;

        public int Misses { get; set; } = 0;

        // Relación: las estadísticas de la IA pertenecen a una partida
        public Games Game { get; set; }
    }
}
