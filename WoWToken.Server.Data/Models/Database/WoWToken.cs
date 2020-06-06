using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WoWToken.Server.Data.Enumerations;

namespace WoWToken.Server.Data.Models.Database
{
    /// <summary>
    /// The [dbo].[Token] table
    /// </summary>
    [Table("WoWToken", Schema = "dbo")]
    public class WoWToken
    {
        /// <summary>
        /// Gets or sets the unique identifier key.
        /// </summary>
        [Key]
        [Column("Id")]
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets te WoW region for the token.
        /// </summary>
        [Column("Region")]
        public string Region { get; set; }

        /// <summary>
        /// Gets or sets the last updated timestamp.
        /// </summary>
        [Column("LastUpdatedTimestamp")]
        public long LastUpdatedTimestamp { get; set; }

        /// <summary>
        /// Gets or sets the price of the token.
        /// </summary>
        [Column("Price")]
        public long Price { get; set; }
    }
}
