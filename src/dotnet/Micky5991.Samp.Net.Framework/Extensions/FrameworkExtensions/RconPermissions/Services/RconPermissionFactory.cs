using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Dawn;
using Micky5991.Samp.Net.Framework.Interfaces.Permissions;
using Micky5991.Samp.Net.Framework.Interfaces.Permissions.Calculators;
using Micky5991.Samp.Net.Framework.Interfaces.Permissions.Factories;
using Microsoft.Extensions.Logging;

namespace Micky5991.Samp.Net.Framework.Extensions.FrameworkExtensions.RconPermissions.Services
{
    /// <summary>
    /// Factory that creates <see cref="RconPermissionContainer"/> instances and calculates context.
    /// </summary>
    public class RconPermissionFactory : IPermissionFactory
    {
        private readonly ILogger<RconPermissionContainer> containerLogger;

        private readonly IList<IPermissibleContextCalculator> contextCalculators;

        /// <summary>
        /// Initializes a new instance of the <see cref="RconPermissionFactory"/> class.
        /// </summary>
        /// <param name="contextCalculators">Available calculators in this container.</param>
        /// <param name="containerLogger">Logger that will be used for <see cref="RconPermissionContainer"/>.</param>
        public RconPermissionFactory(IEnumerable<IPermissibleContextCalculator> contextCalculators, ILogger<RconPermissionContainer> containerLogger)
        {
            this.containerLogger = containerLogger;
            this.contextCalculators = contextCalculators.ToList();
        }

        /// <inheritdoc />
        public IPermissionContainer BuildContainer(IPermissible? permissible)
        {
            Guard.Argument(permissible!, nameof(permissible)).NotNull();

            return new RconPermissionContainer(permissible!, this.containerLogger);
        }

        /// <inheritdoc />
        public IImmutableDictionary<string, string> CalculateContext(IPermissible permissible)
        {
            IImmutableDictionary<string, string> context = new Dictionary<string, string>().ToImmutableDictionary();

            foreach (var contextCalculator in this.contextCalculators)
            {
                context = contextCalculator.Calculate(permissible, context);
            }

            return context;
        }
    }
}
