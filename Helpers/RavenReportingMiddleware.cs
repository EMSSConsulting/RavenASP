using Microsoft.AspNetCore.Builder;
using System;
using SharpRaven.Middleware;

namespace SharpRaven
{
    public static class RavenReportingExtensions
    {
        public static IApplicationBuilder UseRavenReporting(this IApplicationBuilder builder, bool enabledByDefault = true)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            return builder.Use(next => new RavenReportingMiddleware(next, enabledByDefault).Invoke);
        }
    }
}