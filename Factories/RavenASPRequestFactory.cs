﻿using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.Framework.DependencyInjection;
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
            var requestContext = Services.GetRequiredService<IContextAccessor<HttpContext>>();

            if (requestContext.Value == null) return null;
            var context = requestContext.Value;
            var routeFeature = context.GetFeature<IRouteInformationFeature>();

            request.Cookies = context.Request.Cookies.ToDictionary(c => c.Key, c => c.Value.Aggregate((x, y) => x + ", " + y));
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