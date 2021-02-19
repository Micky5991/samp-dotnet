using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Dawn;
using Micky5991.Samp.Net.Framework.Interfaces.Facades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace Micky5991.Samp.Net.Framework.Services.Facades
{
    /// <inheritdoc />
    public class AuthorizationFacade : IAuthorizationFacade
    {
        private readonly IAuthorizationPolicyProvider policyProvider;

        private readonly IAuthorizationService authorizationService;

        private readonly ILogger<AuthorizationFacade> logger;

        private bool useDefaultPolicyForUnknownPolicies = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationFacade"/> class.
        /// </summary>
        /// <param name="policyProvider">Policy provider that holds all needed policies.</param>
        /// <param name="authorizationService">Service that executes checks if a <see cref="ClaimsPrincipal"/> has needed permission.</param>
        /// <param name="logger">Logger used in this type.</param>
        public AuthorizationFacade(
            IAuthorizationPolicyProvider policyProvider,
            IAuthorizationService authorizationService,
            ILogger<AuthorizationFacade> logger)
        {
            this.policyProvider = policyProvider;
            this.authorizationService = authorizationService;
            this.logger = logger;
        }

        /// <inheritdoc />
        public async Task<AuthorizationResult> AuthorizeAsync(ClaimsPrincipal principal, object? resource, AuthorizeAttribute[] attributes)
        {
            Guard.Argument(principal, nameof(principal)).NotNull();
            Guard.Argument(attributes, nameof(attributes)).NotNull().DoesNotContainNull();

            AuthorizationPolicy? policy;

            try
            {
                policy = await AuthorizationPolicy.CombineAsync(this.policyProvider, attributes)
                                                  .ConfigureAwait(false);
            }
            catch (InvalidOperationException)
            {
                if (this.useDefaultPolicyForUnknownPolicies == false)
                {
                    throw;
                }

                policy = await this.policyProvider.GetDefaultPolicyAsync().ConfigureAwait(false);
                if (policy == null)
                {
                    throw;
                }
            }

            if (policy == null)
            {
                this.logger.LogWarning($"No fallback {nameof(AuthorizationPolicy)} has been defined, declining authorize request.");

                return AuthorizationResult.Failed();
            }

            return await this.authorizationService.AuthorizeAsync(principal, resource, policy).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public void UseDefaultPolicyForUnknownPolicies()
        {
            this.useDefaultPolicyForUnknownPolicies = true;

            this.logger.LogInformation("Using default policy for unknown policies has been enabled.");
        }
    }
}
