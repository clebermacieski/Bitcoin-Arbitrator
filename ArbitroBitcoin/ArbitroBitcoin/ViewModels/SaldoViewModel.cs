using ArbitroBitcoin.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArbitroBitcoin.ViewModels
{
    class SaldoViewModel
    {
        string saldo;
        public SaldoViewModel()
        {
            Saldo = Negociador.PegarSaldo();
        }

        public string Saldo { get => saldo; set => saldo = value; }
    }
}
