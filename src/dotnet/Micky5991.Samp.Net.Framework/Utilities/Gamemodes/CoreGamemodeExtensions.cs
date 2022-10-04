// <copyright file="CoreGamemodeExtensions.cs" company="Micky5991">
// Copyright (c) Micky5991. All rights reserved.
// </copyright>

using System;
using System.Threading;
using Micky5991.Samp.Net.Core.Threading;
using Micky5991.Samp.Net.Framework.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Micky5991.Samp.Net.Framework.Utilities.Gamemodes
{
    public static class CoreGamemodeExtensions
    {
        public static IHostBuilder AddCoreGamemodeServices(this IHostBuilder hostBuilder, Action<SampNetOptions>? options)
        {
            return hostBuilder.AddCoreGamemodeServices(options, new CoreHostBuilder());
        }

        public static IHostBuilder AddCoreGamemodeServices(this IHostBuilder hostBuilder, Action<SampNetOptions>? options, CoreHostBuilder builder)
        {
            var synchronizationContext = new SampSynchronizationContext();
            synchronizationContext.Setup();

            builder.ConfigureHost(hostBuilder);

            hostBuilder.ConfigureServices(
                                          (_, services) =>
                                          {
                                              services.AddSingleton(synchronizationContext);
                                              services.Configure(options);
                                          });

            return hostBuilder;
        }

        public static IHostBuilder SetFallbackAuthorizationPolicy(this IHostBuilder hostBuilder, bool accept)
        {
            void ConfigureAuthorizationCore(AuthorizationOptions options)
            {
                var acceptAllPolicy = new AuthorizationPolicyBuilder()
                        .RequireAssertion(x => accept)
                        .Build();

                options.FallbackPolicy = acceptAllPolicy;
                options.DefaultPolicy = acceptAllPolicy;
            }

            void ConfigureSampNet(SampNetOptions options)
            {
                options.UseDefaultPolicyForUnknownPolicy = true;
            }

            hostBuilder.ConfigureServices(
                                          (_, services) =>
                                          {
                                              services.Configure<SampNetOptions>(ConfigureSampNet);
                                              services.AddAuthorizationCore(ConfigureAuthorizationCore);
                                          });

            return hostBuilder;
        }
    }
}
