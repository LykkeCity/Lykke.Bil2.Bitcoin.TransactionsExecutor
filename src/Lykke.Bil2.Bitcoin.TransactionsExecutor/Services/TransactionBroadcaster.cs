using System.Threading.Tasks;
using Lykke.Bil2.Contract.TransactionsExecutor.Requests;
using Lykke.Bil2.Sdk.TransactionsExecutor.Services;
using NBitcoin;
using NBitcoin.RPC;

namespace Lykke.Bil2.Bitcoin.TransactionsExecutor.Services
{
    public class TransactionBroadcaster : ITransactionBroadcaster
    {
        private readonly RPCClient _rpcClient;

        public TransactionBroadcaster(RPCClient rpcClient)
        {
            _rpcClient = rpcClient;
        }

        public Task BroadcastAsync(BroadcastTransactionRequest request)
        {
            return _rpcClient.SendRawTransactionAsync(Transaction.Parse(request.SignedTransaction.DecodeToString(),
                _rpcClient.Network));
        }
    }
}