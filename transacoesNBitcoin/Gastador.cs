using NBitcoin;
using QBitNinja.Client;
using System;
using System.Text;

namespace transacoes_nbitcoin
{
    internal class Gastador
    {
        private Transaction transacao;

        public bool gastar(string txIdOrigem, string enderecoDestino)
        {
            // Private key da transação "Enviando pra mim mesmo1" que gastarei: mthg7QduM2ba3AJ5UwvQwvkuJDjLJsu9Rt
            string chavePrivada = "cTQA9XcNUcS7CVtvAvz6BipKn5xzWTn3ppFTGCkEwe8QS9dVZPDw";
            var bitcoinPrivateKey = new BitcoinSecret(chavePrivada);
            var rede = bitcoinPrivateKey.Network;
            var address = bitcoinPrivateKey.GetAddress();
            Console.WriteLine("Endereço de origem dos fundos: "+address);

            //Consulta transação que gastarei (Um output de troco e um "Enviando pra mim mesmo1" que gastarei
            var client = new QBitNinjaClient(rede);
            var transactionId = uint256.Parse(txIdOrigem);
            var transactionResponse = client.GetTransaction(transactionId).Result; //?

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
            transacao= Transaction.Create(rede);
            transacao.Inputs.Add(new TxIn()
            {
                PrevOut = outPointToSpend
            });

            //Destinação
            var destino = BitcoinAddress.Create(enderecoDestino, rede);


            //Criação do Output
            TxOut txOut = new TxOut()
            {
                Value = new Money(0.0004m, MoneyUnit.BTC),
                ScriptPubKey = destino.ScriptPubKey
            };

            TxOut trocoTxOut = new TxOut()
            {
                Value = new Money(0.00053m, MoneyUnit.BTC),
                ScriptPubKey = bitcoinPrivateKey.ScriptPubKey
            };

            transacao.Outputs.Add(txOut);
            transacao.Outputs.Add(trocoTxOut);
            
            //Mensagem na transação
            var mensagem = "Olá Mundo!";
            var bytes = Encoding.UTF8.GetBytes(mensagem);
            transacao.Outputs.Add(new TxOut()
            {
                Value = Money.Zero,
                ScriptPubKey = TxNullDataTemplate.Instance.GenerateScriptPubKey(bytes)
            });

            Console.WriteLine("\n"+transacao);

            //Assinando a transação

            //Pegando pubkey script do endereço de origem
            var endereco = BitcoinAddress.Create(txIdOrigem, rede);
            transacao.Inputs[0].ScriptSig = endereco.ScriptPubKey;

            //assinando com a private key referente ao pubkey script da origem
            transacao.Sign(bitcoinPrivateKey, receivedCoins.ToArray());

            Console.WriteLine(transacao);


            //Propagar a transação...
            return false;
        }
    }
}
    
        