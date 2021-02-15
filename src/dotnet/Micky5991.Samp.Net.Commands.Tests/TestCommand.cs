using System.Collections.Generic;
using System.Threading.Tasks;
using Micky5991.Samp.Net.Commands.Attributes;
using Micky5991.Samp.Net.Commands.Data.Results;
using Micky5991.Samp.Net.Commands.Elements;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;
using Microsoft.AspNetCore.Authorization;

namespace Micky5991.Samp.Net.Commands.Tests
{
    public class TestCommand : Command
    {
        public TestCommand(
            IAuthorizationService authorizationService,
            CommandAttribute commandAttribute,
            string[] aliasNames,
            IReadOnlyList<ParameterDefinition> parameters)
            : base(authorizationService, commandAttribute, aliasNames, parameters)
        {
        }

        public override Task<CommandResult> TryExecuteAsync(IPlayer player, object[] arguments, bool skipPermissions)
        {
            throw new System.NotImplementedException();
        }
    }
}
