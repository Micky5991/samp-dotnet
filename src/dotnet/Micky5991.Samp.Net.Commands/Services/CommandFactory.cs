using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Dawn;
using Micky5991.Samp.Net.Commands.Attributes;
using Micky5991.Samp.Net.Commands.Elements;
using Micky5991.Samp.Net.Commands.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace Micky5991.Samp.Net.Commands.Services
{
    /// <inheritdoc />
    public class CommandFactory : ICommandFactory
    {
        private readonly IAuthorizationService authorizationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandFactory"/> class.
        /// </summary>
        /// <param name="authorizationService">Service that determines if a certain command can be executed.</param>
        public CommandFactory(IAuthorizationService authorizationService)
        {
            Guard.Argument(authorizationService, nameof(authorizationService)).NotNull();

            this.authorizationService = authorizationService;
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

            return new HandlerCommand(this.authorizationService, attribute, aliasNames, parameters, x => methodInfo.Invoke(handler, x));
        }
    }
}
