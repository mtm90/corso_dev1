using Microsoft.EntityFrameworkCore;

namespace PokerAppMVC.Models
{
    public class PokerDbContext : DbContext
    {
        public DbSet<Player> Players { get; set; }
        public DbSet<Hand> Hands { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Configure Entity Framework to use SQLite database
            optionsBuilder.UseSqlite("Data Source=pokerApp.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define the PlayerId Foreign Key relationship
            modelBuilder.Entity<Hand>()
                .HasOne(h => h.Player)
                .WithMany()
                .HasForeignKey(h => h.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
