using Microsoft.EntityFrameworkCore;
using Shared; 
namespace SistemaIntegracaoProdutos.API.Data
{
   
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

       
        public DbSet<Produto> Produtos { get; set; }
    }
}