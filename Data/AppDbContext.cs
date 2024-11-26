using Microsoft.EntityFrameworkCore;
using Battleship.Models;
using System.Numerics;

namespace Battleship.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Players> Players { get; set; }
        public DbSet<Games> Games { get; set; }
        public DbSet<Boards> Boards { get; set; }
        public DbSet<GameMoves> GameMoves { get; set; }
        public DbSet<AIStats> AIStats { get; set; }

        // Si es necesario configurar algunas relaciones específicas
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
           
        }
    }
}
