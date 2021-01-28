using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Dawn;
using Micky5991.Samp.Net.Commands.Attributes;
using Micky5991.Samp.Net.Commands.Elements;
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
                var attribute = method.GetCustomAttributes<CommandAttribute>().ToList().FirstOrDefault();
                if (attribute == null)
                {
                    continue;
                }

                var aliasAttributes = method
                            .GetCustomAttributes<CommandAliasAttribute>()
                            .ToArray();

                commands.Add(this.BuildCommandFromHandler(commandHandler, attribute, aliasAttributes, method));
            }

            return commands;
        }

        private ICommand BuildCommandFromHandler(ICommandHandler handler, CommandAttribute attribute, IEnumerable<CommandAliasAttribute> aliasAttributes, MethodBase methodInfo)
        {
            var parameters = methodInfo
                             .GetParameters()
                             .Select(x => new ParameterDefinition(
                                                                  x.Name,
                                                                  x.ParameterType,
                                                                  x.HasDefaultValue,
                                                                  x.DefaultValue))
                             .ToList();

            var aliasNames = aliasAttributes.Select(x => x.Name).ToArray();

            return new HandlerCommand(this.handlerLogger, attribute.Group, attribute.Name, aliasNames, attribute.Description, parameters, handler, x => methodInfo.Invoke(handler, x));
        }
    }
}
