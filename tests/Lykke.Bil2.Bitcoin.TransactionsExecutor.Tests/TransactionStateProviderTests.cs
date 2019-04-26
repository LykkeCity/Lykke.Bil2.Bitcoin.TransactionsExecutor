using System;
using System.Threading.Tasks;
using Lykke.Bil2.Bitcoin.TransactionsExecutor.Services;
using Lykke.Bil2.SharedDomain;
using NUnit.Framework;

namespace Lykke.Bil2.Bitcoin.TransactionsExecutor.Tests
{
    public class TransactionStateProviderTests
    {
        [TestCase("964e9c164564ef965d1d99683569c54c95463447388415426112a5c5ed2fce92", "Mined")]
        [TestCase("21bca4b9cd8b401fdfe1304cba8bbd46a2b78fdedfd791d1443d236a181b5974 ", "Unknown")]
        public async Task Can_Estimate_Transaction_State(string txHash, string state)
        {
            var prov = new TransactionsStateProvider(RpcClientFactory.Create());

            var res = await prov.GetStateAsync(txHash);

            Assert.AreEqual(Enum.Parse<TransactionState>(state), res);
        }
    }
}
