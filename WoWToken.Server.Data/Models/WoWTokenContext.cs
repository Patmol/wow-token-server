using System;
using Microsoft.EntityFrameworkCore;

namespace WoWToken.Server.Data.Models
{
    public class WoWTokenContext : DbContext
    {
        /// <summary>
        /// Initialize a new instance of <see cref="WoWTokenContext" />.
        /// </summary>
        public WoWTokenContext()
        { }

        /// <summary>
        /// Initialize a new instance of <see cref="WoWTokenContext" />.
        /// </summary>
        /// <param name="options">Some options for the context.</param>
        public WoWTokenContext(DbContextOptions<WoWTokenContext> options)
        : base(options)
        { }

        /// <summary>
        /// Gets or sets the token DbSet.
        /// </summary>
        public DbSet<Models.Database.WoWToken> Tokens { get; set; }
    }
}
