using System.ComponentModel.DataAnnotations;

namespace Battleship.Models
{
    public class Players
    {
        [Key]
        public int PlayerID { get; set; }

        [Required]
        [StringLength(50)]
        public string PlayerName { get; set; }

        public DateTime RegistrationDate { get; set; } = DateTime.Now;

        // Relación: un jugador puede tener muchas partidas
        public ICollection<Games> Games { get; set; } = new List<Games>();
        public ICollection<Boards> Boards { get; set; } = new List<Boards>();
        public ICollection<GameMoves> GameMoves { get; set; } = new List<GameMoves>();
    }
}
