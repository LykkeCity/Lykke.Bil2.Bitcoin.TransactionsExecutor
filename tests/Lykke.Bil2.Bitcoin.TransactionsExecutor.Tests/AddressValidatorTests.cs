using System;
using System.Threading.Tasks;
using Lykke.Bil2.Bitcoin.TransactionsExecutor.Services;
using Lykke.Bil2.Contract.TransactionsExecutor;
using NBitcoin;
using NUnit.Framework;
using Xunit;

namespace Lykke.Bil2.Bitcoin.TransactionsExecutor.Tests
{
    public class AddressValidatorTests
    {
        [Fact]
        [TestCase("invalidAddress", "mainnet", "InvalidAddressFormat")]
        [TestCase("bitcoincash:qrxa5m5zt8jhm7r39d35m6acgfunp4uhquqwy40pte", "mainnet", "InvalidAddressFormat")] //bch addr
        [TestCase("simpleledger:qrxa5m5zt8jhm7r39d35m6acgfunp4uhquv40w6p48", "mainnet", "InvalidAddressFormat")] //bch addr
        [TestCase("mx1j4TBbJmqbGGxYdbfYfFx5weiMosyvLD", "mainnet", "InvalidAddressFormat")] //ltc addr
        [TestCase("mvq249kND1czVbgbNVYbEWszW5PVeXon6u", "mainnet", "InvalidAddressFormat")]
        [TestCase("1KmTCE1Ap2dWamiMzFiUT2AdNPnRVoruTe", "mainnet", "Valid")]
        [TestCase("19xM6HywehvSYfPvf3C8JVZPfE7zh1ziCD", "mainnet", "Valid")]
        [TestCase("3EaqaJX29c287VgL4boqt8ZgwjwoedVadj", "mainnet", "Valid")] // segwit
        [TestCase("akNH28qctDNpwMKKJgbLURvBHTLKQBoUGJv", "mainnet", "Valid")] // colored address
        [TestCase("mvq249kND1czVbgbNVYbEWszW5PVeXon6u", "testnet", "Valid")]
        public async Task Can_Validate_Address(string invalidAddress, string network, string result)
        {
            var addressValidator = new AddressValidator(Network.GetNetwork(network));

            var res = (await addressValidator.ValidateAsync(invalidAddress)).Result;

            Assert.AreEqual(Enum.Parse<AddressValidationResult>(result), res);
        }
    }
}