using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Dawn;
using Micky5991.Samp.Net.Commands.Attributes;
using Micky5991.Samp.Net.Commands.Elements;
using Micky5991.Samp.Net.Commands.Interfaces;

namespace Micky5991.Samp.Net.Commands.Services
{
    /// <inheritdoc />
    public class CommandFactory : ICommandFactory
    {
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

            return commands;
        }

        private ICommand BuildCommandFromHandler(ICommandHandler handler, CommandAttribute attribute, MethodInfo methodInfo)
        {
            var parameters = methodInfo
                             .GetParameters()
                             .Select(x => new ParameterDefinition(
                                                                  x.Name,
                                                                  x.ParameterType,
                                                                  x.HasDefaultValue,
                                                                  x.DefaultValue))
                             .ToList();

            return new HandlerCommand(attribute.Name, attribute.Group, parameters, handler, methodInfo);
        }
    }
}
