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
            Key chavePrivada = RetonaPrivateKey().PrivateKey;
            PubKey chavePublica = chavePrivada.PubKey;
            BitcoinPubKeyAddress endereco = chavePublica.GetAddress(Network.TestNet);
            return endereco.ToString();
        }

        /// <summary>
        /// Este método gera um endereço baseado em três outros endereços, dos quais 2-3 tem que assinar a fim de gastar uma transação criada para tal endereço
        /// </summary>
        /// <param name="remetente">PubKey de quem vai enviar o valor</param>
        /// <param name="arbitro">PubKey do arbitro da transação</param>
        /// <returns></returns>
        public static string ReceberArbitrado(string strPubKeyRemetente, string strPubKeyArbitro)
        {
            var segredoBitcoin = RetonaPrivateKey();
            var rede = segredoBitcoin.Network;

            PubKey pubKeyRemetente = null;
            if (strPubKeyRemetente == null) {
                pubKeyRemetente = segredoBitcoin.PubKey;
            }

            PubKey pubKeyArbitro = null ;
            if (strPubKeyArbitro == null)
            {
                pubKeyArbitro = segredoBitcoin.PubKey;
            }

            Script scriptResgate = PayToMultiSigTemplate.Instance.GenerateScriptPubKey(2, new[] { segredoBitcoin.PubKey, pubKeyRemetente, pubKeyArbitro });

            return scriptResgate.Hash.GetAddress(rede).ToString();
        }

        /// <summary>
        /// Método que envia valor para para determinado endereço com a possibilidade de usar um arbitro
        /// Se houver arbitro envolvido, cria a transação, assina e serializa para enviar para a assinatura de m-n dos participantes
        /// </summary>
        public static bool Enviar(string destino, decimal valorAEnviar)
        {
            valorAEnviar = 0.00000001m;
            destino = "2N8MhUSTiw5JX8QCD4XoAZ2Qb5DcX9E5Qf6";
            var bitcoinSecret = RetonaPrivateKey();  //TODO: Retorna private key, verifica saldo do outpoint e pega mais uma se não for o suficiente ou erro se não tiver
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

            var taxaDeMineracao = new Money(0.00008000m, MoneyUnit.BTC); //Taxa fixa em 80 bit

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

            var txId = transacao.GetHash();

            return ExploradorBlockchain.PropagarTransacao(transacao, bitcoinSecret.Network);
        }

        /// <summary>
        /// Retorna chaves Bitcoins Secrets gerados deterministicamente
        /// </summary>
        /// <returns></returns>
        private static BitcoinSecret RetonaPrivateKey()
        {

            /*Chaves privadas usadas:
            "cTQA9XcNUcS7CVtvAvz6BipKn5xzWTn3ppFTGCkEwe8QS9dVZPDw"
            */
            string chavePrivada = "cUaDX2ECmotrvVH71puhfmRHSTCjUxUtV5cUipqkMnfLGhzLKHAn";

            ExtKey extKey = new ExtKey();
            byte[] chainCode = extKey.ChainCode;
            Key chave = new BitcoinSecret(chavePrivada).PrivateKey;

            ExtKey chaveMestra = new ExtKey(chave, chainCode);

            var cont = 1;  //TODO: Implementar controle para saber em que nivel esta a geração
            ExtKey key = chaveMestra.Derive((uint)cont);

            var chavePrivadaDerivada = key.PrivateKey;

            return new BitcoinSecret(chavePrivadaDerivada, Network.TestNet); //TODO: rede hardcoded
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
        /// <summary>
        /// Método que pega saldo de determinado endereço
        /// </summary>
        /// <returns> O saldo da chave </returns>
        internal static string PegarSaldo()
        {
            string chavePrivada = "cUaDX2ECmotrvVH71puhfmRHSTCjUxUtV5cUipqkMnfLGhzLKHAn";
            var bitcoinSecret = new BitcoinSecret(chavePrivada);
            var rede = bitcoinSecret.Network;
            return ExploradorBlockchain.RetornarSaldo(bitcoinSecret, rede);
        }

        /// <summary>
        /// Método que cria transação com arbitragem 2-3 e a exporta para arquivo
        /// Este método adiciona coins, assina e serializa um objeto a fim de que seja enviado para mais um dos participantes para que a transação possa ser assinada mais uma vez,
        /// retornada, verificada e transmitida ne rede de forma validável
        /// </summary>
        /// <param name="destino">Endereço de Destino</param>
        /// <param name="valor">Quantia em BTC</param>
        public static bool Exportar(string destino, decimal valorAEnviar)
        {
            valorAEnviar = 0.00000001m;
            destino = "2N8MhUSTiw5JX8QCD4XoAZ2Qb5DcX9E5Qf6";
            var bitcoinSecret = RetonaPrivateKey();
            var rede = bitcoinSecret.Network;
            TransactionBuilder construtor = rede.CreateTransactionBuilder();

            //TODO: buildar a transação com os coins, assina-la e serializa-la a fim de que seja possível assina-la novamente a partir da tela do arbitro

            return false;

        }
    }
}
