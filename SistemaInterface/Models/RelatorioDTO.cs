using System;

namespace SistemaInterface.Models
{
    public class RelatorioDTO
    {
        public int Id { get; set; }
        public DateTime DataGeracao { get; set; }
        public int TotalItens { get; set; }
        public decimal ValorTotalEstoque { get; set; }
        public string Resumo { get; set; }
    }
}