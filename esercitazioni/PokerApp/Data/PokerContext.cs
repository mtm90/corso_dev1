using Microsoft.EntityFrameworkCore;

namespace PokerApp.Models
{
    public class PokerContext : DbContext
    {
        public DbSet<Game> Games { get; set; }
        public DbSet<Card> Cards { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=poker.db");
        }
    }
}