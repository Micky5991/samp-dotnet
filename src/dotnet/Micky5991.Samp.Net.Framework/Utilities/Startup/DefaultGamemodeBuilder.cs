// <copyright file="DefaultGamemodeBuilder.cs" company="Micky5991">
// Copyright (c) Micky5991. All rights reserved.
// </copyright>

using Micky5991.Samp.Net.Framework.Interfaces.Startup;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Micky5991.Samp.Net.Framework.Utilities.Startup
{
    /// <inheritdoc />
    public class DefaultGamemodeBuilder : IGamemodeBuilder
    {
        /// <inheritdoc />
        public virtual void RegisterServices(IServiceCollection serviceCollection)
        {
        }

        /// <inheritdoc />
        public virtual void ConfigureAuthorization(AuthorizationOptions options)
        {
            // empty
        }
    }
}
