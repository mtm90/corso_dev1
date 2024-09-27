namespace PokerAppMVC.Models
{
    public class Player
    {
        public int PlayerId { get; set; }  // Primary Key
        public string PlayerName { get; set; }
        public int Stack { get; set; } = 500;  // Initial Stack of 500
    }
}
