namespace PokerAppMVC.Models
{
    public class Hand
    {
        public int HandId { get; set; }  // Primary Key
        public int PlayerId { get; set; }  // Foreign Key
        public string PlayerHand { get; set; }
        public string ComputerHand { get; set; }
        public int PlayerStack { get; set; }  // Track player's stack at the end of the hand
        public int ComputerStack { get; set; }  // Track computer's stack at the end of the hand
        public int Pot { get; set; }  // Total pot for the hand
        public int PlayerBet { get; set; }  // Current bet for the player
        public int ComputerBet { get; set; }  // Current bet for the computer

        // Navigation Property for Player (Foreign Key Relationship)
        public Player Player { get; set; }
    }
}
