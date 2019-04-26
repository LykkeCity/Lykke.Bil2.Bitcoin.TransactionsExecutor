using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using Lykke.Bil2.Bitcoin.TransactionsExecutor.Services.Helpers;
using Lykke.Bil2.Contract.TransactionsExecutor.Requests;
using Lykke.Bil2.Contract.TransactionsExecutor.Responses;
using Lykke.Bil2.Sdk.TransactionsExecutor.Services;
using Lykke.Bil2.SharedDomain;
using Lykke.Numerics;
using NBitcoin;
using Constants = Lykke.Bil2.Bitcoin.TransactionsExecutor.Services.Helpers.Constants;
using Money = NBitcoin.Money;

namespace Lykke.Bil2.Bitcoin.TransactionsExecutor.Services
{
    public class TransferCoinsTransactionsEstimator : ITransferCoinsTransactionsEstimator
    {
        private readonly Network _network;
        private readonly FeeRate _feeRate;

        public TransferCoinsTransactionsEstimator(Network network, int feePerByte)
        {
            _network = network;
            _feeRate = new FeeRate(new Money(feePerByte *1024, MoneyUnit.Satoshi));
        }

        public Task<EstimateTransactionResponse> EstimateTransferCoinsAsync(EstimateTransferCoinsTransactionRequest request)
        {
            var txBuilder = _network.CreateTransactionBuilder()
                .AddCoinsToSpend(request.CoinsToSpend, _network)
                .AddCoinsToReceive(request.CoinsToReceive, _network);

            var fee = txBuilder.EstimateFees(txBuilder.BuildTransaction(false), _feeRate);

            return Task.FromResult(new EstimateTransactionResponse(new List<Fee>
            {
                new Fee(new Asset(new AssetId(Constants.Bitcoin.AssetId)),
                    new UMoney(new BigInteger(fee.ToUnit(MoneyUnit.BTC)), Constants.Bitcoin.Accuracy))
            }));
        }
    }
}