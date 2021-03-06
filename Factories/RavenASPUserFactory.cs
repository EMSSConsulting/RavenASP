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
    public class RavenASPUserFactory : SentryUserFactory
    {
        protected readonly IServiceProvider Services;
        public RavenASPUserFactory(IServiceProvider serviceProvider)
        {
            Services = serviceProvider;
        }

        protected override SentryUser OnCreate(SentryUser user)
        {
            var requestContext = Services.GetRequiredService<IHttpContextAccessor>();

            if (requestContext.HttpContext == null) return null;
            var context = requestContext.HttpContext;

            user.Username = context.User.Identity.Name;

            return user;
        }
    }
}