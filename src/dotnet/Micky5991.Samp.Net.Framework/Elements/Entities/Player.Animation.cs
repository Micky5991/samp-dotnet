using System;
using Dawn;
using Micky5991.Samp.Net.Core.Natives.Players;
using Micky5991.Samp.Net.Framework.Data;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;

namespace Micky5991.Samp.Net.Framework.Elements.Entities
{
    /// <inheritdoc cref="IPlayer" />
    public partial class Player
    {
        /// <inheritdoc />
        public AnimationData? Animation
        {
            get
            {
                Guard.Disposal(this.Disposed);

                var index = this.AnimationIndex;

                if (index == 0)
                {
                    return null;
                }

                this.playersNatives.GetAnimationName(index, out var animationLibrary, 32, out var animationName, 32);

                return new AnimationData(animationLibrary, animationName);
            }
        }

        /// <inheritdoc />
        public int AnimationIndex
        {
            get
            {
                Guard.Disposal(this.Disposed);

                return this.playersNatives.GetPlayerAnimationIndex(this.Id);
            }
        }

        /// <inheritdoc />
        public FightStyle FightStyle
        {
            get
            {
                Guard.Disposal(this.Disposed);

                return (FightStyle)this.playersNatives.GetPlayerFightingStyle(this.Id);
            }

            set
            {
                Guard.Disposal(this.Disposed);

                this.playersNatives.SetPlayerFightingStyle(this.Id, (int)value);
            }
        }

        /// <inheritdoc />
        public SpecialAction SpecialAction
        {
            get
            {
                Guard.Disposal(this.Disposed);

                return (SpecialAction)this.playersNatives.GetPlayerSpecialAction(this.Id);
            }

            set
            {
                Guard.Disposal(this.Disposed);

                this.playersNatives.SetPlayerSpecialAction(this.Id, (int)value);
            }
        }

        /// <inheritdoc />
        public void ApplyAnimation(
            AnimationData animation,
            float delta,
            bool loop,
            bool lockX,
            bool lockY,
            bool freeze,
            TimeSpan time,
            bool forceSync = false)
        {
            Guard.Disposal(this.Disposed);

            var (animationLibrary, animationName) = animation;

            this.playersNatives.ApplyAnimation(
                                               this.Id,
                                               animationLibrary,
                                               animationName,
                                               delta,
                                               loop,
                                               lockX,
                                               lockY,
                                               freeze,
                                               (int)time.TotalMilliseconds,
                                               forceSync);
        }

        /// <inheritdoc />
        public void ClearAnimations(bool forceSync = false)
        {
            Guard.Disposal(this.Disposed);

            this.playersNatives.ClearAnimations(this.Id, forceSync);
        }
    }
}
