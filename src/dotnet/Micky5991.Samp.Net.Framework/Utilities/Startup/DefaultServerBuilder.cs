// <copyright file="DefaultServerBuilder.cs" company="Micky5991">
// Copyright (c) Micky5991. All rights reserved.
// </copyright>

using System;
using Micky5991.Samp.Net.Framework.Interfaces.Startup;
using Microsoft.Extensions.DependencyInjection;

namespace Micky5991.Samp.Net.Framework.Utilities.Startup
{
    /// <inheritdoc cref="IServerBuilder" />
    public class DefaultServerBuilder : DefaultGamemodeBuilder, IServerBuilder
    {
        /// <inheritdoc />
        public virtual IServiceCollection CreateServiceCollection()
        {
            return new ServiceCollection();
        }

        /// <inheritdoc />
        public virtual IServiceProvider BuildServiceProvider(IServiceCollection serviceCollection)
        {
            return serviceCollection.BuildServiceProvider();
        }
    }
}
