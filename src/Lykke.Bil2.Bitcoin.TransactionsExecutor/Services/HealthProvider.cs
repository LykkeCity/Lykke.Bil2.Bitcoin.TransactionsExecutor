using System;
using System.Threading.Tasks;
using Lykke.Bil2.Sdk.TransactionsExecutor.Services;
using NBitcoin.RPC;

namespace Lykke.Bil2.Bitcoin.TransactionsExecutor.Services
{
    public class HealthProvider : IHealthProvider
    {
        private readonly RPCClient _rpcClient;
        private readonly TimeSpan _timeout;

        public HealthProvider(RPCClient rpcClient, TimeSpan timeout)
        {
            _rpcClient = rpcClient;
            _timeout = timeout;
        }

        public async Task<string> GetDiseaseAsync()
        {
            try
            {
                var bl = _rpcClient.GetBlockHeader(await _rpcClient.GetBestBlockHashAsync());
                if (DateTime.UtcNow - bl.BlockTime.DateTime > _timeout)
                {
                    return
                        $"Node probably out of sync. Last block time: {bl.BlockTime.DateTime}. Warning timeout: {_timeout}";
                }

                return null;
            }
            catch (Exception e)
            {
                return $"Exception during interacting with node: {e}";
            }
        }
    }
}
