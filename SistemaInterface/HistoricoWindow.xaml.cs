using System.Linq;
using System.Windows;
using SistemaInterface.Models;

namespace SistemaInterface
{
    public partial class HistoricoWindow : Window
    {
        public HistoricoWindow()
        {
            InitializeComponent();
            CarregarRelatorios();
        }

        private void CarregarRelatorios()
        {
            using (var db = new WpfDbContext())
            {
                dgHistorico.ItemsSource = db.HistoricoRelatorios.ToList();
            }
        }
    }
}