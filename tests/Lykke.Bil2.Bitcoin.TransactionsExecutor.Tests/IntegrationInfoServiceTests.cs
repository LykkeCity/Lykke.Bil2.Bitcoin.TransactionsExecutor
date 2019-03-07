using System.Threading.Tasks;
using Lykke.Bil2.Bitcoin.TransactionsExecutor.Services;
using NUnit.Framework;
using Xunit;

namespace Lykke.Bil2.Bitcoin.TransactionsExecutor.Tests
{
    public class IntegrationInfoServiceTests
    {
        [TestCase]
        [Fact]
        public async Task Can_Run()
        {
            var serv = new IntegrationInfoService(RpcClientFactory.Create(), "https://api.github.com/repos/bitcoin/bitcoin/releases");

            var res = await serv.GetInfoAsync();

            Assert.True(res.Dependencies.ContainsKey("node"));
        }
    }
}
