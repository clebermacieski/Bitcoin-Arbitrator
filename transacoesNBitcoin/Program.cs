using NBitcoin;
using QBitNinja.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace transacoes_nbitcoin
{

    class Program
    {
        private static Network rede = Network.TestNet;

        static void Main(string[] args)
        {
            // retornaInfoTransacao();
            Gastar();

        }

        private static void Gastar()
        {
            Gastador gastador = new Gastador();

            String txIdOrigem = "39f2fa57c3620ee44724244c2747d79ce6d392a54fdf637ab969911763deaa61"; // Transação de 0.01995480 enviados a mim mesmo

            String enderecoDestino = "mwGYn4JXyjXXtLVsJJNDKRYS95vSG6dY3o"; //Transação Enviado do meu programa.

            gastador.gastar(txIdOrigem , enderecoDestino);
        }








        private static void retornaInfoTransacao()
        {
            void recebidos(List<ICoin> moedasRecebidas)
            {
                foreach (var moeda in moedasRecebidas)
                {
                    Money montante = (Money)moeda.Amount;

                    Console.WriteLine("Montante= " + montante.ToDecimal(MoneyUnit.BTC));
                    var scriptDePagamento2 = moeda.TxOut.ScriptPubKey;
                    Console.WriteLine("Script de pagamento= " + scriptDePagamento2);
                    var endereco = scriptDePagamento2.GetDestinationAddress(Network.Main);
                    Console.WriteLine("Endereço= " + endereco);
                    }
            }

            void consultaRede(string transacao)
            {
                if (transacao == null)
                {
                    throw new ArgumentNullException(nameof(transacao));
                }

                var cliente = new QBitNinjaClient(rede);

                var transactionId = uint256.Parse(transacao);
                var transactionResponse = cliente.GetTransaction(transactionId).Result;

                Console.WriteLine(transactionResponse.TransactionId); // O próprio ID
                Console.WriteLine(transactionResponse.Block.Confirmations); // Número de confirmações
                recebidos(transactionResponse.ReceivedCoins);

            }

            String txid = "39f2fa57c3620ee44724244c2747d79ce6d392a54fdf637ab969911763deaa61";

            consultaRede(txid);
            
        }
    }
}
