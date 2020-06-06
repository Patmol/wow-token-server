using System;
using Hangfire.Annotations;
using Hangfire.Dashboard;

namespace WoWToken.Server.Api.Authorization
{
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize([NotNull] DashboardContext context)
        {
            return true;
        }
    }
}
