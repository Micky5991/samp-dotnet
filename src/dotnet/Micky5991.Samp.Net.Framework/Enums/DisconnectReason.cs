namespace Micky5991.Samp.Net.Framework.Enums
{
    /// <summary>
    /// Possible list of reasons why the player disconnected.
    /// </summary>
    public enum DisconnectReason
    {
        /// <summary>
        /// Player disconnected due to a timeout / crash.
        /// </summary>
        Timeout,

        /// <summary>
        /// Player purposefully quit from the game menu or with /q.
        /// </summary>
        Quit,

        /// <summary>
        /// Player has been kicked or banned from the server.
        /// </summary>
        Kick
    }
}
