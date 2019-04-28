using System;
using System.Collections.Generic;
using System.Text;
using NBitcoin;

namespace ArbitroBitcoin.Services
{
    class Negociar
    {
        public static string Receber()
        {
            Key chavePrivada = new Key();
            PubKey chavePublica = chavePrivada.PubKey;
            BitcoinPubKeyAddress endereco = chavePublica.GetAddress(Network.TestNet);
            return endereco.ToString();
        }
    }
}
