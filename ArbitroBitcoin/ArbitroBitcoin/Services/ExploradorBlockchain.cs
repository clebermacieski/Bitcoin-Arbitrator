using NBitcoin;
using QBitNinja.Client;
using QBitNinja.Client.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArbitroBitcoin.Services
{
    /// <summary>
    /// Classe responsável por consultar o blockchain e trazer dados de transações.
    /// </summary>
    class ExploradorBlockchain
    {
        /// <summary>
        /// Consultar e retornar Transaction Response de transação que gastarei.
        /// </summary>
        /// <param name="scriptPubKey"></param>
        /// <param name="rede"></param>
        /// <returns></returns>
        public static List<ICoin> PegarCoins(Script scriptPubKey, Network rede)
        {
            var client = new QBitNinjaClient(rede);
            var listaDeOperacoes = client.GetBalance(scriptPubKey).Result.Operations;

            uint256 txId = null;
            foreach (var operacao in listaDeOperacoes)
            {
                txId = operacao.TransactionId;
            }

            if (txId == null) throw new Exception("ID da transação não encontrado.");

            var transactionResponse = client.GetTransaction(txId).Result;

            return transactionResponse.ReceivedCoins;
        }

        public static bool PropagarTransacao(Transaction transacao , Network rede)
        {
            var client = new QBitNinjaClient(rede);

            BroadcastResponse broadcastResponse = client.Broadcast(transacao).Result;

            if (!broadcastResponse.Success)
            {
                Console.Error.WriteLine("ErrorCode: " + broadcastResponse.Error.ErrorCode);
                Console.Error.WriteLine("Error message: " + broadcastResponse.Error.Reason);
            }
            else
            {
                /*Console.WriteLine("Success! You can check out the hash of the transaciton in any block explorer:");
                Console.WriteLine(transacao.GetHash());*/
                return true;
            }

            return false;
        }

        public static void RetornarSaldo(BitcoinSecret segredo, Network rede)
        {
            var client = new QBitNinjaClient(rede);
            var coinsNaoGastos = new Dictionary<Coin, bool>();

            BitcoinAddress endereco = segredo.PrivateKey.ScriptPubKey.GetDestinationAddress(rede);
            var modeloDeBalanco = client.GetBalance(endereco, unspentOnly: false).Result;
            foreach (var operacoes in modeloDeBalanco.Operations)
            {
                /*if (operacoes.Confirmations) > 0){
                    foreach (var elemento in operacoes.ReceivedCoins)
                    {
                        if (elemento.
                        coinsNaoGastos.Add(elementos, operacoes.Confirmations > 0);
                    }*/
                }
                
            }
        }
    }
}
