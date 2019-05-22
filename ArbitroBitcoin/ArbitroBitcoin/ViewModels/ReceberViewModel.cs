using ArbitroBitcoin.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ArbitroBitcoin.ViewModels
{
    class ReceberViewModel : INotifyPropertyChanged
    {
        string enderecoRecebimento;
        public event PropertyChangedEventHandler PropertyChanged;
        bool podeGerar = true;
        string enderecoRecebimentoArbitrado;
        string enderecoRemetente;
        string enderecoArbitro;

        public ReceberViewModel()
        {
            RetornaEnderecoCommand = new Command(async () => await GerarEndereco(), () => podeGerar);
            RetornaEnderecoArbitradoCommand = new Command(async () => await GerarEnderecoArbitrado(), () => podeGerar);
        }

        private async Task GerarEnderecoArbitrado()
        {
            PodeGerarEndereco(false);
            await Task.Run(() => EnderecoRecebimentoArbitrado = Negociador.ReceberArbitrado(EnderecoRemetente, EnderecoArbitro));  //TODO
            PodeGerarEndereco(true);
        }

        private async Task GerarEndereco()
        {
            PodeGerarEndereco(false);
            await Task.Run(() => EnderecoRecebimento = Negociador.Receber());
            PodeGerarEndereco(true);
        }

        private void PodeGerarEndereco(bool v)
        {
            podeGerar = v;
            ((Command)RetornaEnderecoCommand).ChangeCanExecute();
        }

        public string EnderecoRecebimento
        {
            set
            {
                if (enderecoRecebimento != value)
                {
                    enderecoRecebimento = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("EnderecoRecebimento"));
                }
            }
            get
            {
                return enderecoRecebimento;
            }
        }

        public ICommand RetornaEnderecoCommand { private set; get; }
        public string EnderecoRecebimentoArbitrado
        {
            get { return enderecoRecebimentoArbitrado; }
            set
            {
                if (enderecoRecebimentoArbitrado != value)
                {
                    enderecoRecebimentoArbitrado = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("EnderecoRecebimentoArbitrado"));
                }
            }
        }
        public ICommand RetornaEnderecoArbitradoCommand { private set; get; }
        public string EnderecoRemetente { get => enderecoRemetente; set => enderecoRemetente = value; }
        public string EnderecoArbitro { get => enderecoArbitro; set => enderecoArbitro = value; }
    }
}
