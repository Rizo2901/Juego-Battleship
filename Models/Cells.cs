using System.ComponentModel.DataAnnotations;

namespace Battleship.Models
{
    public class Cells
    {
        [Key]
        public int CellID { get; set; }
        public int BoardID { get; set; }
        public Boards Board { get; set; }
        public int Row { get; set; }
        public int ColumnNumber { get; set; }
        public bool HasShip { get; set; }
        public bool IsHit { get; set; }
    }
}