using ArbitroBitcoin.Resources;
using System;
using System.Windows.Input;

using Xamarin.Forms;

namespace ArbitroBitcoin.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            Title = AppResources.Sobre;

            OpenWebCommand = new Command(() => Device.OpenUri(new Uri("https://xamarin.com/platform")));
        }

        public ICommand OpenWebCommand { get; }
    }
}