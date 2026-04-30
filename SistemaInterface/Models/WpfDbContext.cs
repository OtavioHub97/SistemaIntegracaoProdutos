using Microsoft.EntityFrameworkCore;

namespace SistemaInterface.Models
{
    public class WpfDbContext : DbContext
    {
        // Tabela de Produtos Local
        public DbSet<ProdutoDTO> ProdutosLocal { get; set; }

        // Tabela para salvar o histórico de relatórios gerados
        public DbSet<RelatorioDTO> HistoricoRelatorios { get; set; }

        public WpfDbContext()
        {
            // Garante que o banco e as tabelas sejam criados onde o app estiver rodando
            Database.Migrate();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite("Data Source=wpf_local.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define a chave primária para Produtos
            modelBuilder.Entity<ProdutoDTO>().HasKey(p => p.Id);

            // Define a chave primária para os Relatórios
            modelBuilder.Entity<RelatorioDTO>().HasKey(r => r.Id);
        }
    }
}