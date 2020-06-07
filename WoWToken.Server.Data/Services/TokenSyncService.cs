using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.Extensions.Options;

using Newtonsoft.Json;

using WoWToken.Server.Data.Core;
using WoWToken.Server.Data.Enumerations;
using WoWToken.Server.Data.Models;
using WoWToken.Server.Data.Models.Battlenet;

namespace WoWToken.Server.Data.Services
{
    /// <summary>
    /// Contains methods to synchronize the token information from Battle.net
    /// </summary>
    public class TokenSyncService : ITokenSyncService
    {
        /// <summary>
        /// The HTTP client.
        /// </summary>
        private readonly HttpClient client = new HttpClient();

        /// <summary>
        /// The Battle.net API URL.
        /// </summary>
        private readonly string battleNetApiUrl;

        /// <summary>
        /// The Battle.net API URL.
        /// </summary>
        private readonly string battleNetCNApiUrl;

        /// <summary>
        /// The Battle.net OAuth API URL.
        /// </summary>
        private readonly string battleNetOAuthApiUrl;
        /// <summary>
        /// The Battle.net OAuth API URL.
        /// </summary>
        private readonly string battleNetCNOAuthApiUrl;

        /// <summary>
        /// The Battle.net Client Id.
        /// </summary>
        private readonly string battleNetClientId;

        /// <summary>
        /// The Battle.net Client Secret.
        /// </summary>
        private readonly string battleNetClientSecret;

        /// <summary>
        /// The WoW token context.
        /// </summary>
        private readonly WoWTokenContext wowTokenContext;

        /// <summary>
        /// Initialize a new instance of <see cref="TokenSyncService" />.
        /// </summary>
        /// <param name="wowTokenContext">The WoWToken context.</param>
        /// <param name="settings">The application settings.</param>
        public TokenSyncService(
            WoWTokenContext wowTokenContext,
            IOptions<AppSettings> settings)
        {
            this.wowTokenContext = wowTokenContext;
            this.battleNetClientId = settings.Value.BattleNetClientId;
            this.battleNetClientSecret = settings.Value.BattleNetClientSecret;
            this.battleNetApiUrl = settings.Value.BattleNetApiUrl;
            this.battleNetCNApiUrl = settings.Value.BattleNetCNApiUrl;
            this.battleNetOAuthApiUrl = settings.Value.BattleNetOAuthApiUrl;
            this.battleNetCNOAuthApiUrl = settings.Value.BattleNetCNOAuthApiUrl;
        }

        /// <summary>
        /// Synchronize the token prices for all regions.
        /// </summary>
        /// <returns></returns>
        public async Task SyncTokenInformationAsync()
        {
            await SyncTokenGlobalInformationAsync(WoWRegion.US, WoWLocalization.NorthAmerica);
            await SyncTokenGlobalInformationAsync(WoWRegion.EU, WoWLocalization.Europe);
            await SyncTokenGlobalInformationAsync(WoWRegion.TW, WoWLocalization.Taiwan);
            await SyncTokenGlobalInformationAsync(WoWRegion.KR, WoWLocalization.Korea);
            await SyncTokenChinaInformationAsync();
        }

