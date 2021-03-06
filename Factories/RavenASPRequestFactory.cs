﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using SharpRaven;
using SharpRaven.Data;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using SharpRaven.Logging;
using System.Text;
using Auditor.Features;
using SharpRaven.Features;
using Microsoft.AspNetCore.Hosting;

namespace SharpRaven.Factories
{
    public class RavenASPRequestFactory : SentryRequestFactory
    {
        private readonly IServiceProvider Services;
        public RavenASPRequestFactory(IServiceProvider serviceProvider)
        {
            Services = serviceProvider;
        }

        public override SentryRequest OnCreate(SentryRequest request)
        {
            if (request == null) request = new SentryRequest();
            var requestContext = Services.GetRequiredService<IHttpContextAccessor>();

            if (requestContext.HttpContext == null) return null;
            var context = requestContext.HttpContext;
            var routeFeature = context.Features.Get<IRouteInformationFeature>();

            request.Cookies = context.Request.Cookies.ToDictionary(c => c.Key, c => c.Value);
            request.Headers = context.Request.Headers.ToDictionary(h => h.Key, h => h.Value.Aggregate((x, y) => x + ", " + y));
            request.Method = context.Request.Method;
            request.QueryString = context.Request.QueryString.ToString();
            request.Url = context.Request.PathBase.Add(context.Request.Path).ToString();
            if(routeFeature != null)
                request.Data = routeFeature.Arguments;

            return request;
        }
    }
}