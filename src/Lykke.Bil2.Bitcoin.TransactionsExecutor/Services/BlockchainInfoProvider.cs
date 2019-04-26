using System;
using System.Threading.Tasks;
using Lykke.Bil2.Sdk.TransactionsExecutor.Services;
using NBitcoin.RPC;
using BlockchainInfo = Lykke.Bil2.Contract.TransactionsExecutor.Responses.BlockchainInfo;

namespace Lykke.Bil2.Bitcoin.TransactionsExecutor.Services
{
    public class BlockchainInfoProvider : IBlockchainInfoProvider
    {
        private readonly RPCClient _rpcClient;

        public BlockchainInfoProvider(RPCClient rpcClient)
        {
            _rpcClient = rpcClient;
        }


        public async Task<BlockchainInfo> GetInfoAsync()
        {
            var getTime = GetLastBlockTime();
            var getHeight = GetLastBlockHeight();

            await Task.WhenAll(getTime, getHeight);
            return new BlockchainInfo(getHeight.Result, getTime.Result);
        }

        private async Task<DateTime> GetLastBlockTime()
        {
            return (await _rpcClient.GetBlockHeaderAsync(await _rpcClient.GetBestBlockHashAsync()))
                .BlockTime.DateTime;
        }

        private async Task<long> GetLastBlockHeight()
        {
            return await _rpcClient.GetBlockCountAsync();
        }
    }
    
}
