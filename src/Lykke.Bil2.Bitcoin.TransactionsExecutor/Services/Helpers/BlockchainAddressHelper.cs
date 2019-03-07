using System;
using NBitcoin;

namespace Lykke.Bil2.Bitcoin.TransactionsExecutor.Services.Helpers
{
    public static class BlockchainAddressHelper
    {
        public static BitcoinAddress GetBitcoinAddress(string base58Data, Network network)
        {
            try
            {
                return BitcoinAddress.Create(base58Data, network);
            }
            catch (Exception)
            {
                try
                {
                    return new BitcoinColoredAddress(base58Data, network).Address;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public static PubKey GetPubkey(string pubkey)
        {
            try
            {
                return new PubKey(pubkey);
            }
            catch
            {
                return null;
            }
        }
    }
}
