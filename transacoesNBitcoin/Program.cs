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
            //Gastar();
            //MostrarInformacaoDeEndereco();
            RetornaSaldo();

        }

        private static void MostrarInformacaoDeEndereco()
        {
            Console.WriteLine("cTQA9XcNUcS7CVtvAvz6BipKn5xzWTn3ppFTGCkEwe8QS9dVZPDw");
            string chavePrivada = "cTQA9XcNUcS7CVtvAvz6BipKn5xzWTn3ppFTGCkEwe8QS9dVZPDw";
            var bitcoinSecret = new BitcoinSecret(chavePrivada);
            PubKey chavePublica = bitcoinSecret.PubKey;
            Console.WriteLine(chavePublica);
            BitcoinPubKeyAddress endereco = chavePublica.GetAddress(Network.TestNet);
            var client = new QBitNinjaClient(bitcoinSecret.Network);

            var listaDeOperacoes = client.GetBalance(bitcoinSecret.GetAddress()).Result.Operations;

            foreach (var operacao in listaDeOperacoes)
            {
                Console.WriteLine(operacao.TransactionId);
            }
        }

        private static void Gastar()
        {
            Gastador gastador = new Gastador();

            String txIdOrigem = "39f2fa57c3620ee44724244c2747d79ce6d392a54fdf637ab969911763deaa61"; // Transação de 0.01995480 enviados a mim mesmo

            String enderecoDestino = "mwGYn4JXyjXXtLVsJJNDKRYS95vSG6dY3o"; //Transação Enviado do meu programa.

            gastador.gastar(txIdOrigem, enderecoDestino);
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

        private static Money RetornaSaldo()
        {
            var client = new QBitNinjaClient(rede);
            var coinsNaoGastos = new Dictionary<Coin, bool>();
            string chavePrivada = "cTQA9XcNUcS7CVtvAvz6BipKn5xzWTn3ppFTGCkEwe8QS9dVZPDw";
            var segredo = new BitcoinSecret(chavePrivada);

            BitcoinAddress endereco = segredo.PrivateKey.ScriptPubKey.GetDestinationAddress(rede);
            var modeloDeBalanco = client.GetBalance(endereco, unspentOnly: true).Result;
            foreach (var operacoes in modeloDeBalanco.Operations)
            {
                if (operacoes.Confirmations > 0)
                {
                    foreach (var elemento in operacoes.ReceivedCoins.Select(coin => coin as Coin))
                    {
                       coinsNaoGastos.Add(elemento, operacoes.Confirmations > 0);
                    }
                }
            }

            var quantia = Money.Zero;

            foreach(var moeda in coinsNaoGastos)
            {
                if (moeda.Value) //Somente valores confirmados
                {
                    quantia += moeda.Key.Amount;
                }
            }

            return quantia;
        }
    }
}