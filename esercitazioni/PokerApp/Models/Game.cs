using System.Collections.Generic;

namespace PokerApp.Models
{
    public class Game
    {
        public int Id { get; set; }
        public List<Card> PlayerHand { get; set; }
        public List<Card> ComputerHand { get; set; }
        public List<Card> CommunityCards { get; set; }
        public int PlayerChips { get; set; }
        public int ComputerChips { get; set; }
    }
}