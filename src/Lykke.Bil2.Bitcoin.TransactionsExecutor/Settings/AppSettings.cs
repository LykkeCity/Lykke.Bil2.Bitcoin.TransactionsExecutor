using System;
using Lykke.Bil2.Sdk.TransactionsExecutor.Settings;
using Lykke.SettingsReader.Attributes;

namespace Lykke.Bil2.Bitcoin.TransactionsExecutor.Settings
{
    /// <summary>
    /// Specific blockchain settings
    /// </summary>
    public class AppSettings : BaseTransactionsExecutorSettings<DbSettings>
    {
        public string Network { get; set; }

        public RpcClientSettings Rpc { get; set; }

        public TimeSpan WarningTimeoutFromLastBlock { get; set; }
       
        public int FeePerByte { get; set; } 
        
        public string NodeReleasesGithubUrl { get; set; }
    }
}
