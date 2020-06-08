using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using WoWToken.Server.Data.Core;
using WoWToken.Server.Data.Services;

namespace WoWToken.Server.Api.Extensions
{
    /// <summary>
    /// The scoped extension.
    /// </summary>
    public static class ScopedExtension
    {
        /// <summary>
        /// Add the scopeds to the application
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configuration">The configuration.</param>
        public static void AddScopeds(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ITokenSyncService, TokenSyncService>();
            services.AddScoped<ITokenService, TokenService>();
        }
    }
}
