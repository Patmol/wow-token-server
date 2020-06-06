using System;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Hangfire;

using WoWToken.Server.Api.Extensions;
using WoWToken.Server.Data.Models;
using WoWToken.Server.Api.Authorization;

namespace WoWToken.Server.Api
{
    public class Startup
    {
        private const string SYNC_WOW_TOKEN_DATA = "Synchonize WoW token information";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHangfireService(this.Configuration);

            services.AddDbContext<WoWTokenContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("WoWTokenConnection"))
            );

            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            services.AddScopeds(this.Configuration);
            services.AddOptions();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IBackgroundJobClient backgroundJobs,
            IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHangfireDashboard("/hangfire", new DashboardOptions() {
                Authorization = new[] { new HangfireAuthorizationFilter() }
            });

            RecurringJob.AddOrUpdate<Data.ITokenService>(
                SYNC_WOW_TOKEN_DATA,
                service => service.SyncTokenInformationAsync(),
                env.IsDevelopment() ? "0 0 * * *" : "* * * * *");

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
