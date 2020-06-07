using System;

namespace WoWToken.Server.Data.Core
{
    /// <summary>
    /// Contains methods to get the token information from the database.
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Gets the latest token information for a specific region.
        /// </summary>
        /// <param name="region">The region of the token.</param>
        /// <returns>The token information.</returns>
        Models.Database.WoWToken GetLastTokenInformation(string region);
    }
}
