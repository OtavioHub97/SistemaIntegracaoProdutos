using System;
using System.Collections.Generic;
using System.Linq; 
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows;
using SistemaInterface.Models;

namespace SistemaInterface
{
    public partial class MainWindow : Window
    {
        private HttpClient cliente = new HttpClient();

        public MainWindow()
        {
            InitializeComponent();
        }

        private async Task CarregarDados()
        {
            try
            {
                var lista = await cliente.GetFromJsonAsync<List<ProdutoDTO>>("https://localhost:7009/api/produtos");
                dgProdutos.ItemsSource = lista;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao conectar com a API: " + ex.Message);
            }
        }

        private async void btnAtualizar_Click(object sender, RoutedEventArgs e)
        {
            await CarregarDados();
        }

        private async void btnAdicionar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var novo = new ProdutoDTO
                {
                    Nome = txtNome.Text,
                    Preco = decimal.Parse(txtPreco.Text),
                    QuantidadeEstoque = 10,
                    DataCriacao = DateTime.Now
                };

                var response = await cliente.PostAsJsonAsync("https://localhost:7009/api/produtos", novo);

                if (response.IsSuccessStatusCode)
                {
                    using (var db = new WpfDbContext())
                    {
                        db.ProdutosLocal.Add(novo);
                        await db.SaveChangesAsync();
                    }

                    txtNome.Clear();
                    txtPreco.Clear();
                    await CarregarDados();
                    MessageBox.Show("Produto salvo na API e no banco local do WPF!");
                }
                else
                {
                    MessageBox.Show("Erro ao salvar na API.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao processar: " + ex.Message);
            }
        }

        private async void btnExcluir_Click(object sender, RoutedEventArgs e)
        {
            var selecionado = dgProdutos.SelectedItem as ProdutoDTO;
            if (selecionado == null) return;

            var response = await cliente.DeleteAsync($"https://localhost:7009/api/produtos/{selecionado.Id}");

            if (response.IsSuccessStatusCode)
            {
                await CarregarDados();
            }
        }
        private void btnVerHistorico_Click(object sender, RoutedEventArgs e)
        {
            var telaHistorico = new HistoricoWindow();
            telaHistorico.ShowDialog(); 
        }

        /// <summary>
        /// MÉTODO PARA O RELATÓRIO
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnRelatorio_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                /// Busca os dados da API para consolidar
                var produtos = await cliente.GetFromJsonAsync<List<ProdutoDTO>>("https://localhost:7009/api/produtos");

                if (produtos != null && produtos.Count > 0)
                {
                    /// Cria o objeto de relatório com os cálculos (Soma de estoque e valor total)
                    var novoRelatorio = new RelatorioDTO
                    {
                        DataGeracao = DateTime.Now,
                        TotalItens = produtos.Sum(p => p.QuantidadeEstoque),
                        ValorTotalEstoque = produtos.Sum(p => p.Preco * p.QuantidadeEstoque),
                        Resumo = $"Relatório gerado com {produtos.Count} tipos de produtos cadastrados."
                    };

                    ///Persistência: Salva no histórico do banco SQLite local do WPF
                    using (var db = new WpfDbContext())
                    {
                        db.HistoricoRelatorios.Add(novoRelatorio);
                        await db.SaveChangesAsync();
                    }

                    ///Exibe o relatório para o usuário
                    string msg = "RELATÓRIO GERADO E SALVO!\n\n" +
                                 $"Data: {novoRelatorio.DataGeracao:dd/MM/yyyy HH:mm}\n" +
                                 $"Tipos de Produtos: {produtos.Count}\n" +
                                 $"Total de Itens Físicos: {novoRelatorio.TotalItens}\n" +
                                 $"Valor Total em Estoque: {novoRelatorio.ValorTotalEstoque:C2}\n\n" +
                                 "O histórico foi gravado com sucesso no banco local.";

                    MessageBox.Show(msg, "Sistema de Relatórios", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Não existem produtos para gerar um relatório.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao gerar relatório: " + ex.Message);
            }
        }
    }
}