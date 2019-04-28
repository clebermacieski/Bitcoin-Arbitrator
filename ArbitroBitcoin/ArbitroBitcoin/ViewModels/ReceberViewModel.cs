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
        int enderecoRecebimento = 1;
        public event PropertyChangedEventHandler PropertyChanged;
        public ReceberViewModel()
        {
            RetornaEnderecoCommand = new Command(() => EnderecoRecebimento *= 2);
        }

        public int EnderecoRecebimento
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
