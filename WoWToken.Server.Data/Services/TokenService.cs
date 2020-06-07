using System.Linq;

using WoWToken.Server.Data.Core;
using WoWToken.Server.Data.Models;

namespace WoWToken.Server.Data.Services
{
    public class TokenService : ITokenService
    {
        /// <summary>
        /// The WoW token context.
        /// </summary>
        private readonly WoWTokenContext wowTokenContext;

        /// <summary>
        /// Initialize a new instance of <see cref="TokenService" />.
        /// </summary>
        /// <param name="wowTokenContext">The WoWToken context.</param>
        public TokenService(WoWTokenContext wowTokenContext)
        {
            this.wowTokenContext = wowTokenContext;
        }

        /// <summary>
        /// Gets the latest token information for a specific region.
        /// </summary>
        /// <param name="region">The region of the token.</param>
        /// <returns>The token information.</returns>
        public Models.Database.WoWToken GetLastTokenInformation(string region)
        {
            return this.wowTokenContext.Tokens
                .OrderByDescending(i => i.LastUpdatedTimestamp)
                .First(i => i.Region == region);
        }
    }
}
