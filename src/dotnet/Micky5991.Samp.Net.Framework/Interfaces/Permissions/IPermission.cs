namespace Micky5991.Samp.Net.Framework.Interfaces.Permissions
{
    /// <summary>
    /// General permission definition of this permission.
    /// </summary>
    public interface IPermission
    {
        /// <summary>
        /// Gets the unique full qualified name of this permission.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the description of this permission.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Gets a value indicating whether this permission should be granted by default.
        /// </summary>
        public bool DefaultValue { get; }
    }
}
