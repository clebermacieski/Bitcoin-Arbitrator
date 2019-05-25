using ArbitroBitcoin.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace ArbitroBitcoin.ViewModels
{
    class SaldoViewModel
    {
        string saldo;
        public event PropertyChangedEventHandler PropertyChanged;
        public SaldoViewModel()
        {
            PegaSaldoAsync();
        }

        private async void PegaSaldoAsync()
        {
            await Task.Run(() => Saldo = Negociador.PegarSaldo() );
        }

        public string Saldo {
            get {
                return saldo;
                    }
            set {
                if (saldo != value)
                {
                    saldo = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Saldo"));
                }
            }
        }
    }
}
