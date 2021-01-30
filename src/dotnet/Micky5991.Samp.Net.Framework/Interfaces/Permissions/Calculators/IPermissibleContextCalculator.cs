using System.Collections.Immutable;

namespace Micky5991.Samp.Net.Framework.Interfaces.Permissions.Calculators
{
    /// <summary>
    /// Calculates the context for an entity.
    /// </summary>
    public interface IPermissibleContextCalculator
    {
        /// <summary>
        /// Calculates the context for the specified <paramref name="permissible"/>. The <paramref name="context"/>
        /// itself cannot be mutated, but the modified <paramref name="context"/> has to be returned.
        /// </summary>
        /// <param name="permissible">Permissible where the context should be extracted from.</param>
        /// <param name="context">Input context that can be altered and has to be returned.</param>
        /// <returns>Resulting and modified permission context.</returns>
        IImmutableDictionary<string, string> Calculate(IPermissible permissible, IImmutableDictionary<string, string> context);
    }
}
