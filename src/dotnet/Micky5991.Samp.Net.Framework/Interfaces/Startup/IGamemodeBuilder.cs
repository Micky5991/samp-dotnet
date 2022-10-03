// <copyright file="IGamemodeBuilder.cs" company="Micky5991">
// Copyright (c) Micky5991. All rights reserved.
// </copyright>

using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Micky5991.Samp.Net.Framework.Interfaces.Startup
{
    public interface IGamemodeBuilder
    {
        void RegisterServices(IServiceCollection serviceCollection);

        void ConfigureAuthorization(AuthorizationOptions options);

        void Start(IServiceProvider serviceProvider);
    }
}
