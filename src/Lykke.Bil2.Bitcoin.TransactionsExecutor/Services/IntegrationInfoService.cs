using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Flurl.Http;
using Lykke.Bil2.Contract.TransactionsExecutor.Responses;
using Lykke.Bil2.Sdk.TransactionsExecutor.Models;
using Lykke.Bil2.Sdk.TransactionsExecutor.Services;
using NBitcoin.RPC;
using Newtonsoft.Json.Linq;
using BlockchainInfo = Lykke.Bil2.Contract.TransactionsExecutor.Responses.BlockchainInfo;

namespace Lykke.Bil2.Bitcoin.TransactionsExecutor.Services
{
    public class IntegrationInfoService : IIntegrationInfoService
    {
        private readonly RPCClient _rpcClient;
        private readonly string _nodeReleasesGithubUrl;

        public IntegrationInfoService(RPCClient rpcClient, string nodeReleasesGithubUrl)
        {
            _rpcClient = rpcClient;
            _nodeReleasesGithubUrl = nodeReleasesGithubUrl;
        }

        public async Task<IntegrationInfo> GetInfoAsync()
        {
            var getCurrenctVers = GetCurentNodeVersion();
            var getAvailableVers = GetAvailiableNodeVersion();
            var getTime = GetLastBlockTime();
            var getHeight = GetLastBlockHeight();

            await Task.WhenAll(getCurrenctVers, getAvailableVers, getTime, getHeight);

            return new IntegrationInfo(new BlockchainInfo(getHeight.Result, getTime.Result), 
                new Dictionary<string, DependencyInfo>()
            {
                {
                    "node", new DependencyInfo(new Version(getCurrenctVers.Result), new Version(getAvailableVers.Result))
                }
            } );
        }

        private async Task<string> GetCurentNodeVersion()
        {
            return (await _rpcClient.SendCommandAsync("getnetworkinfo"))
                .Result["subversion"].ToString()
                .Replace("/Satoshi:", "").Replace("/", "");
        }

        private async Task<string> GetAvailiableNodeVersion()
        {
            var httpResp = await _nodeReleasesGithubUrl
                .WithHeader("User-Agent", "smth") //github requires User-Agent
                .GetStringAsync();

            return JArray.Parse(httpResp)
                .First["tag_name"].ToString()
                .Replace("v", "");
        }

        private async Task<DateTime> GetLastBlockTime()
        {
            return (await _rpcClient.GetBlockHeaderAsync(await _rpcClient.GetBestBlockHashAsync()))
                .BlockTime.DateTime;
        }

        private Task<int> GetLastBlockHeight()
        {
            return _rpcClient.GetBlockCountAsync();
        }
    }
}
