using System;

namespace Micky5991.Samp.Net.Framework.Interfaces.Entities
{
    public interface IEntity : IDisposable
    {
        int Id { get; }

        /// <summary>
        /// Gets a value indicating whether the current entity has been disposed.
        /// </summary>
        bool Disposed { get; }
    }
}
