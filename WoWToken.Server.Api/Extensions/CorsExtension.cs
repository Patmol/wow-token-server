using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WoWToken.Server.Api.Extensions
{
    /// <summary>
    /// The cors extension.
    /// </summary>
    public static class CorsExtension
    {
         /// <summary>
        /// Add Cors to allow every origin.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configuration">The configuration.</param>
        public static void AddCorsAllow(this IServiceCollection services, IConfiguration configuration)
        {
            var clientUrls = configuration.GetSection("AppSettings").GetSection("ClientUrls")
                .AsEnumerable()
                .Select(i => i.Value)
                .Where(i => !string.IsNullOrWhiteSpace(i))
                .ToArray();

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder
                            .WithOrigins(clientUrls)
                            .AllowCredentials()
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                    });
            });
        }
    }
}
