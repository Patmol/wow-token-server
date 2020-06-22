using System;
using System.Threading.Tasks;

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using WoWToken.Server.Data.Models;
using WoWToken.Server.Data.Services;

namespace WoWToken.Server.Tests.Services
{
    /// <summary>
    /// Test the Token Service.
    /// </summary>
    [TestClass]
    public class TokenServiceTest
    {
        /// <summary>
        /// Test the service to get the latest token information with only two tokens
        ///   in the database for the same region.
        /// </summary>
        [TestMethod]
        public async Task GetLatestTokenInformationOneRegionTwoTokens()
        {
            var connection = new SqliteConnection("DataSource=:memory");

            try
            {
                await connection.OpenAsync();

                var options = new DbContextOptionsBuilder<WoWTokenContext>()
                    .UseSqlite(connection)
                    .Options;

                using (var context = new WoWTokenContext(options))
                {
                    context.Tokens.RemoveRange(await context.Tokens.ToListAsync());
                    context.SaveChanges();

                    var latestToken = new Data.Models.Database.WoWToken()
                    {
                        LastUpdatedTimestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds(),
                        Price = 1240000000,
                        Region = "us"
                    };

                    var beforeLastToken = new Data.Models.Database.WoWToken()
                    {
                        LastUpdatedTimestamp = new DateTimeOffset(DateTime.UtcNow.AddMinutes(-5)).ToUnixTimeSeconds(),
                        Price = 1230000000,
                        Region = "us"
                    };

                    await context.Database.EnsureCreatedAsync();

                    await context.Tokens.AddAsync(latestToken);
                    await context.Tokens.AddAsync(beforeLastToken);

                    await context.SaveChangesAsync();

                    var tokenService = new TokenService(context);
                    var latestTokenFromDatabase = await tokenService.GetLatestTokenInformationAsync("us");

                    Assert.AreEqual(latestToken.Price, latestTokenFromDatabase.Price);
                    Assert.AreEqual("us", latestTokenFromDatabase.Region);
                    Assert.AreEqual(latestToken.Price - beforeLastToken.Price, latestTokenFromDatabase.PriceDifference);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        /// <summary>
        /// Test the service to get the latest token information with more than two 
        ///   tokens in the database for the same region.
        /// </summary
        [TestMethod]
        public async Task GetLatestTokenInformationOneRegionMoreThanTwoTokens()
        {
            var connection = new SqliteConnection("DataSource=:memory");

            try
            {
                await connection.OpenAsync();

                var options = new DbContextOptionsBuilder<WoWTokenContext>()
                    .UseSqlite(connection)
                    .Options;

                using (var context = new WoWTokenContext(options))
                {
                    context.Tokens.RemoveRange(await context.Tokens.ToListAsync());
                    context.SaveChanges();

                    var latestToken = new Data.Models.Database.WoWToken()
                    {
                        LastUpdatedTimestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds(),
                        Price = 1240000000,
                        Region = "us"
                    };
                    var beforeLastToken = new Data.Models.Database.WoWToken()
                    {
                        LastUpdatedTimestamp = new DateTimeOffset(DateTime.UtcNow.AddMinutes(-5)).ToUnixTimeSeconds(),
                        Price = 1230000000,
                        Region = "us"
                    };

                    await context.Database.EnsureCreatedAsync();

                    await context.Tokens.AddAsync(latestToken);
                    await context.Tokens.AddAsync(beforeLastToken);
                    await context.Tokens.AddAsync(new Data.Models.Database.WoWToken()
                    {
                        LastUpdatedTimestamp = new DateTimeOffset(DateTime.UtcNow.AddMinutes(-10)).ToUnixTimeSeconds(),
                        Price = 1235000000,
                        Region = "us"
                    });

                    await context.SaveChangesAsync();

                    var tokenService = new TokenService(context);
                    var latestTokenFromDatabase = await tokenService.GetLatestTokenInformationAsync("us");

                    Assert.AreEqual(latestToken.Price, latestTokenFromDatabase.Price);
                    Assert.AreEqual("us", latestTokenFromDatabase.Region);
                    Assert.AreEqual(latestToken.Price - beforeLastToken.Price, latestTokenFromDatabase.PriceDifference);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        /// <summary>
        /// Test the service to get the latest token information with only
        ///   one token for the specified region.
        /// </summary
        [TestMethod]
        public async Task GetLatestTokenInformationOneToken()
        {
            var connection = new SqliteConnection("DataSource=:memory");

            try
            {
                await connection.OpenAsync();

                var options = new DbContextOptionsBuilder<WoWTokenContext>()
                    .UseSqlite(connection)
                    .Options;

                using (var context = new WoWTokenContext(options))
                {
                    context.Tokens.RemoveRange(await context.Tokens.ToListAsync());
                    context.SaveChanges();

                    var latestToken = new Data.Models.Database.WoWToken()
                    {
                        LastUpdatedTimestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds(),
                        Price = 1240000000,
                        Region = "us"
                    };

                    await context.Database.EnsureCreatedAsync();

                    await context.Tokens.AddAsync(latestToken);

                    await context.SaveChangesAsync();

                    var tokenService = new TokenService(context);
                    var latestTokenFromDatabase = await tokenService.GetLatestTokenInformationAsync("us");

                    Assert.AreEqual(latestToken.Price, latestTokenFromDatabase.Price);
                    Assert.AreEqual("us", latestTokenFromDatabase.Region);
                    Assert.AreEqual(0, latestTokenFromDatabase.PriceDifference);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        /// <summary>
        /// Test the service to get the latest token information.
        /// </summary
        [TestMethod]
        public async Task GetLatestTokenInformation()
        {
            var connection = new SqliteConnection("DataSource=:memory");

            try
            {
                await connection.OpenAsync();

                var options = new DbContextOptionsBuilder<WoWTokenContext>()
                    .UseSqlite(connection)
                    .Options;

                using (var context = new WoWTokenContext(options))
                {
                    context.Tokens.RemoveRange(await context.Tokens.ToListAsync());
                    context.SaveChanges();

                    var latestToken = new Data.Models.Database.WoWToken()
                    {
                        LastUpdatedTimestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds(),
                        Price = 1240000000,
                        Region = "us"
                    };

                    var beforeLastToken = new Data.Models.Database.WoWToken()
                    {
                        LastUpdatedTimestamp = new DateTimeOffset(DateTime.UtcNow.AddMinutes(-5)).ToUnixTimeSeconds(),
                        Price = 1230000000,
                        Region = "us"
                    };

                    await context.Database.EnsureCreatedAsync();

                    await context.Tokens.AddAsync(latestToken);
                    await context.Tokens.AddAsync(beforeLastToken);
                    await context.Tokens.AddAsync(new Data.Models.Database.WoWToken()
                    {
                        LastUpdatedTimestamp = new DateTimeOffset(DateTime.UtcNow.AddMinutes(-10)).ToUnixTimeSeconds(),
                        Price = 2235000000,
                        Region = "eu"
                    });
                    await context.Tokens.AddAsync(new Data.Models.Database.WoWToken()
                    {
                        LastUpdatedTimestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds(),
                        Price = 1235000000,
                        Region = "eu"
                    });
                    await context.Tokens.AddAsync(new Data.Models.Database.WoWToken()
                    {
                        LastUpdatedTimestamp = new DateTimeOffset(DateTime.UtcNow.AddMinutes(-5)).ToUnixTimeSeconds(),
                        Price = 1255000000,
                        Region = "eu"
                    });
                    await context.Tokens.AddAsync(new Data.Models.Database.WoWToken()
                    {
                        LastUpdatedTimestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds(),
                        Price = 2255000000,
                        Region = "kr"
                    });

                    await context.SaveChangesAsync();

                    var tokenService = new TokenService(context);
                    var latestTokenFromDatabase = await tokenService.GetLatestTokenInformationAsync("us");

                    Assert.AreEqual(latestToken.Price, latestTokenFromDatabase.Price);
                    Assert.AreEqual("us", latestTokenFromDatabase.Region);
                    Assert.AreEqual(latestToken.Price - beforeLastToken.Price, latestTokenFromDatabase.PriceDifference);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        /// <summary>
        /// Test the service to get the latest token information with a
        ///   region that does not exist.
        /// </summary
        [TestMethod]
        public async Task GetLatestTokenInformationNoRegion()
        {
            var connection = new SqliteConnection("DataSource=:memory");

            try
            {
                await connection.OpenAsync();

                var options = new DbContextOptionsBuilder<WoWTokenContext>()
                    .UseSqlite(connection)
                    .Options;

                using (var context = new WoWTokenContext(options))
                {
                    context.Tokens.RemoveRange(await context.Tokens.ToListAsync());
                    context.SaveChanges();

                    var latestToken = new Data.Models.Database.WoWToken()
                    {
                        LastUpdatedTimestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds(),
                        Price = 1240000000,
                        Region = "us"
                    };

                    await context.Database.EnsureCreatedAsync();

                    await context.Tokens.AddAsync(latestToken);

                    await context.SaveChangesAsync();

                    var tokenService = new TokenService(context);
                    var latestTokenFromDatabase = await tokenService.GetLatestTokenInformationAsync("ls");

                    Assert.AreEqual(null, latestTokenFromDatabase);
                }
            }
            finally
            {
                connection.Close();
            }
        }
    }
}