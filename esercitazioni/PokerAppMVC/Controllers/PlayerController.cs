using PokerAppMVC.Models;

namespace PokerAppMVC.Controllers
{
    public class PlayerController
    {
        private readonly PokerDbContext _context;

        public PlayerController()
        {
            _context = new PokerDbContext();
        }

        // Create a new player
        public Player CreatePlayer(string playerName)
        {
            var player = new Player { PlayerName = playerName };
            _context.Players.Add(player);
            _context.SaveChanges();
            return player;
        }

        // Retrieve a player by name
        public Player GetPlayer(string playerName)
        {
            return _context.Players.FirstOrDefault(p => p.PlayerName == playerName);
        }

        // Retrieve a player by ID
        public Player GetPlayerById(int playerId)
        {
            return _context.Players.Find(playerId);
        }
    }
}
