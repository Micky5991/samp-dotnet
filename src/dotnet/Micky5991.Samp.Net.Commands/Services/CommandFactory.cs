using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Dawn;
using Micky5991.Samp.Net.Commands.Attributes;
using Micky5991.Samp.Net.Commands.Elements;
using Micky5991.Samp.Net.Commands.Interfaces;
using Micky5991.Samp.Net.Framework.Interfaces.Facades;
using Microsoft.AspNetCore.Authorization;

namespace Micky5991.Samp.Net.Commands.Services
{
    /// <inheritdoc />
    public class CommandFactory : ICommandFactory
    {
        private readonly IAuthorizationFacade authorization;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandFactory"/> class.
        /// </summary>
        /// <param name="authorization">Service that determines if a certain command can be executed.</param>
        public CommandFactory(IAuthorizationFacade authorization)
        {
            Guard.Argument(authorization, nameof(authorization)).NotNull();

            this.authorization = authorization;
        }

        /// <inheritdoc />
        public ICollection<ICommand> BuildFromCommandHandler(ICommandHandler commandHandler)
        {
            Guard.Argument(commandHandler, nameof(commandHandler)).NotNull();

            var commands = new List<ICommand>();
            var handlerType = commandHandler.GetType();

            var handlerAuthorizationAttributes = handlerType.GetCustomAttributes<AuthorizeAttribute>(true).ToArray();

            foreach (var method in handlerType.GetMethods())
            {
                var attribute = method.GetCustomAttributes<CommandAttribute>().ToList().FirstOrDefault();
                if (attribute == null)
                {
                    continue;
                }

                var aliasAttributes = method
                            .GetCustomAttributes<CommandAliasAttribute>()
                            .ToArray();

                var authorizeAttributes = method
                                          .GetCustomAttributes<AuthorizeAttribute>(true)
                                          .ToArray();

                commands.Add(this.BuildCommandFromHandler(commandHandler, attribute, aliasAttributes, authorizeAttributes.Concat(handlerAuthorizationAttributes).ToArray(), method));
            }

            return commands;
        }

        private ICommand BuildCommandFromHandler(ICommandHandler handler, CommandAttribute attribute, IEnumerable<CommandAliasAttribute> aliasAttributes, AuthorizeAttribute[] authorizeAttributes, MethodInfo methodInfo)
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

            async Task Executor(object[] x)
            {
                var isAwaitable = methodInfo.ReturnType.GetMethod(nameof(Task.GetAwaiter)) != null;

                if (isAwaitable)
                {
                    await (Task)methodInfo.Invoke(handler, x);

                    return;
                }

                methodInfo.Invoke(handler, x);
            }

            return new HandlerCommand(this.authorization, attribute, authorizeAttributes, aliasNames, parameters, Executor);
        }
    }
}
