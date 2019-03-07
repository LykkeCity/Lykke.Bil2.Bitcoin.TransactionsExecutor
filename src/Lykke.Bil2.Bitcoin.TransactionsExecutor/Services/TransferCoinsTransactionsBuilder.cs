using System.Threading.Tasks;
using Lykke.Bil2.Bitcoin.TransactionsExecutor.Services.Helpers;
using Lykke.Bil2.Contract.Common;
using Lykke.Bil2.Contract.TransactionsExecutor.Requests;
using Lykke.Bil2.Contract.TransactionsExecutor.Responses;
using Lykke.Bil2.Sdk.TransactionsExecutor.Services;
using NBitcoin;

namespace Lykke.Bil2.Bitcoin.TransactionsExecutor.Services
{
    public class TransferCoinsTransactionsBuilder : ITransferCoinsTransactionsBuilder
    {
        private readonly Network _network;

        public TransferCoinsTransactionsBuilder(Network network)
        {
            _network = network;
        }

        public Task<BuildTransactionResponse> BuildTransferCoinsAsync(BuildTransferCoinsTransactionRequest request)
        {
            var txBuilder = _network.CreateTransactionBuilder()
                .AddCoinsToSpend(request.CoinsToSpend, _network)
                .AddCoinsToReceive(request.CoinsToReceive, _network);
            
            return Task.FromResult(new BuildTransactionResponse(new Base58String(txBuilder.BuildTransaction(sign: false).ToHex())));
        }
    }
}