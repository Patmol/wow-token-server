using System;
using Hangfire.Annotations;
using Hangfire.Dashboard;

namespace WoWToken.Server.Api.Authorization
{
    /// <summary>
    /// The Hangfire Authorization filter.
    /// </summary>
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        /// <summary>
        /// Chekc if the user can access to the Hangfire dashboard or not.
        /// </summary>
        /// <param name="context">The dashboard context.</param>
        /// <returns>True if the user can access to the Hangfire dashboard; false otherwise.</returns>
        public bool Authorize([NotNull] DashboardContext context)
        {
            return true;
        }
    }
}
