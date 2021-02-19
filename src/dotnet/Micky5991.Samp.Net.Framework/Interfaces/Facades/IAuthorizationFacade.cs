using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Micky5991.Samp.Net.Framework.Interfaces.Facades
{
    /// <summary>
    /// Facade that simplifies combination of authorize attribute handling.
    /// </summary>
    public interface IAuthorizationFacade
    {
        /// <summary>
        /// Checks if the specified <paramref name="principal"/> has needed access.
        /// </summary>
        /// <param name="principal">User that needs to be checked if authorized.</param>
        /// <param name="resource">Resource that needs access to.</param>
        /// <param name="attributes">Attributes defined on this attribute.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<AuthorizationResult> AuthorizeAsync(ClaimsPrincipal principal, object? resource, AuthorizeAttribute[] attributes);

        /// <summary>
        /// Enables falling back to default policy if the provided policy was not found.
        /// </summary>
        void UseDefaultPolicyForUnknownPolicies();
    }
}
