using ArbitroBitcoin.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ArbitroBitcoin.ViewModels
{
    class SaldoViewModel
    {
        string saldo;
        public SaldoViewModel()
        {
            PegaSaldoAsync();
        }

        private async void PegaSaldoAsync()
        {
            await Task.Run(() => Saldo = Negociador.PegarSaldo() );
        }

        public string Saldo { get => saldo; set => saldo = value; }
    }
}
