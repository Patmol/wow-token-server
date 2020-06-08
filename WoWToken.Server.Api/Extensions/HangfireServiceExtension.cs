using System;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace WoWToken.Server.Api.Extensions
{
    /// <summary>
    /// The Hangfire services configuration extension.
    /// </summary>
    public static class HangfireServiceExtension
    {
        /// <summary>
        /// The name of the Hangfire job for the synchronisation of the WoW Token data.
        /// </summary>
        private const string SYNC_WOW_TOKEN_DATA = "Synchonize WoW token information";

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

        /// <summary>
        /// Add the Hangfire jobs to the application.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <param name="env">The host environment.</param>
        public static void AddHangfireJobs(this IApplicationBuilder app, IHostEnvironment env)
        {
            app.UseHangfireDashboard();

            RecurringJob.AddOrUpdate<Data.Core.ITokenSyncService>(
                SYNC_WOW_TOKEN_DATA,
                service => service.SyncTokenInformationAsync(),
                env.IsDevelopment() ? "*/15 * * * *" : "* * * * *");
        }
    }
}
