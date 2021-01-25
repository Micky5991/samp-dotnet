using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Micky5991.Samp.Net.Framework.Interfaces.Entities
{
    /// <summary>
    /// Represents any samp entity.
    /// </summary>
    public interface IEntity : IDisposable
    {
        /// <summary>
        /// Gets current non-negative id of this entity.
        /// </summary>
        /// <exception cref="ObjectDisposedException"><see cref="Disposed"/> is true.</exception>
        int Id { get; }

        /// <summary>
        /// Gets a value indicating whether the current entity has been disposed.
        /// </summary>
        bool Disposed { get; }

        /// <summary>
        /// Gets the current store for <see cref="SetData{T}"/>, <see cref="HasData"/>, <see cref="TryGetData{T}"/>, <see cref="TryRemoveData{T}"/>.
        /// </summary>
        IImmutableDictionary<string, object?> Data { get; }

        /// <summary>
        /// Adds data that is only available during the lifetime of this entity. Use this method to avoid race conditions.
        /// To share data on players to filterscripts or gamemodes, use the native SAMP player or server-variables
        /// available in <see cref="IPlayer"/>.
        /// </summary>
        /// <param name="key">Unique key to store this <paramref name="data"/> to.</param>
        /// <param name="data">Data that should be stored.</param>
        /// <typeparam name="T">Datatype of <paramref name="data"/>.</typeparam>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="key"/> is empty.</exception>
        void SetData<T>(string key, T? data);

        /// <summary>
        /// Adds mutliple rows of data that is only available during the lifetime of this entity. Use this method to
        /// avoid race conditions. To share data on players to filterscripts or gamemodes, use the native SAMP player
        /// or server-variables available in <see cref="IPlayer"/>.
        /// </summary>
        /// <param name="data">Data that should be stored.</param>
        /// <exception cref="ArgumentNullException"><paramref name="data"/> is null.</exception>
        void SetData(IDictionary<string, object?> data);

        /// <summary>
        /// Checks if the given <paramref name="key"/> was defined.
        /// </summary>
        /// <param name="key">Unique key to search for.</param>
        /// <returns>true if the key has been used, false otherwise.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="key"/> is empty.</exception>
        bool HasData(string key);

        /// <summary>
        /// Tries to get the specified data by <paramref name="key"/>. If the return value is false, <paramref name="data"/> will be null.
        /// </summary>
        /// <param name="key">Unique key to search for.</param>
        /// <param name="data">Data that has been stored before, otherwise null.</param>
        /// <typeparam name="T">Datatype of <paramref name="data"/>.</typeparam>
        /// <returns>true if a value has been found with the type <typeparamref name="T"/>, false otehrwise.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="key"/> is empty.</exception>
        bool TryGetData<T>(string key, out T? data);

        /// <summary>
        /// Tries to remove the data from the entity and return the stored value in <paramref name="data"/>.
        /// Use this method to avoid race conditions. If the return value is false, <paramref name="data"/> will be null.
        /// </summary>
        /// <param name="key">Unique key to search for.</param>
        /// <param name="data">Data that has been stored before, otherwise null.</param>
        /// <typeparam name="T">Datatype of <paramref name="data"/>.</typeparam>
        /// <returns>true if a value has been found with the specified type <typeparamref name="T"/> and removed, false otherwise.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="key"/> is empty.</exception>
        bool TryRemoveData<T>(string key, out T? data);

        /// <summary>
        /// Gets if the entity is still valid.
        /// </summary>
        /// <returns>true if the instance has not been disposed, false otherwise.</returns>
        bool Valid();
    }
}
