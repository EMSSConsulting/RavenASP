using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using SharpRaven.Data;
using System;
using System.Threading.Tasks;
using Auditor.Features;
using Auditor.Middleware;
using SharpRaven.Features;

namespace AntennaPortal.Middleware
{   
    public class RavenRouteLogger : RouteLoggingMiddleware
    {

        public RavenRouteLogger(RequestDelegate next) : base(next)
        {

        }

        protected override async Task LogRoute(HttpContext context, IRouteInformationFeature routeInformation)
        {
            var reporter = context.GetFeature<IRavenReportingFeature>();
            if (reporter == null) return;
            
            var level = ErrorLevel.Info;
            Exception exception = null;
            switch (context.Response.StatusCode)
            {
                case 400:
                    level = ErrorLevel.Warning;
                    exception = new Exception("Bad Request");
                    break;
                case 404:
                    level = ErrorLevel.Warning;
                    exception = new Exception("Not Found");
                    break;
                case 401:
                    level = ErrorLevel.Warning;
                    exception = new Exception("Unauthorized");
                    break;
                case 403:
                    level = ErrorLevel.Warning;
                    exception = new Exception("Forbidden");
                    break;
            }


            if (exception != null) await (reporter.Raven?.CaptureExceptionAsync(exception, routeInformation.RouteName + " - " + exception.Message, level) ?? Task.FromResult(""));
            else await (reporter.Raven?.CaptureMessageAsync(routeInformation.RouteName, level) ?? Task.FromResult(""));
        }

        protected override async Task LogNotFound(HttpContext context, IRouteInformationFeature routeInformation, IRouteNotFoundFeature notFound)
        {
            var reporter = context.GetFeature<IRavenReportingFeature>();
            if (reporter == null) return;

            var level = ErrorLevel.Warning;
            var exception = new Exception("Not Found");

            await (reporter.Raven?.CaptureExceptionAsync(exception, routeInformation.RouteName + " - " + exception.Message, level) ?? Task.FromResult(""));
        }
    }
}