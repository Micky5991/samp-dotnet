using System;
using Micky5991.Samp.Net.Commands.Interfaces;

namespace Micky5991.Samp.Net.Commands.Exceptions
{
    /// <summary>
    /// Signals the occurence of duplicate commands.
    /// </summary>
    public class DuplicateCommandException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateCommandException"/> class.
        /// </summary>
        /// <param name="name">Name of the duplicated command.</param>
        /// <param name="group">Optional group name of the command.</param>
        public DuplicateCommandException(string name, string? group)
            : base($"The command \"{name}\" in group \"{group}\" has at least 1 duplicate!")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateCommandException"/> class.
        /// </summary>
        /// <param name="commandHandlerType">Type of the <see cref="ICommandHandler"/> instance that has a duplicate.</param>
        public DuplicateCommandException(Type commandHandlerType)
            : base($"The commandhandler \"{commandHandlerType}\" has at least 1 duplicated command.")
        {
        }
    }
}
