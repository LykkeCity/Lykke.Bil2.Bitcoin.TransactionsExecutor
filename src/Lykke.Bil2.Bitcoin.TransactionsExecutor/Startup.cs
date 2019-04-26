using System;
using JetBrains.Annotations;
using Lykke.Bil2.Bitcoin.TransactionsExecutor.Services;
using Lykke.Bil2.Bitcoin.TransactionsExecutor.Services.Helpers;
using Lykke.Bil2.Bitcoin.TransactionsExecutor.Settings;
using Lykke.Bil2.Sdk.TransactionsExecutor;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using NBitcoin;

namespace Lykke.Bil2.Bitcoin.TransactionsExecutor
{
    [UsedImplicitly]
    public class Startup
    {
        private const string IntegrationName = "Bitcoin";

        [UsedImplicitly]
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            return services.BuildBlockchainTransactionsExecutorServiceProvider<AppSettings>(options =>
            {
                options.IntegrationName = IntegrationName;
                
                options.AddressValidatorFactory = ctx =>
                    new AddressValidator
                    (
                        Network.GetNetwork(ctx.Settings.CurrentValue.Network)
                    );

                options.HealthProviderFactory = ctx =>
                    new HealthProvider
                    (
                        ctx.Settings.CurrentValue.CreateRpcClient(),
                        ctx.Settings.CurrentValue.WarningTimeoutFromLastBlock
                    );

                options.DependenciesInfoProviderFactory = ctx =>
                    new DependencyInfoProvider
                    (
                        ctx.Settings.CurrentValue.NodeReleasesGithubUrl,
                        ctx.Settings.CurrentValue.CreateRpcClient()
                    );

                options.BlockchainInfoProviderFactory = ctx =>
                    new BlockchainInfoProvider
                    (
                        ctx.Settings.CurrentValue.CreateRpcClient()
                    );

                options.TransactionBroadcasterFactory = ctx =>
                    new TransactionBroadcaster
                    (
                        ctx.Settings.CurrentValue.CreateRpcClient()
                    );

                options.TransactionsStateProviderFactory = ctx =>
                    new TransactionsStateProvider
                    (
                        ctx.Settings.CurrentValue.CreateRpcClient()
                    );

                options.TransferCoinsTransactionsBuilderFactory = ctx =>
                    new TransferCoinsTransactionsBuilder(Network.GetNetwork(ctx.Settings.CurrentValue.Network));

                options.TransferCoinsTransactionsEstimatorFactory = ctx =>
                    new TransferCoinsTransactionsEstimator
                    (
                        Network.GetNetwork(ctx.Settings.CurrentValue.Network),
                        ctx.Settings.CurrentValue.FeePerByte
                    );

                //TODO Format provider
            });
        }

        [UsedImplicitly]
        public void Configure(IApplicationBuilder app)
        {
            app.UseBlockchainTransactionsExecutor(options =>
            {
                options.IntegrationName = IntegrationName;
            });
        }
    }
}
