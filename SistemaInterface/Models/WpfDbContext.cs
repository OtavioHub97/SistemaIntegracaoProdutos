using Microsoft.EntityFrameworkCore;

namespace SistemaInterface.Models
{
    public class WpfDbContext : DbContext
    {
        public DbSet<ProdutoDTO> ProdutosLocal { get; set; }
        public WpfDbContext()
        {
            Database.Migrate();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite("Data Source=wpf_local.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProdutoDTO>().HasKey(p => p.Id);
        }
    }
}