using System.Threading.Tasks;
using Lykke.Bil2.Bitcoin.TransactionsExecutor.Services.Helpers;
using Lykke.Bil2.Contract.TransactionsExecutor.Responses;
using Lykke.Bil2.Sdk.TransactionsExecutor.Services;
using Lykke.Bil2.SharedDomain;
using NBitcoin;

namespace Lykke.Bil2.Bitcoin.TransactionsExecutor.Services
{
    public class AddressValidator : IAddressValidator
    {
        private readonly Network _network;

        public AddressValidator(Network network)
        {
            _network = network;
        }

        public Task<AddressValidityResponse> ValidateAsync(string address, AddressTagType? tagType = null, string tag = null)
        {
            var addr = BlockchainAddressHelper.GetBitcoinAddress(address, _network);

            var  res = addr != null
                ? AddressValidationResult.Valid 
                : AddressValidationResult.InvalidAddressFormat;

            return  Task.FromResult(new AddressValidityResponse(res));
        }
    }
}
