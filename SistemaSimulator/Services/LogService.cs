using SistemaSimulator.Data;
using SistemaSimulator.DTOs;

namespace SistemaSimulator.Services
{
    public static class LogService
    {
        public static void SalvarLogLocal(ProdutoDTO produto)
        {
            try
            {
                using (var db = new SimulatorDbContext())
                {
                    db.LogProdutos.Add(produto);
                    db.SaveChanges();
                }
                Console.WriteLine($"[LOG LOCAL] Sucesso: {produto.Nome} persistido no banco do simulador.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERRO LOG] Falha ao salvar no banco local: {ex.Message}");
            }
        }
    }
}