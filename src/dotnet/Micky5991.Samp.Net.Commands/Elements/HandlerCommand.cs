using System;
using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;
using Micky5991.Samp.Net.Commands.Interfaces;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;

namespace Micky5991.Samp.Net.Commands.Elements
{
    /// <summary>
    /// Implementation that executes commands contained in a <see cref="ICommandHandler"/> implementation.
    /// </summary>
    public class HandlerCommand : Command
    {
        private readonly ICommandHandler commandHandler;

        private readonly MethodInfo methodInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="HandlerCommand"/> class.
        /// </summary>
        /// <param name="name">Name of the command.</param>
        /// <param name="group">Optional group of this command.</param>
        /// <param name="parameters">Parameter information about this command.</param>
        /// <param name="commandHandler">Target instance of this command.</param>
        /// <param name="methodInfo">Reflection method information used to invoke the method.</param>
        public HandlerCommand(
            [NotNull] string name,
            [CanBeNull] string? @group,
            [NotNull] IReadOnlyList<ParameterDefinition> parameters,
            ICommandHandler commandHandler,
            MethodInfo methodInfo)
            : base(name, @group, parameters)
        {
            this.commandHandler = commandHandler;
            this.methodInfo = methodInfo;
        }

        /// <inheritdoc />
        public override bool TryExecute(IPlayer player, IList<object> arguments, out string? errorMessage)
        {
            throw new NotImplementedException();
        }
    }
}
