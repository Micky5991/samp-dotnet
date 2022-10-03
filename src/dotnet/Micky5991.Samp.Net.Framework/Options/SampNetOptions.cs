namespace Micky5991.Samp.Net.Framework.Options
{
    /// <summary>
    /// Options used for general gamemode behavior.
    /// </summary>
    public class SampNetOptions
    {
        /// <summary>
        /// Constant used for a gamemode section.
        /// </summary>
        public const string SampNet = "SampNet";

        /// <summary>
        /// Gets or sets a value indicating whether logs from the native SA:MP server should be redirect to the .NET logger.
        /// </summary>
        public bool LogRedirection { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the authorization should use the default policy for unknown policies.
        /// </summary>
        public bool UseDefaultPolicyForUnknownPolicy { get; set; }
    }
}
