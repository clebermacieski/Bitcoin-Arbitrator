using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using NBitcoin;

namespace ArbitroBitcoin.Services
{
    //Classe que opera dados retornados do blockchain
    class Negociador
    {
        private static Network rede = Network.TestNet;
        public static string Receber()
        {
            Key chavePrivada = new Key();
            PubKey chavePublica = chavePrivada.PubKey;
            BitcoinPubKeyAddress endereco = chavePublica.GetAddress(Network.TestNet);
            return endereco.ToString();
        }

        public static void Enviar(string enderecoDestino, double valor, string enderecoArbitro = null)
        {
            //TODO: Como gerenciar as private keys? Idéia: transacionar apenas consigo mesmo e ir guardando as chaves e txIds em sqllite
            string chavePrivada = "cTQA9XcNUcS7CVtvAvz6BipKn5xzWTn3ppFTGCkEwe8QS9dVZPDw";
            var bitcoinPrivateKey = new BitcoinSecret(chavePrivada);
            var rede = bitcoinPrivateKey.Network;
            var address = bitcoinPrivateKey.GetAddress();

            /* Preciso de uma transaction id de minha posse para gerar a transação para o destino, com um valor maior que esse valor + taxa*/

            //TODO: Como Gerenciar txIDs? Iterar sobre txIds disponiveis no banco, somar seus valores até ter a transação + taxa
             var receivedCoins = ExploradorBlockchain.PegarCoins("39f2fa57c3620ee44724244c2747d79ce6d392a54fdf637ab969911763deaa61", rede);

            /* Na transação origem, verificar qual outpoint bate com a respectiva private key*/

            OutPoint outPointToSpend = null;
            foreach (var coin in receivedCoins)
            {
                if (coin.TxOut.ScriptPubKey == bitcoinPrivateKey.ScriptPubKey)
                {
                    outPointToSpend = coin.Outpoint;
                }
            }
            if (outPointToSpend == null)
                throw new Exception("TxOut não contém o ScriptPubKey da chave privada fornecida.");

            //Criando transação com base nessa origem
            Transaction transacao = Transaction.Create(rede);
            transacao.Inputs.Add(new TxIn()
            {
                PrevOut = outPointToSpend
            });


            /*Destinar*/
            var destino = BitcoinAddress.Create(enderecoDestino, rede);

            //Valorar
            var quantiaTxIn = (Money)receivedCoins[(int)outPointToSpend.N].Amount;
            //Todo: verificar se quantia é suficiente
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
            /*Broadcast*/
        }
    }
}
