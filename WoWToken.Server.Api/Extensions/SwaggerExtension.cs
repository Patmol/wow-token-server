using System;
using System.IO;
using System.Reflection;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WoWToken.Server.Api.Extensions
{
    /// <summary>
    /// The Swagger extension.
    /// </summary>
    public static class SwaggerExtension
    {
        /// <summary>
        /// Add Swagger to the application.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <param name="configuration">The configuration.</param>
        public static void AddSwagger(this IApplicationBuilder app, IConfiguration configuration)
        {
            var swaggerUrl = configuration.GetSection("AppSettings").GetValue<string>("SwaggerUrl");

            if (swaggerUrl != null)
            {
                var swaggerName = configuration.GetSection("AppSettings").GetValue<string>("SwaggerName");
                var swaggerVersion = configuration.GetSection("AppSettings").GetValue<string>("SwaggerVersion");

                // Enable middleware to serve generated Swagger as a JSON endpoint.
                app.UseSwagger();

                // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
                // specifying the Swagger JSON endpoint.
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint(swaggerUrl, $"{swaggerName} {swaggerVersion}");
                });
            }
        }

        /// <summary>
        /// Add Swagger generation to the service.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configuration">The configuration.</param>
        public static void AddSwaggerGen(this IServiceCollection services, IConfiguration configuration)
        {
            // Register the Swagger generator, defining 1 or more Swagger documents
            var swaggerName = configuration.GetSection("AppSettings").GetValue<string>("SwaggerName");
            var swaggerVersion = configuration.GetSection("AppSettings").GetValue<string>("SwaggerVersion");

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(swaggerVersion, new Microsoft.OpenApi.Models.OpenApiInfo { Title = swaggerName, Version = swaggerVersion });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }
    }
}
