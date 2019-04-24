using ArbitroBitcoin.Models;
using ArbitroBitcoin.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ArbitroBitcoin.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(true)]
    public partial class MenuPage : ContentPage
    {
        MainPage RootPage { get => Application.Current.MainPage as MainPage; }
        List<HomeMenuItem> menuItems;
        public MenuPage()
        {
            InitializeComponent();

            menuItems = new List<HomeMenuItem>
            {
                new HomeMenuItem {Id = MenuItemType.Browse, Title=AppResources.ArbitroBitcoin },
                new HomeMenuItem {Id = MenuItemType.Receber, Title=AppResources.Receber },
                new HomeMenuItem {Id = MenuItemType.Enviar, Title=AppResources.Enviar },
                new HomeMenuItem {Id = MenuItemType.Arbitrar, Title=AppResources.Arbitrar },
                new HomeMenuItem {Id = MenuItemType.About, Title=AppResources.Sobre }
            };

            ListViewMenu.ItemsSource = menuItems;

            ListViewMenu.SelectedItem = menuItems[0];
            ListViewMenu.ItemSelected += async (sender, e) =>
            {
                if (e.SelectedItem == null)
                    return;

                var id = (int)((HomeMenuItem)e.SelectedItem).Id;
                await RootPage.NavigateFromMenu(id);
            };
        }
    }
}