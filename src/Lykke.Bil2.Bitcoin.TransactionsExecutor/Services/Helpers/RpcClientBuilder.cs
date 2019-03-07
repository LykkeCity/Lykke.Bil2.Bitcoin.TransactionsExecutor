using System;
using System.Net;
using Lykke.Bil2.Bitcoin.TransactionsExecutor.Settings;
using NBitcoin;
using NBitcoin.RPC;

namespace Lykke.Bil2.Bitcoin.TransactionsExecutor.Services.Helpers
{
    public static class RpcClientBuilder
    {
        public static RPCClient CreateRpcClient(this AppSettings settings)
        {
            return new RPCClient(
                new NetworkCredential(settings.Rpc.UserName, settings.Rpc.Password),
                new Uri(settings.Rpc.Host),
                Network.GetNetwork(settings.Network));
        }
    }
}
