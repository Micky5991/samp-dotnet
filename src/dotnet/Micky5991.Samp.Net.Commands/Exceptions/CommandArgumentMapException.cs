using System;
using AutoMapper;

namespace Micky5991.Samp.Net.Commands.Exceptions
{
    /// <summary>
    /// Exception that will be thrown when the mapper is unable to map source type to target type.
    /// </summary>
    public class CommandArgumentMapException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandArgumentMapException"/> class.
        /// </summary>
        /// <param name="parameterIndex">Index of the parameter that failed to map.</param>
        /// <param name="autoMapperMappingException">Inner exception of this exception.</param>
        public CommandArgumentMapException(int parameterIndex, AutoMapperMappingException autoMapperMappingException)
            : base($"Unable to map parameter \"{parameterIndex}\" to {autoMapperMappingException.Types?.DestinationType}", autoMapperMappingException)
        {
            this.ParameterIndex = parameterIndex;
        }

        /// <summary>
        /// Gets the name of the parameter that failed.
        /// </summary>
        public int ParameterIndex { get; }
    }
}
