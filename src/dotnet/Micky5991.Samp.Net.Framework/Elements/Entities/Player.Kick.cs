using System;
using System.Threading.Tasks;
using Dawn;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;

namespace Micky5991.Samp.Net.Framework.Elements.Entities
{
    /// <inheritdoc cref="IPlayer" />
    public partial class Player
    {
        /// <inheritdoc />
        public async void Kick(TimeSpan delay)
        {
            Guard.Argument(delay, nameof(delay)).Min(TimeSpan.Zero);

            Guard.Disposal(this.Disposed);

            await Task.Delay(delay);

            this.sampNatives.Kick(this.Id);
        }

        /// <inheritdoc />
        public void Kick()
        {
            this.Kick(TimeSpan.FromMilliseconds(1));
        }

        /// <inheritdoc />
        public async void Ban(string reason, TimeSpan delay)
        {
            Guard.Argument(reason, nameof(reason)).NotNull().NotEmpty();
            Guard.Argument(delay, nameof(delay)).Min(TimeSpan.MinValue);

            Guard.Disposal(this.Disposed);

            await Task.Delay(delay);

            this.sampNatives.BanEx(this.Id, reason);
        }

        /// <inheritdoc />
        public void Ban(string reason)
        {
            this.Ban(reason, TimeSpan.FromMilliseconds(1));
        }

        /// <inheritdoc />
        public void Ban(TimeSpan delay)
        {
            Guard.Argument(delay, nameof(delay)).Min(TimeSpan.MinValue);

            Guard.Disposal(this.Disposed);

            this.sampNatives.Ban(this.Id);
        }

        /// <inheritdoc />
        public void Ban()
        {
            this.Ban(TimeSpan.FromMilliseconds(1));
        }
    }
}
