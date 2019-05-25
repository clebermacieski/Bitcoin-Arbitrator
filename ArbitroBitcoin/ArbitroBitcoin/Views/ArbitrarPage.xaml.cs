using ArbitroBitcoin.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ArbitroBitcoin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ArbitrarPage : ContentPage
    {
        public ArbitrarPage()
        {
            InitializeComponent();
            Title = AppResources.Arbitrar;
            botaoImportarTransacao.Text = AppResources.ImportarTransacao;
            botaoAssinar.Text = AppResources.AssinarTransacao;
            botaoModificarTransacao.Text = AppResources.ModificarTransacao;
        }

        public void OnButtonImportarTransacaoClicked(object sender, EventArgs e)
        {
            DisplayAlert("Informação", "Abrir dialogo para importar arquivo serializado com a transação a ser assinado.", "OK");
        }

        public void OnButtonAssinarClicked(object sender, EventArgs e)
        {
            DisplayAlert("Informação", "Exportar arquivo importado serializado com a transação assinada.", "OK");
        }

        public void OnButtonModificarClicked(object sender, EventArgs e)
        {
            DisplayAlert("Informação", "Exportar arquivo serializado com a transação modificada de acordo com novo consenso.", "OK");
        }
    }
}