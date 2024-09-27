namespace PokerAppMVC.Models
{
    public class Hand
    {
        public int HandId { get; set; }  // Primary Key
        public int PlayerId { get; set; }  // Foreign Key
        public string PlayerHand { get; set; }
        public string ComputerHand { get; set; }
        public int PlayerStack { get; set; }
        public int ComputerStack { get; set; }

        // Navigation Property for Player (Foreign Key Relationship)
        public Player Player { get; set; }
    }
}
