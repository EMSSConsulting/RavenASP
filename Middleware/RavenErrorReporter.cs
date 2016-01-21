﻿using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using System;
using System.Threading.Tasks;
using Auditor.Middleware;
using SharpRaven.Features;
using Microsoft.AspNet.Http.Features;

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
            await (errorReporter?.Raven?.CaptureExceptionAsync(ex) ?? Task.FromResult(""));
        }
    }
}