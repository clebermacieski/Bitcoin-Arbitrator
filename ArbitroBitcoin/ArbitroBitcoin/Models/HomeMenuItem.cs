using System;
using System.Collections.Generic;
using System.Text;

namespace ArbitroBitcoin.Models
{
    public enum MenuItemType
    {
        Browse
       , Receber
       , Enviar
       , Arbitrar
       , About
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}
