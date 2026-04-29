namespace SistemaIntegracaoProdutos.API.DTOs
{
    /// <summary>
    /// Objeto de Transferência de Dados para o Produto.
    /// Utilizado para receber dados da Interface ou do Simulador sem expor a Entidade completa.
    /// </summary>
    public class ProdutoDTO
    {
        public string Nome { get; set; } = string.Empty;
        public decimal Preco { get; set; }
        public int QuantidadeEstoque { get; set; }
    }
}