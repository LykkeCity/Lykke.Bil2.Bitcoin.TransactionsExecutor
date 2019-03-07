using System;
using System.Collections.Generic;
using System.Linq;
using Lykke.Bil2.Contract.TransactionsExecutor.Requests;
using NBitcoin;

namespace Lykke.Bil2.Bitcoin.TransactionsExecutor.Services.Helpers
{
    public static class TransactionBuilderHelper
    {
        public static TransactionBuilder AddCoinsToSpend(this TransactionBuilder transactionBuilder,
            IEnumerable<CoinToSpend> coins, Network network)
        {
            return transactionBuilder.AddCoins(coins.Select(p => ToBlockchainCoin(p, network)));
        }

        public static TransactionBuilder AddCoinsToReceive(this TransactionBuilder transactionBuilder,
            IEnumerable<CoinToReceive> coins, 
            Network network)
        {
            foreach (var coin in coins.OrderBy(p => p.CoinNumber))
            {
                if (coin.AssetId.ToString() != Constants.Bitcoin.AssetId)
                {
                    throw new ArgumentException($"Unable to send not {Constants.Bitcoin.AssetId} coin: {coin.AssetId}");
                }
                var address = BlockchainAddressHelper.GetBitcoinAddress(coin.Address.ToString(), network);
                if (address == null)
                {
                    throw new ArgumentException($"Invalid Address: {coin.Address}", nameof(coin.Address));
                }


                transactionBuilder.Send(address, new Money(coin.Value.ToDecimal(), MoneyUnit.BTC));
            }

            return transactionBuilder;
        }


        private static ICoin ToBlockchainCoin(this CoinToSpend coin, Network network)
        {
            if (coin.AssetId.ToString() != Constants.Bitcoin.AssetId)
            {
                throw new ArgumentException($"Unable to send {Constants.Bitcoin.AssetId} coin: {coin.AssetId}", nameof(coin.AssetId));
            }

            var address = BlockchainAddressHelper.GetBitcoinAddress(coin.Address.ToString(), network);
            if (address == null)
            {
                throw new ArgumentException($"Invalid Address: {coin.Address}", nameof(coin.Address));
            }

            PubKey pubKey = null;
            if (!string.IsNullOrEmpty(coin.AddressContext.ToString()))
            {
                pubKey = BlockchainAddressHelper.GetPubkey(coin.AddressContext.ToString());
                if (pubKey == null)
                {
                    throw new ArgumentException("Invalid AddressContext", nameof(coin.AddressContext));
                }
            }

            var redeem = pubKey?.WitHash.ScriptPubKey;
            var result = new Coin(new OutPoint(uint256.Parse(coin.Coin.TransactionId), coin.Coin.CoinNumber),
                new TxOut(Money.FromUnit(coin.Value.ToDecimal(), MoneyUnit.BTC), address.ScriptPubKey));

            return redeem != null ? result.ToScriptCoin(redeem) : result;
        }
    }
}
