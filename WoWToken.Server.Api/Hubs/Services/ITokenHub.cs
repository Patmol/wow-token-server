using System.Threading.Tasks;

namespace WoWToken.Server.Api.Hubs.Services
{
    /// <summary>
    /// Contains the methods for the SignalR Hub for the token.
    /// </summary>
    public interface ITokenHub
    {
        /// <summary>
        /// Send the information for all refresh tokens.
        /// </summary>
        /// <returns>A SignalR task.</returns>
        Task SendTokens();
    }
}
