using System.Collections.Generic;
using JetBrains.Annotations;
using Micky5991.Samp.Net.Commands.Elements;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;

namespace Micky5991.Samp.Net.Commands.Tests
{
    public class TestCommand : Command
    {
        public TestCommand(string name, string[] aliasNames, string @group, string description, IReadOnlyList<ParameterDefinition> parameters)
            : base(@group, name, aliasNames, description, parameters)
        {
        }

        public override CommandExecutionStatus TryExecute(IPlayer player, object[] arguments, out string errorMessage)
        {
            throw new System.NotImplementedException();
        }
    }
}
