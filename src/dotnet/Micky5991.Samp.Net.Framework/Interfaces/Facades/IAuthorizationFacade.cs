using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Micky5991.Samp.Net.Framework.Interfaces.Facades
{
    public interface IAuthorizationFacade
    {
        Task<AuthorizationResult> AuthorizeAsync(ClaimsPrincipal principal, object? resource, AuthorizeAttribute[] attributes);
    }
}
