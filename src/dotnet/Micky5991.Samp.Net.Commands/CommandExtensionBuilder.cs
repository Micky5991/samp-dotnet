using System;
using Micky5991.Samp.Net.Framework.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Micky5991.Samp.Net.Commands
{
    /// <summary>
    /// Registers all services for the Commands extension.
    /// </summary>
    public class CommandExtensionBuilder : IExtensionBuilder
    {
        /// <inheritdoc/>
        public void Register(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IExtensionStarter, CommandExtensionStarter>();
        }
    }
}
