using System;
namespace WoWToken.Server.Data.Models
{
    /// <summary>
    /// Contains the application settings.
    /// </summary>
    public class AppSettings
    {
        /// <summary>
        /// Gets or sets the Battle.net Client Id for the Battle.net API
        /// </summary>
        public virtual string BattleNetClientId { get; set; }

        /// <summary>
        /// Gets or sets the Battle.net Client Secret for the Battle.net API
        /// </summary>
        public virtual string BattleNetClientSecret { get; set; }

        /// <summary>
        /// Gets or sets the Battle.net Global Api URL.
        /// </summary>
        public virtual string BattleNetApiUrl { get; set; }

        /// <summary>
        /// Gets or sets the Battle.net China Api URL.
        /// </summary>
        public virtual string BattleNetCNApiUrl { get; set; }

        /// <summary>
        /// Gets or sets the Battle.net OAuth Global Api URL.
        /// </summary>
        public virtual string BattleNetOAuthApiUrl { get; set; }

        /// <summary>
        /// Gets or sets the Battle.net OAuth China Api URL.
        /// </summary>
        public virtual string BattleNetCNOAuthApiUrl { get; set; }
    }
}
