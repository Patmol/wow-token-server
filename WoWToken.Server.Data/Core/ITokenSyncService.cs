using System.Threading.Tasks;

namespace WoWToken.Server.Data.Core
{
    /// <summary>
    /// Contains methods to synchronize the token information from Battle.net
    /// </summary>
    public interface ITokenSyncService
    {
        /// <summary>
        /// Synchronize the token prices for all regions.
        /// </summary>
        /// <returns></returns>
        Task SyncTokenInformationAsync();
    }
}