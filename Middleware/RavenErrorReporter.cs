using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using Auditor.Middleware;
using SharpRaven.Features;
using SharpRaven.Data;

namespace AntennaPortal.Middleware
{
    public class RavenErrorReporter : ErrorReporterMiddleware
    {
        public RavenErrorReporter(RequestDelegate next, bool rethrow) : base(next, rethrow)
        {

        }

        protected override async Task ReportException(HttpContext context, Exception ex)
        {
            var errorReporter = context.Features.Get<IRavenReportingFeature>();
            await (errorReporter?.Raven?.CaptureAsync(new SentryEvent(ex)) ?? Task.FromResult(""));
        }
    }
}