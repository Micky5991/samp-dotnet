using System;
using Dawn;
using Micky5991.Samp.Net.Commands.Interfaces;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;

namespace Micky5991.Samp.Net.Commands.Elements
{
    /// <summary>
    /// Simple data structure that contains information about a parameter in a <see cref="ICommand"/> instance.
    /// </summary>
    public struct ParameterDefinition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterDefinition"/> struct.
        /// </summary>
        /// <param name="name">Name of the parameter.</param>
        /// <param name="type">Type of the parameter.</param>
        /// <param name="hasDefault">true if this parameter has a default value.</param>
        /// <param name="defaultValue">Default value of the parameter. Will be null if <paramref name="hasDefault"/> is false.</param>
        /// <exception cref="ArgumentNullException"><paramref name="type"/> is null or empty.</exception>
        /// <exception cref="ArgumentException"><paramref name="name"/> is null or whitespace.</exception>
        public ParameterDefinition(string name, Type type, bool hasDefault, object? defaultValue)
        {
            Guard.Argument(name, nameof(name)).NotEmpty().NotWhiteSpace();
            Guard.Argument(type, nameof(type)).NotNull();

            this.Name = name;
            this.Type = type;
            this.HasDefault = hasDefault;
            this.DefaultValue = hasDefault ? defaultValue : null;
        }

        /// <summary>
        /// Gets the name of the parameter.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the type of the parameter.
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// Gets a value indicating whether the parameter has a value.
        /// </summary>
        public bool HasDefault { get; }

        /// <summary>
        /// Gets the default value of the parameter.
        /// </summary>
        public object? DefaultValue { get; }

        /// <summary>
        /// Returns a default <see cref="ParameterDefinition"/> with a parameter named "player" and of type <see cref="IPlayer"/>.
        /// </summary>
        /// <returns>Created <see cref="ParameterDefinition"/> instance.</returns>
        public static ParameterDefinition Player()
        {
            return new ("player", typeof(IPlayer), false, null);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        /// <summary>
        /// Provides a type safe comparison function for this type.
        /// </summary>
        /// <param name="other">Other <see cref="ParameterDefinition"/> type.</param>
        /// <returns>true if the type is equal.</returns>
        public bool Equals(ParameterDefinition other)
        {
            return this.Name == other.Name && this.Type == other.Type && this.HasDefault == other.HasDefault && Equals(this.DefaultValue, other.DefaultValue);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = this.Name.GetHashCode();
                hashCode = (hashCode * 397) ^ this.Type.GetHashCode();
                hashCode = (hashCode * 397) ^ this.HasDefault.GetHashCode();
                hashCode = (hashCode * 397) ^ (this.DefaultValue != null ? this.DefaultValue.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
