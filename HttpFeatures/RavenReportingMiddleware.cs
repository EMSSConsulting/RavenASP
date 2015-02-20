using Microsoft.Framework.DependencyInjection;
using System;

namespace SharpRaven.Features
{
    public interface IRavenReportingFeature
    {
        bool Enabled { get; set; }

        IRavenClient Raven { get; }
    }

    public class RavenReportingFeature : IRavenReportingFeature
    {
        public bool Enabled { get; set; }

        public IServiceProvider Services { get; set; }

        public IRavenClient Raven
        {
            get
            {
                return Services.GetRequiredService<IRavenClient>();
            }
        }
    }
}