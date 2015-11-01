// ==========================================================================
// AppBuilderExtensions.cs
// Green Parrot Framework
// ==========================================================================
// Copyright (c) Sebastian Stehle
// All rights reserved.
// ========================================================================== 

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.DataProtection;
using Microsoft.AspNet.Hosting;
using Microsoft.Owin.Builder;
using Microsoft.Owin.BuilderProperties;
using Owin;

namespace SE.Upload.Web.Pipeline
{
    public static class AppBuilderExtensions
    {
        public static IApplicationBuilder UseOwinAppBuilder(this IApplicationBuilder app, Action<IAppBuilder> configuration)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            return app.UseOwin(setup => setup(next =>
            {
                AppBuilder builder = new AppBuilder();
                
                new AppProperties(builder.Properties)
                    .UseNext(next)
                    .UseName(app)
                    .HandleDispose(app);

                configuration(builder);

                return builder.Build<Func<IDictionary<string, object>, Task>>();
            }));
        }

        private static AppProperties UseNext(this AppProperties properties, Func<IDictionary<string, object>, Task> next)
        {
            properties.DefaultApp = next;

            return properties;
        }

        private static AppProperties UseName(this AppProperties properties, IApplicationBuilder app)
        {
            properties.AppName = app.ApplicationServices.GetApplicationUniqueIdentifier();

            return properties;
        }

        private static void HandleDispose(this AppProperties properties, IApplicationBuilder app)
        {
            IApplicationLifetime lifetime = app.ApplicationServices.GetService(typeof(IApplicationLifetime)) as IApplicationLifetime;

            if (lifetime != null)
            {
                properties.OnAppDisposing = lifetime.ApplicationStopping;
            }
        }
    }
}
