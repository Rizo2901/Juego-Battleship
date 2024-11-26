
namespace Battleship.Models
{
    public class PlayViewModel
    {
            public int GameID { get; set; }
            public int PlayerID { get; set; }
            public string PlayerName { get; set; } 
            public string[][] PlayerBoard { get; set; }
            public string[][] AIBoard { get; set; }

        
    }
}
