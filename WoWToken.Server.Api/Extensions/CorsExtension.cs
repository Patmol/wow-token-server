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
        public static void AddCorsAllow(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder
                            .WithOrigins("http://localhost:8080")
                            .AllowCredentials()
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                    });
            });
        }
    }
}
