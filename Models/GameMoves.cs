using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace Battleship.Models
{
    public class GameMoves
    {
        [Key]
        public int MoveID { get; set; }

        public int GameID { get; set; }

        public int? PlayerID { get; set; } // Puede ser NULL para los movimientos de la IA

        [Required]
        public int Row { get; set; }

        [Required]
        public int ColumnNumber { get; set; }

        [Required]
        [StringLength(10)]
        public string Result { get; set; } // "Hit", "Miss", "Sunk"

        public DateTime MoveTime { get; set; } = DateTime.Now;

        // Relación: un movimiento pertenece a una partida
        public Games Game { get; set; }

        // Relación: un movimiento puede pertenecer a un jugador
        public Players Player { get; set; }
    }
}
