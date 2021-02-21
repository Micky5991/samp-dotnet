using System;
using System.Numerics;
using System.Threading.Tasks;
using Dawn;
using Micky5991.Samp.Net.Core.Natives.Players;
using Micky5991.Samp.Net.Framework.Enums;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;

namespace Micky5991.Samp.Net.Framework.Elements.Entities
{
    /// <inheritdoc cref="IPlayer"/>
    public partial class Player
    {
        /// <inheritdoc />
        public void ToggleSpectating(bool spectating)
        {
            Guard.Disposal(this.Disposed);

            this.playersNatives.TogglePlayerSpectating(this.Id, spectating);
        }

        /// <inheritdoc />
        public async void Spectate(IPlayer player, SpectateMode spectateMode)
        {
            Guard.Disposal(this.Disposed);
            Guard.Disposal(player.Disposed);

            this.ToggleSpectating(true);

            await Task.Delay(1);

            this.playersNatives.PlayerSpectatePlayer(this.Id, player.Id, (int)spectateMode);
        }

        /// <inheritdoc />
        public async void Spectate(IVehicle vehicle, SpectateMode spectateMode)
        {
            Guard.Disposal(this.Disposed);
            Guard.Disposal(vehicle.Disposed);

            this.ToggleSpectating(true);

            await Task.Delay(1);

            this.playersNatives.PlayerSpectateVehicle(this.Id, vehicle.Id, (int)spectateMode);
        }

        /// <inheritdoc />
        public void SetCameraBehindPlayer()
        {
            Guard.Disposal(this.Disposed);

            this.playersNatives.SetCameraBehindPlayer(this.Id);
        }

        /// <inheritdoc />
        public async void SetCamera(Vector3 position, Vector3 rotation, CameraCutStyle cutStyle)
        {
            Guard.Disposal(this.Disposed);

            await Task.Delay(1);

            this.playersNatives.SetPlayerCameraPos(this.Id, position.X, position.Y, position.Z);
            this.playersNatives.SetPlayerCameraLookAt(this.Id, rotation.X, rotation.Y, rotation.Z, (int)cutStyle);
        }

        /// <inheritdoc />
        public async void InterpolateCameraPosition(
            Vector3 startPosition,
            Vector3 endPosition,
            TimeSpan timeSpan,
            CameraCutStyle cutStyle = CameraCutStyle.CameraMove)
        {
            Guard.Argument(timeSpan, nameof(timeSpan)).Min(TimeSpan.Zero);

            Guard.Disposal(this.Disposed);

            await Task.Delay(1);

            this.playersNatives.InterpolateCameraPos(
                                                     this.Id,
                                                     startPosition.X,
                                                     startPosition.Y,
                                                     startPosition.Z,
                                                     endPosition.X,
                                                     endPosition.Y,
                                                     endPosition.Z,
                                                     (int)timeSpan.TotalMilliseconds,
                                                     (int)cutStyle);
        }

        /// <inheritdoc />
        public async void InterpolateCameraLookAt(
            Vector3 startRotation,
            Vector3 endRotation,
            TimeSpan timeSpan,
            CameraCutStyle cutStyle = CameraCutStyle.CameraMove)
        {
            Guard.Argument(timeSpan, nameof(timeSpan)).Min(TimeSpan.Zero);

            Guard.Disposal(this.Disposed);

            await Task.Delay(1);

            this.playersNatives.InterpolateCameraLookAt(
                                                     this.Id,
                                                     startRotation.X,
                                                     startRotation.Y,
                                                     startRotation.Z,
                                                     endRotation.X,
                                                     endRotation.Y,
                                                     endRotation.Z,
                                                     (int)timeSpan.TotalMilliseconds,
                                                     (int)cutStyle);
        }
    }
}
