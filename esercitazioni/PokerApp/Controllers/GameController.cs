using PokerApp.Models;
using System.Linq;

namespace PokerApp.Controllers
{
    public class GameController
    {
        private PokerContext _context;

        public GameController()
        {
            _context = new PokerContext();
        }

        public void StartNewGame()
        {
            var game = new Game
            {
                PlayerHand = new List<Card>(),
                ComputerHand = new List<Card>(),
                CommunityCards = new List<Card>(),
                PlayerChips = 1000,
                ComputerChips = 1000
            };
            _context.Games.Add(game);
            _context.SaveChanges();
        }

        public Game LoadGame(int gameId)
        {
            return _context.Games.FirstOrDefault(g => g.Id == gameId);
        }
    }
}