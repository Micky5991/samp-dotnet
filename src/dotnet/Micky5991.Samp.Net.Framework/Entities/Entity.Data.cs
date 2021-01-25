using System.Collections.Generic;
using System.Collections.Immutable;
using Dawn;

namespace Micky5991.Samp.Net.Framework.Entities
{
    /// <inheritdoc cref="Entity"/>
    public abstract partial class Entity
    {
        private readonly object dataChangeLock = new ();

        /// <inheritdoc />
        public IImmutableDictionary<string, object?> Data { get; private set; }

        /// <inheritdoc />
        public void SetData<T>(string key, T? data)
        {
            Guard.Argument(key, nameof(key)).NotNull().NotWhiteSpace();

            lock (this.dataChangeLock)
            {
                this.Data = this.Data.Remove(key).Add(key, data);
            }
        }

        /// <inheritdoc />
        public void SetData(IDictionary<string, object?> data)
        {
            Guard.Argument(data, nameof(data)).NotNull();

            lock (this.dataChangeLock)
            {
                this.Data = this.Data.RemoveRange(data.Keys).AddRange(data);
            }
        }

        /// <inheritdoc />
        public bool HasData(string key)
        {
            Guard.Argument(key, nameof(key)).NotNull().NotWhiteSpace();

            return this.Data.ContainsKey(key);
        }

        /// <inheritdoc />
        public bool TryGetData<T>(string key, out T? data)
        {
            Guard.Argument(key, nameof(key)).NotNull().NotWhiteSpace();

            if (this.Data.TryGetValue(key, out var storedData) == false || storedData is not T convertedData)
            {
                data = default;

                return false;
            }

            data = convertedData;

            return true;
        }

        /// <inheritdoc />
        public bool TryRemoveData<T>(string key, out T? data)
        {
            Guard.Argument(key, nameof(key)).NotNull().NotWhiteSpace();

            lock (this.dataChangeLock)
            {
                if (this.Data.TryGetValue(key, out var storedData) || storedData is not T convertedData)
                {
                    data = default;

                    return false;
                }

                this.Data = this.Data.Remove(key);

                data = convertedData;

                return true;
            }
        }
    }
}
