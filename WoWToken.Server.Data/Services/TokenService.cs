using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
        public async Task<Models.Database.WoWToken> GetLatestTokenInformationAsync(string region)
        {
            var latestToken = await this.wowTokenContext.Tokens
                .OrderByDescending(i => i.LastUpdatedTimestamp)
                .FirstOrDefaultAsync(i => i.Region == region);

            if (latestToken == null)
            {
                return null;
            }

            var beforeLastToken = await this.wowTokenContext.Tokens
                .OrderByDescending(i => i.LastUpdatedTimestamp)
                .FirstOrDefaultAsync(i => i.Region == region && i.LastUpdatedTimestamp != latestToken.LastUpdatedTimestamp);

            if (beforeLastToken != null)
            {
                latestToken.PriceDifference = latestToken.Price - beforeLastToken.Price;
            }
            else
            {
                latestToken.PriceDifference = 0;
            }

            return latestToken;
        }
    }
}
