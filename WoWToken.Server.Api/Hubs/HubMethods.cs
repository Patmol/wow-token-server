using System;
namespace WoWToken.Server.Api.Hubs
{
    /// <summary>
    /// Contains the name of the methods for the SignalR hubs.
    /// </summary>
    public class HubMethods
    {
        /// <summary>
        /// The name of the SignalR method to refresh the tokens.
        /// </summary>
        public const string UpdateTokenPrice = "UpdateTokenPrice";
    }
}
