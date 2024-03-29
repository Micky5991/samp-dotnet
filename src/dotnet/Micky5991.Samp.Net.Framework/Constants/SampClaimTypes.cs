using System.Security.Claims;

namespace Micky5991.Samp.Net.Framework.Constants
{
    /// <summary>
    /// List of possible claim types for SA:MP.
    /// </summary>
    public static class SampClaimTypes
    {
        /// <summary>
        /// Nickname of the player when they first connected.
        /// </summary>
        public const string Name = ClaimTypes.Name;

        /// <summary>
        /// Id of the player which they currently have.
        /// </summary>
        public const string PlayerId = "samp/playerid";

        /// <summary>
        /// IP Address of the current principal.
        /// </summary>
        public const string IpAddress = "samp/ipaddress";
    }
}
