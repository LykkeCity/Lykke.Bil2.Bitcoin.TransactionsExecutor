using System.Collections.Generic;
using System.Threading.Tasks;
using Flurl.Http;
using Lykke.Bil2.Contract.TransactionsExecutor.Responses;
using Lykke.Bil2.Sdk.TransactionsExecutor.Services;
using Lykke.Bil2.SharedDomain;
using NBitcoin.RPC;
using Newtonsoft.Json.Linq;

namespace Lykke.Bil2.Bitcoin.TransactionsExecutor.Services
{
    public class DependencyInfoProvider : IDependenciesInfoProvider
    {
        private readonly RPCClient _rpcClient;
        private readonly string _nodeReleasesGithubUrl;

        public DependencyInfoProvider(string nodeReleasesGithubUrl, RPCClient rpcClient)
        {
            _nodeReleasesGithubUrl = nodeReleasesGithubUrl;
            _rpcClient = rpcClient;
        }

        public async Task<IReadOnlyDictionary<DependencyName, DependencyInfo>> GetInfoAsync()
        {
            var getCurrenctVers = GetCurentNodeVersion();
            var getAvailableVers = GetAvailiableNodeVersion();


            await Task.WhenAll(getCurrenctVers, getAvailableVers);

            return new Dictionary<DependencyName, DependencyInfo>
             {
                 {
                     "node",
                     new DependencyInfo(new Semver(getCurrenctVers.Result), new Semver(getAvailableVers.Result))
                 }
             };
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
    }
}
