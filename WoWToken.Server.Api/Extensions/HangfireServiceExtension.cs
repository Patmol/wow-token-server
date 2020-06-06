using System;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Hangfire;
using Hangfire.SqlServer;

namespace WoWToken.Server.Api.Extensions
{
    /// <summary>
    /// The Hangfire services configuration extension.
    /// </summary>
    public static class HangfireServiceExtension
    {
        /// <summary>
        /// Add the Hangfire services to the application.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="config">The configuration.</param>
        public static void AddHangfireService(this IServiceCollection services, IConfiguration config)
        {
            // Add Hangfire services.
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(config.GetConnectionString("HangfireConnection"), new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    UsePageLocksOnDequeue = true,
                    DisableGlobalLocks = true
                }));

            // Add the processing server as IHostedService
            services.AddHangfireServer();
        }
    }
}
