using System;

namespace Micky5991.Samp.Net.Framework.Exceptions
{
    /// <summary>
    /// Signals that a certain entity limit has been reached.
    /// </summary>
    public class EntityLimitReachedException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityLimitReachedException"/> class.
        /// </summary>
        /// <param name="entityType">Type of entity which limit has been reached.</param>
        public EntityLimitReachedException(Type entityType)
            : base($"Limit for entity {entityType} has been reached.")
        {
        }
    }
}
