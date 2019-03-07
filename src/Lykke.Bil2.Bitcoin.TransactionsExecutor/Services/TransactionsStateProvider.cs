using System.Threading.Tasks;
using Lykke.Bil2.Contract.TransactionsExecutor;
using Lykke.Bil2.Sdk.TransactionsExecutor.Services;
using NBitcoin;
using NBitcoin.RPC;

namespace Lykke.Bil2.Bitcoin.TransactionsExecutor.Services
{
    public class TransactionsStateProvider : ITransactionsStateProvider
    {
        private readonly RPCClient _rpcClient;

        public TransactionsStateProvider(RPCClient rpcClient)
        {
            _rpcClient = rpcClient;
        }

        public async Task<TransactionState> GetStateAsync(string transactionId)
        {
            RawTransactionInfo tx;
            try
            {
                tx = await _rpcClient.GetRawTransactionInfoAsync(uint256.Parse(transactionId));
            }
            catch (RPCException e) when(e.RPCCode == RPCErrorCode.RPC_INVALID_ADDRESS_OR_KEY)
            {
                return TransactionState.Unknown;
            }

            if (tx.Confirmations == 0)
            {
                return TransactionState.Broadcasted;
            }
            
            return TransactionState.Mined;
        }
    }
}