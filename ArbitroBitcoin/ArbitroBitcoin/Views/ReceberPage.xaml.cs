using ArbitroBitcoin.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace ArbitroBitcoin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReceberPage : ContentPage
    {
        public ReceberPage()
        {
            //TODO: Arrumar placeholders e labels de retorno
            InitializeComponent();

            gerarEndereco.Text = AppResources.GerarEndereco;
            gerarEnderecoArbitrado.Text = AppResources.EnderecodeRecebimentoArbitrado;
            enderecoRemetenteEntry.Placeholder = AppResources.EnderecoRemetente;
            enderecoArbitroEntry.Placeholder = AppResources.EnderecoArbitro;
        }

        private void OnEnderecoLabelTapped(object sender, EventArgs e)
        {
            Clipboard.SetTextAsync(labelEndereco.Text);
            DisplayAlert("Informação", "Endereço copiado.", "OK");
        }

        private void OnEnderecoArbitradoLabelTapped(object sender, EventArgs e)
        {
            Clipboard.SetTextAsync(labelEnderecoArbitrado.Text);
            DisplayAlert("Informação", "Endereço arbitrado copiado.", "OK");
        }
    }
}