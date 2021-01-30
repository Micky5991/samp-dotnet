using System;
using System.Collections.Immutable;

namespace Micky5991.Samp.Net.Framework.Interfaces.Permissions.Factories
{
    /// <summary>
    /// Factory that creates different types of data.
    /// </summary>
    public interface IPermissionFactory
    {
        /// <summary>
        /// Builds a new instance of <see cref="IPermissionContainer"/>.
        /// </summary>
        /// <param name="permissible">Optional permissible to integrate into cointainer.</param>
        /// <returns>Newly created container.</returns>
        IPermissionContainer BuildContainer(IPermissible? permissible);

        /// <summary>
        /// Calculates the permission context for the given <paramref name="permissible"/>.
        /// </summary>
        /// <param name="permissible">Entity to base the context on.</param>
        /// <returns>Calculated dictionary with the calculated context.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="permissible"/> is null.</exception>
        IImmutableDictionary<string, string> CalculateContext(IPermissible permissible);
    }
}
