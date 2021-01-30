using System.Collections.Generic;
using Micky5991.Samp.Net.Commands.Attributes;
using Micky5991.Samp.Net.Commands.Elements;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;

namespace Micky5991.Samp.Net.Commands.Tests
{
    public class TestCommand : Command
    {
        public TestCommand(
            CommandAttribute commandAttribute,
            string[] aliasNames,
            IReadOnlyList<ParameterDefinition> parameters)
            : base(commandAttribute, aliasNames, parameters)
        {
        }

        public override CommandExecutionStatus TryExecute(IPlayer player, object[] arguments, out string errorMessage)
        {
            throw new System.NotImplementedException();
        }
    }
}
