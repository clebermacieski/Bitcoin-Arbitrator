using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using NBitcoin;

namespace ArbitroBitcoin.Services
{
    /// <summary>
    /// Classe que opera dados para criar transações.
    /// </summary>
    class Negociador
    {
        public static string Receber()
        {
            Key chavePrivada = new Key();  //TODO: Persistir as PK's
            PubKey chavePublica = chavePrivada.PubKey;
            BitcoinPubKeyAddress endereco = chavePublica.GetAddress(Network.TestNet);
            return endereco.ToString();
        }

        /// <summary>
        /// Método que envia valor para para determinado endereço com a possibilidade de usar um arbitro
        /// </summary>
        /// <param name="destino">Endereço de Destino</param>
        /// <param name="valor">Quantia em BTC</param>
        /// <param name="enderecoArbitro">Endereço Do Arbitro</param>
        public static void Enviar(string destino, decimal valorAEnviar, string enderecoArbitro = null)
        {
            valorAEnviar = 0.00000001m;
            destino = "2N8MhUSTiw5JX8QCD4XoAZ2Qb5DcX9E5Qf6";
            var bitcoinSecret = RetonaPrivateKey();
            var rede = bitcoinSecret.Network;

            var transacao = Transaction.Create(rede);

            var coinsDaTransacao = ExploradorBlockchain.PegarCoins(bitcoinSecret.ScriptPubKey, rede);

            var outPointAGastar = PegarOutpointOrigem(coinsDaTransacao, bitcoinSecret.ScriptPubKey);

            transacao.Inputs.Add(new TxIn()
            {
                PrevOut = outPointAGastar
            });

            var enderecoDestino = BitcoinAddress.Create(destino, rede);
            //{2N8MhUSTiw5JX8QCD4XoAZ2Qb5DcX9E5Qf6}

            var taxaDeMineracao = new Money(0.00000100m, MoneyUnit.BTC); //Taxa fixa em um bit

            var quantiaTxInDisponivel = (Money)coinsDaTransacao[(int)outPointAGastar.N].Amount;

            var valor = new Money(valorAEnviar, MoneyUnit.BTC);

            if (quantiaTxInDisponivel < valor)
            {
                throw new Exception("Não há valor disponível para a transação.");
            }
            if (quantiaTxInDisponivel - valor < taxaDeMineracao)
            {
                throw new Exception("A quantia disponivel para envio não é suficiente para arcar com a taxa de mineração.");
            }

            var troco = quantiaTxInDisponivel - valor - taxaDeMineracao;

            TxOut txOut = new TxOut()
            {
                Value = valor,
                ScriptPubKey = enderecoDestino.ScriptPubKey
            };

            TxOut trocoTxOut = new TxOut()
            {
                Value = troco,
                ScriptPubKey = bitcoinSecret.ScriptPubKey
            };

            transacao.Outputs.Add(txOut);
            transacao.Outputs.Add(trocoTxOut);

            var mensagem = "Primeira transacao Arbitro Bitcoin!";
            var bytes = Encoding.UTF8.GetBytes(mensagem);
            transacao.Outputs.Add(new TxOut()
            {
                Value = Money.Zero,
                ScriptPubKey = TxNullDataTemplate.Instance.GenerateScriptPubKey(bytes)
            });

            transacao.Inputs[0].ScriptSig = bitcoinSecret.ScriptPubKey;

            transacao.Sign(bitcoinSecret, coinsDaTransacao.ToArray());

            ExploradorBlockchain.PropagarTransacao(transacao, bitcoinSecret.Network);
        }

        private static BitcoinSecret RetonaPrivateKey()
        {
            //TODO: Como gerenciar as private keys? Idéia: transacionar apenas consigo mesmo e ir guardando as chaves e txIds em sqllite
            
            /*Chaves privadas usadas:
            "cTQA9XcNUcS7CVtvAvz6BipKn5xzWTn3ppFTGCkEwe8QS9dVZPDw"
            */
            string chavePrivada = "cUaDX2ECmotrvVH71puhfmRHSTCjUxUtV5cUipqkMnfLGhzLKHAn"; 

            return new BitcoinSecret(chavePrivada);
        }

        private static OutPoint PegarOutpointOrigem(List<ICoin> coinsDaTransacao, Script scriptPubKey)
        {
            /* Pegar detalhes da transaction id de minha posse(pk acima) para gerar a transação para o destino, com um valor maior que esse valor + taxa*/

            //TODO: Como Gerenciar txIDs? Iterar sobre txIds disponiveis no banco, somar seus valores até ter a transação + taxa

            /* Na transação origem, verificar qual outpoint bate com a respectiva private key*/

            OutPoint outPoint = null;
            foreach (var coin in coinsDaTransacao)
            {
                if (coin.TxOut.ScriptPubKey == scriptPubKey)
                {
                    outPoint = coin.Outpoint;
                }
            }
            if (outPoint == null)
                throw new Exception("TxOut não contém o ScriptPubKey da chave privada fornecida.");

            return outPoint;
        }
    }
}
