using NBitcoin;
using QBitNinja.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArbitroBitcoin.Services
{
    class ExploradorBlockchain
    {

        //Classe responsável por consultar o blockchain e trazer dados de transações.
        public static List<ICoin> PegarCoins(string txIdOrigem, Network rede)
        {
            /* Consultar e retornar Transaction Response de transação que gastarei*/

            var client = new QBitNinjaClient(rede);
            var transactionId = uint256.Parse(txIdOrigem);
            var transactionResponse = client.GetTransaction(transactionId).Result;

            return transactionResponse.ReceivedCoins;
        }

    }
}
