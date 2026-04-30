using System.Net.Http;
using System.Net.Http.Json;
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

                txtNome.Clear(); txtPreco.Clear();
                await CarregarDados();
                MessageBox.Show("Produto salvo na API e no banco local do WPF!");
            }
            else
            {
                MessageBox.Show("Erro ao salvar na API.");
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
    }
}