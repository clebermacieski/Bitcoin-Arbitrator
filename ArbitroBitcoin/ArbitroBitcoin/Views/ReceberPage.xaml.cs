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
    public partial class ReceberPage : ContentPage
    {
        public ReceberPage()
        {
            InitializeComponent();

            gerarEndereco.Text = AppResources.GerarEndereco;
            enderecoRecebimento.Text = AppResources.EnderecodeRecebimento;
        }

        void OnGerarEnderecoButtonClicked(object sender, EventArgs e)
        {
            //DisplayAlert("Alert", "Botão clicado", "OK");

        }
    }
}