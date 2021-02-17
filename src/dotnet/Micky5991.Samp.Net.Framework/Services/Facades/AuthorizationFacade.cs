using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Dawn;
using Micky5991.Samp.Net.Framework.Interfaces.Facades;
using Microsoft.AspNetCore.Authorization;

namespace Micky5991.Samp.Net.Framework.Services.Facades
{
    /// <inheritdoc />
    public class AuthorizationFacade : IAuthorizationFacade
    {
        private readonly IAuthorizationPolicyProvider policyProvider;

        private readonly IAuthorizationService authorizationService;

        public AuthorizationFacade(
            IAuthorizationPolicyProvider policyProvider,
            IAuthorizationService authorizationService)
        {
            this.policyProvider = policyProvider;
            this.authorizationService = authorizationService;
        }

        /// <inheritdoc />
        public async Task<AuthorizationResult> AuthorizeAsync(ClaimsPrincipal principal, object? resource, AuthorizeAttribute[] attributes)
        {
            Guard.Argument(principal, nameof(principal)).NotNull();
            Guard.Argument(attributes, nameof(attributes)).NotNull().DoesNotContainNull();

            AuthorizationPolicy? policy = null;

            if (attributes.Length > 0)
            {
                try
                {
                    policy = await AuthorizationPolicy.CombineAsync(this.policyProvider, attributes)
                                                      .ConfigureAwait(false);
                }
                catch (InvalidOperationException)
                {
                    policy = await this.policyProvider.GetFallbackPolicyAsync();

                    if (policy == null)
                    {
                        throw;
                    }
                }
            }

            if (policy == null)
            {
                return AuthorizationResult.Success();
            }

            return await this.authorizationService.AuthorizeAsync(principal, resource, policy).ConfigureAwait(false);
        }
    }
}
