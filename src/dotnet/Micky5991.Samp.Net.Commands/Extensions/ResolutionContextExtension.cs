using System;
using AutoMapper;
using Dawn;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;

namespace Micky5991.Samp.Net.Commands.Extensions
{
    /// <summary>
    /// General extensions to simplify <see cref="ResolutionContext"/> usage.
    /// </summary>
    public static class ResolutionContextExtension
    {
        /// <summary>
        /// Tries to get the executor from context of this resolution.
        /// </summary>
        /// <param name="context">Context to search in.</param>
        /// <param name="executor">Resulting executor of this resolution. Null if no context was set or executor is not <see cref="IPlayer"/>.</param>
        /// <returns>true if executor has been found of type <see cref="IPlayer"/>, false otherwise.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="context"/> is null.</exception>
        public static bool TryGetCommandExecutor(this ResolutionContext context, out IPlayer? executor)
        {
            Guard.Argument(context, nameof(context)).NotNull();

            executor = default;

            if (context.Items.TryGetValue(CommandConstants.CommandExecutorKey, out var value) == false)
            {
                return false;
            }

            if (value is not IPlayer player)
            {
                return false;
            }

            executor = player;

            return true;
        }
    }
}
