using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using SharpRaven.Features;

namespace SharpRaven.Middleware
{
    public class RavenReportingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly bool _enabled;

        public RavenReportingMiddleware(RequestDelegate next, bool enabledByDefault)
        {
            _next = next;
            _enabled = enabledByDefault;
        }

        public async Task Invoke(HttpContext context)
        {
            // Set the default error reporting feature
            context.Features.Set<IRavenReportingFeature>(new RavenReportingFeature() { Enabled = _enabled, Services = context.RequestServices });
            await _next(context);
        }
    }
}