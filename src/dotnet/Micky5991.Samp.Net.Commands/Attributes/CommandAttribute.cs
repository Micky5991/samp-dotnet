using System;
using Dawn;
using JetBrains.Annotations;

namespace Micky5991.Samp.Net.Commands.Attributes
{
    /// <summary>
    /// Attribute which defines how the command is called and how its used.
    /// </summary>
    [MeansImplicitUse]
    [AttributeUsage(AttributeTargets.Method)]
    public class CommandAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandAttribute"/> class.
        /// </summary>
        /// <param name="group">Optional group this command is assigned to.</param>
        /// <param name="name">Name of the command.</param>
        public CommandAttribute(string? group, string name)
        {
            Guard.Argument(name, nameof(name)).NotNull().NotWhiteSpace();
            Guard.Argument(group, nameof(group)).NotWhiteSpace();

            this.Name = name;
            this.Group = group;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandAttribute"/> class.
        /// </summary>
        /// <param name="name">Name of the command.</param>
        public CommandAttribute(string name)
            : this(null!, name)
        {
        }

        /// <summary>
        /// Gets the name of this command.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the current optional group of this command.
        /// </summary>
        public string? Group { get; }

        /// <summary>
        /// Gets or sets the optional description for this command.
        /// </summary>
        public string? Description { get; set; }
    }
}
