namespace Micky5991.Samp.Net.Framework.Options
{
    /// <summary>
    /// Options used for general gamemode behavior.
    /// </summary>
    public class GamemodeOptions
    {
        /// <summary>
        /// Constant used for a gamemode section.
        /// </summary>
        public const string Gamemode = "Gamemode";

        /// <summary>
        /// Gets or sets a value indicating whether logs from the native SA:MP server should be redirect to the .NET logger.
        /// </summary>
        public bool LogRedirection { get; set; }
    }
}
