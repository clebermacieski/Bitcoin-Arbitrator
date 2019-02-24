using NBitcoin;
using QBitNinja.Client;
using System;

namespace transacoes_nbitcoin
{
    internal class Gastador
    {

        public bool gastar(string txIdOrigem, string enderecoDestino)
        {
            // Private key da transação "Enviando pra mim mesmo1" que gastarei: mthg7QduM2ba3AJ5UwvQwvkuJDjLJsu9Rt
            string chavePrivada = "cTQA9XcNUcS7CVtvAvz6BipKn5xzWTn3ppFTGCkEwe8QS9dVZPDw";
            var bitcoinPrivateKey = new BitcoinSecret(chavePrivada);
            var rede = bitcoinPrivateKey.Network;
            var address = bitcoinPrivateKey.GetAddress();
            Console.WriteLine(address);

            //Consulta transação que gastarei (Um output de troco e um "Enviando pra mim mesmo1" que gastarei
            var client = new QBitNinjaClient(rede);
            var transactionId = uint256.Parse(txIdOrigem);
            var transactionResponse = client.GetTransaction(transactionId).Result;

            //Verifica outpoints da transação que gastarei
            var receivedCoins = transactionResponse.ReceivedCoins;
            OutPoint outPointToSpend = null;
            foreach (var coin in receivedCoins)
            {
                Console.WriteLine(coin.TxOut.ScriptPubKey);
                Console.WriteLine(bitcoinPrivateKey.ScriptPubKey);
                if (coin.TxOut.ScriptPubKey == bitcoinPrivateKey.ScriptPubKey)
                {
                    outPointToSpend = coin.Outpoint;
                }
            }
            if (outPointToSpend == null)
                throw new Exception("TxOut doesn't contain our ScriptPubKey");
            Console.WriteLine("We want to spend {0}. outpoint:", outPointToSpend.N + 1);

            //Criando transação e adicionando o output que quero gastar no input da transação
            var transacao= Transaction.Create(rede);
            transacao.Inputs.Add(new TxIn()
            {
                PrevOut = outPointToSpend
            });

            //Destinação
            var destino = BitcoinAddress.Create(enderecoDestino, rede);

            return false;
        }
    }
}
    
        