        /// <summary>
        /// Synchronize the token prices for a Global region.
        /// </summary>
        /// <param name="region">The region to synchronize.</param>
        /// <param name="localization">The localization of the region to synchronize.</param>
        /// <returns>A task with the result of the API call.</returns>
        private async Task SyncTokenGlobalInformationAsync(string region, string localization)
        {
            try
            {
                var token = await GetTokenGlobalAsync(client, region);

                var wowTokenResponse = await client.GetAsync(this.GetWoWTokenGlobalUrl(token.AccessToken, region, localization));
                var jsonContent = await wowTokenResponse.Content.ReadAsStringAsync();
                var wowToken = JsonConvert.DeserializeObject<Models.Battlenet.WoWToken>(jsonContent);

                await SaveWoWTokenAsync(wowToken, region);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Synchronize the token prices for China.
        /// </summary>
        /// <returns>A task with the result of the API call.</returns>
        private async Task SyncTokenChinaInformationAsync()
        {
            try
            {
                var token = await GetTokenChinaAsync(client);

                var wowTokenResponse = await client.GetAsync(this.GetWoWTokenChinaUrl(token.AccessToken));
                var jsonContent = await wowTokenResponse.Content.ReadAsStringAsync();
                var wowToken = JsonConvert.DeserializeObject<Models.Battlenet.WoWToken>(jsonContent);

                await SaveWoWTokenAsync(wowToken, WoWRegion.CN);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Save a WoW Token into the database.
        /// </summary>
        /// <param name="wowToken">The WoW Token to save.</param>
        /// <param name="region">The region of the WoW Token.</param>
        /// <returns>A task with the result of the save into the database.</returns>
        private async Task SaveWoWTokenAsync(Models.Battlenet.WoWToken wowToken, string region)
        {
            if (wowToken != null)
            {
                var latestWoWTokenInformation = this.wowTokenContext.Tokens
                    .OrderByDescending(i => i.LastUpdatedTimestamp).FirstOrDefault(i => i.Region == region);

                // We check if the latest token we have in the database is at the same timestamp, if it's the case we don't save the information.
                if (latestWoWTokenInformation == null || latestWoWTokenInformation.LastUpdatedTimestamp != wowToken.LastUpdatedTimestamp)
                {
                    await this.wowTokenContext.Tokens.AddAsync(new Models.Database.WoWToken()
                    {
                        LastUpdatedTimestamp = wowToken.LastUpdatedTimestamp,
                        Price = wowToken.Price,
                        Region = region
                    });
                    await this.wowTokenContext.SaveChangesAsync();
                }
            }
        }

        /// <summary>
        /// Gets the token to access to Battle.net Global.
        /// </summary>
        /// <param name="client">The HTTP client.</param>
        /// <param name="region">The region for the token.</param>
        /// <returns>A task with the Token.</returns>
        private async Task<Token> GetTokenGlobalAsync(HttpClient client, string region)
        {
            var regionToken = region == WoWRegion.TW ? WoWRegion.APAC : region;

            return await GetTokenAsync(client, $"https://{regionToken}.{battleNetOAuthApiUrl}");
        }

        /// <summary>
        /// Gets the token to access to Battle.net China.
        /// </summary>
        /// <param name="client">The HTTP client.</param>
        /// <returns>A task with the Token.</returns>
        private async Task<Token> GetTokenChinaAsync(HttpClient client)
        {
            return await GetTokenAsync(client, $"https://{battleNetCNOAuthApiUrl}");
        }

        /// <summary>
        /// Gets the token to access to Battle.net.
        /// </summary>
        /// <param name="tokenUrl">The URL to retrieve the token.</param>
        /// <param name="client">The HTTP client.</param>
        /// <returns>A task with the Token.</returns>
        private async Task<Token> GetTokenAsync(HttpClient client, string tokenUrl)
        {
            var form = new Dictionary<string, string>
                {
                    {"grant_type", "client_credentials"},
                    {"client_id", battleNetClientId},
                    {"client_secret", battleNetClientSecret},
                };

            var tokenResponse = await client.PostAsync(tokenUrl, new FormUrlEncodedContent(form));
            var jsonContent = await tokenResponse.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Token>(jsonContent);
        }

        /// <summary>
        /// Get the WoW token Global URL.
        /// </summary>
        /// <param name="token">The access token.</param>
        /// <param name="region">The region for the token.</param>
        /// <param name="localization">The localization for the token.</param>
        /// <returns><The token./returns>
        private string GetWoWTokenGlobalUrl(string token, string region, string localization)
            => $"https://{region}.{battleNetApiUrl}/data/wow/token/index?namespace=dynamic-{region}&locale={localization}&access_token={token}";

        /// <summary>
        /// Get the WoW token China URL.
        /// </summary>
        /// <param name="token">The access token.</param>
        /// <param name="localization">The localization for the token.</param>
        /// <returns><The token./returns>
        private string GetWoWTokenChinaUrl(string token)
            => $"https://{battleNetCNApiUrl}/data/wow/token/index?namespace=dynamic-cn&locale=zh_CN&access_token={token}";
    }
}
