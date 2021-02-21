using Micky5991.Samp.Net.Core.Natives.Players;

namespace Micky5991.Samp.Net.Framework.Enums
{
    /// <summary>
    /// Style to change the position and rotation of the camera like.
    /// </summary>
    public enum CameraCutStyle
    {
        /// <summary>
        /// Camera will be moved smoothly to the new destination.
        /// </summary>
        CameraMove = PlayersConstants.CameraMove,

        /// <summary>
        /// Camera will be moved instantly to the new destination.
        /// </summary>
        CameraCut = PlayersConstants.CameraCut,
    }
}
