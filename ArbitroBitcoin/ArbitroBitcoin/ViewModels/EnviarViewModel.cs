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
            RealizarTransacaoCommand = new Command(() => Negociar.Enviar(EnderecoDestino, Valor, EnderecoArbitro));
        }

        public ICommand RealizarTransacaoCommand { get; private set; }
        public string EnderecoDestino
        {
            set
            {
                if (enderecoDestino != value)
                {
                    enderecoDestino = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("EnderecoDestino"));
                }
            }
            get
            {
                return enderecoDestino;
            }
        }
        public double Valor {
            set
            {
                if ( valor != value)
                {
                    valor = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Valor"));
                }
            }
            get
            {
                return valor;
            }
        }
        public string EnderecoArbitro
        {
            set
            {
                if (enderecoArbitro != value)
                {
                    enderecoArbitro = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("EnderecoArbitro"));
                }
            }
            get
            {
                return enderecoArbitro;
            }
        }
    }
}
