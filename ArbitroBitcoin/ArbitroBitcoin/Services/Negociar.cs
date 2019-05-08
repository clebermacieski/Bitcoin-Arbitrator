using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public static void Enviar(string enderecoDestino, double valor, string enderecoArbitro = null)
        {
            Debug.WriteLine(enderecoDestino);
            Debug.WriteLine(valor);
            Debug.WriteLine(enderecoArbitro);
            /* Preciso de uma transaction id de minha posse para gerar a transação para o destino, com um valor maior que esse valor + taxa*/
            var transacaoOrigem = ExploradorBlockchain.PegarIdTransacao();
            Negociar.Enviar(enderecoDestino, valor,enderecoArbitro);


            /* Na transação origem, verificar qual outpoint bate com a respectiva private key*/

            /*Destinar*/

            /*Broadcast*/
        }
    }
}
