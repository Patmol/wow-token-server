using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Hangfire;

using WoWToken.Server.Api.Extensions;
using WoWToken.Server.Data.Models;
using AutoMapper;

namespace WoWToken.Server.Api
{
    /// <summary>
    /// The startup class.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initialize a new instance of a <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">The service collection.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<WoWTokenContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("WoWTokenConnection"))
            );

            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.AddHangfireService(this.Configuration);
            services.AddScopeds(this.Configuration);
            services.AddCorsAllow();
            services.AddOptions();
            services.AddSwaggerGen(this.Configuration);
            services.AddAutoMapper(typeof(Startup).Assembly);
            services.AddControllers();
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <param name="backgroundJobs">The background job client.</param>
        /// <param name="env">The web host environment.</param>
        public void Configure(
            IApplicationBuilder app,
            IBackgroundJobClient backgroundJobs,
            IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors();
            app.AddSwagger(this.Configuration);
            app.AddHangfireJobs(env);
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
