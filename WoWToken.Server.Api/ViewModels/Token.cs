using System.Text.Json.Serialization;

namespace WoWToken.Server.Api.ViewModels
{
    /// <summary>
    /// The token view model.
    /// </summary>
    public class Token
    {
        /// <summary>
        /// Gets or sets the unique identifier key.
        /// </summary>
        [JsonPropertyName("id")]
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the WoW region for the token.
        /// </summary>
        [JsonPropertyName("region")]
        public string Region { get; set; }

        /// <summary>
        /// Gets or sets the last updated timestamp.
        /// </summary>
        [JsonPropertyName("lastUpdatedTimestamp")]
        public long LastUpdatedTimestamp { get; set; }

        /// <summary>
        /// Gets or sets the price of the token in copper.
        /// </summary>
        [JsonPropertyName("price")]
        public long Price { get; set; }

        /// <summary>
        /// Gets or sets the price difference between two tokens.
        /// </summary>
        [JsonPropertyName("priceDifference")]
        public long PriceDifference { get; set; }
    }
}
