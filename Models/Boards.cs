using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace Battleship.Models
{
    public class Boards
    {
        [Key]
        public int BoardID { get; set; }

        public int GameID { get; set; }

        public int? PlayerID { get; set; } // Puede ser NULL para la computadora

        public string BoardData { get; set; } // Representación en JSON

        // Relación: un tablero pertenece a una partida
        public ICollection<Games> Games { get; set; } = new List<Games>();

        public ICollection<Players> Players { get; set; } = new List<Players>();
        public ICollection<Cells> Cells { get; set; } = new List<Cells>();
    }
}
