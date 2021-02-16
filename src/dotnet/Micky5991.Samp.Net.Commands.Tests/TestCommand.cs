using System.Collections.Generic;
using System.Threading.Tasks;
using Micky5991.Samp.Net.Commands.Attributes;
using Micky5991.Samp.Net.Commands.Data.Results;
using Micky5991.Samp.Net.Commands.Elements;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;
using Micky5991.Samp.Net.Framework.Interfaces.Facades;
using Microsoft.AspNetCore.Authorization;

namespace Micky5991.Samp.Net.Commands.Tests
{
    public class TestCommand : Command
    {
        public TestCommand(
            IAuthorizationFacade authorization,
            CommandAttribute commandAttribute,
            AuthorizeAttribute[] authorizeAttributes,
            string[] aliasNames,
            IReadOnlyList<ParameterDefinition> parameters)
            : base(authorization, commandAttribute, authorizeAttributes, aliasNames, parameters)
        {
        }

        public override Task<CommandResult> TryExecuteAsync(IPlayer player, object[] arguments, bool skipPermissions)
        {
            throw new System.NotImplementedException();
        }
    }
}
