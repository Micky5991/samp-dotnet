using System;

namespace Micky5991.Samp.Net.Commands.Attributes
{
    /// <summary>
    /// Adds another alias for the already defined command.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class CommandAliasAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandAliasAttribute"/> class.
        /// </summary>
        /// <param name="name">Alias name to register.</param>
        public CommandAliasAttribute(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Gets the alias of this command.
        /// </summary>
        public string Name { get; }
    }
}
