using GameStore.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Data;

public class GameStoreContext(DbContextOptions<GameStoreContext> options) : DbContext(options)
{
    public DbSet<Game> Games => Set<Game>();
    public DbSet<Genre> Genres => Set<Genre>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Genre>().HasData(
            new { Id = 1, Name = "Action" },
            new { Id = 2, Name = "RPG" },
            new { Id = 3, Name = "Shooter" },
            new { Id = 4, Name = "Sports" },
            new { Id = 5, Name = "Strategy" },
            new { Id = 6, Name = "Survival" }
            );
    }

}
