using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Dawn;
using Micky5991.Samp.Net.Commands.Attributes;
using Micky5991.Samp.Net.Commands.Elements;
using Micky5991.Samp.Net.Commands.Exceptions;
using Micky5991.Samp.Net.Commands.Interfaces;
using Microsoft.Extensions.Logging;

namespace Micky5991.Samp.Net.Commands.Services
{
    /// <inheritdoc />
    public class CommandFactory : ICommandFactory
    {
        private readonly ILogger<HandlerCommand> handlerLogger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandFactory"/> class.
        /// </summary>
        /// <param name="handlerLogger">Logger needed for <see cref="HandlerCommand"/>.</param>
        public CommandFactory(ILogger<HandlerCommand> handlerLogger)
        {
            Guard.Argument(handlerLogger, nameof(handlerLogger)).NotNull();

            this.handlerLogger = handlerLogger;
        }

        /// <inheritdoc />
        public ICollection<ICommand> BuildFromCommandHandler(ICommandHandler commandHandler)
        {
            Guard.Argument(commandHandler, nameof(commandHandler)).NotNull();

            var commands = new List<ICommand>();

            foreach (var method in commandHandler.GetType().GetMethods())
            {
                var attributes = method.GetCustomAttributes<CommandAttribute>().ToList();
                if (attributes.Count == 0)
                {
                    continue;
                }

                foreach (var attribute in attributes)
                {
                    commands.Add(this.BuildCommandFromHandler(commandHandler, attribute, method));
                }
            }

            if (commands.GroupBy(x => $"{x.Group ?? string.Empty}:{x.Name}").Any(x => x.Count() > 1))
            {
                throw new DuplicateCommandException(commandHandler.GetType());
            }

            return commands;
        }

        private ICommand BuildCommandFromHandler(ICommandHandler handler, CommandAttribute attribute, MethodBase methodInfo)
        {
            var parameters = methodInfo
                             .GetParameters()
                             .Select(x => new ParameterDefinition(
                                                                  x.Name,
                                                                  x.ParameterType,
                                                                  x.HasDefaultValue,
                                                                  x.DefaultValue))
                             .ToList();

            return new HandlerCommand(this.handlerLogger, attribute.Name, attribute.Group, parameters, handler, x => methodInfo.Invoke(handler, x));
        }
    }
}
