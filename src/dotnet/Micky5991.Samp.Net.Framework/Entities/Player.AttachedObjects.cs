using System.Numerics;
using Dawn;
using Micky5991.Samp.Net.Core.Natives.Players;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;

namespace Micky5991.Samp.Net.Framework.Entities
{
    /// <inheritdoc cref="IPlayer" />
    public partial class Player
    {
        /// <inheritdoc />
        public bool SetAttachedObject(
            int index,
            int modelid,
            int bone,
            Vector3 offset,
            Vector3 rotation,
            Vector3 scale,
            int materialColor1 = 0,
            int materialColor2 = 0)
        {
            Guard.Argument(index, nameof(index)).NotNegative().Max(PlayersConstants.MaxPlayerAttachedObjects);
            Guard.Disposal(this.Disposed);

            return this.playersNatives.SetPlayerAttachedObject(
                                                               this.Id,
                                                               index,
                                                               modelid,
                                                               bone,
                                                               offset.X,
                                                               offset.Y,
                                                               offset.Z,
                                                               rotation.X,
                                                               rotation.Y,
                                                               rotation.Z,
                                                               scale.X,
                                                               scale.Y,
                                                               scale.Z,
                                                               materialColor1,
                                                               materialColor2);
        }

        /// <inheritdoc />
        public void RemoveAttachedObject(int index)
        {
            Guard.Argument(index, nameof(index)).NotNegative().Max(PlayersConstants.MaxPlayerAttachedObjects);
            Guard.Disposal(this.Disposed);

            this.playersNatives.RemovePlayerAttachedObject(this.Id, index);
        }

        /// <inheritdoc />
        public bool IsAttachedObjectSlotUsed(int index)
        {
            Guard.Argument(index, nameof(index)).NotNegative().Max(PlayersConstants.MaxPlayerAttachedObjects);
            Guard.Disposal(this.Disposed);

            return this.playersNatives.IsPlayerAttachedObjectSlotUsed(this.Id, index);
        }

        /// <inheritdoc />
        public void EditAttachedObject(int index)
        {
            Guard.Argument(index, nameof(index)).NotNegative().Max(PlayersConstants.MaxPlayerAttachedObjects);
            Guard.Disposal(this.Disposed);

            this.playersNatives.EditAttachedObject(this.Id, index);
        }
    }
}
