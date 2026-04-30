using Microsoft.EntityFrameworkCore;
using SistemaSimulator.DTOs; 

namespace SistemaSimulator.Data
{
    public class SimulatorDbContext : DbContext
    {
        public DbSet<ProdutoDTO> LogProdutos { get; set; }

        public SimulatorDbContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite("Data Source=simulator_log.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProdutoDTO>().HasKey(p => p.Id);
        }
    }
}