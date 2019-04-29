using ArbitroBitcoin.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace ArbitroBitcoin.ViewModels
{
    class EnviarViewModel : INotifyPropertyChanged
    {
        string enderecoDestino;
        double valor;
        string enderecoArbitro;

        public event PropertyChangedEventHandler PropertyChanged;

        public EnviarViewModel()
        {
            RealizarTransacaoCommand = new Command(() => Negociar.Enviar(EnderecoDestino,Valor,EnderecoArbitro));
        }

        public ICommand RealizarTransacaoCommand { get; private set; }
        public string EnderecoDestino { get => enderecoDestino; set => enderecoDestino = value; }
        public double Valor { get => valor; set => valor = value; }
        public string EnderecoArbitro { get => enderecoArbitro; set => enderecoArbitro = value; }
    }
}
