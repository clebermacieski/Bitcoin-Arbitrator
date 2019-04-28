using ArbitroBitcoin.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace ArbitroBitcoin.ViewModels
{
    class ReceberViewModel : INotifyPropertyChanged
    {
        string enderecoRecebimento;
        public event PropertyChangedEventHandler PropertyChanged;
        public ReceberViewModel()
        {
            RetornaEnderecoCommand = new Command(() => EnderecoRecebimento = Negociar.Receber());
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
    }
}
