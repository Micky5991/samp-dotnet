using System.Collections.Immutable;

namespace Micky5991.Samp.Net.Framework.Interfaces.Permissions
{
    /// <summary>
    /// Describes an object that can hold permissions.
    /// </summary>
    public interface IPermissible
    {
        /// <summary>
        /// Checks if the current <see cref="IPermissible"/> has the permission to do something. If the permission was not set, <paramref name="defaultValue"/> will be returned.
        /// </summary>
        /// <param name="permission">Permission to check if the <see cref="IPermissible"/> is granted to do.</param>
        /// <param name="defaultValue">If the given <paramref name="permission"/> is not set, this value will be returned.</param>
        /// <returns>true if the <see cref="IPermissible"/> is granted to do something.</returns>
        bool HasPermission(string permission, bool defaultValue = false);

        /// <summary>
        /// Checks if the given <paramref name="permission"/> is set at oll on the current <see cref="IPermissible"/>.
        /// </summary>
        /// <param name="permission">Permission to check.</param>
        /// <returns>true if the permission is defined, false otherwise.</returns>
        bool IsPermissionSet(string permission);
    }
}
