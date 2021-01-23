using Dawn;

namespace Micky5991.Samp.Net.Framework.Data
{
    /// <summary>
    /// Holds combination of animation library and animation name.
    /// </summary>
    public struct AnimationData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AnimationData"/> struct.
        /// </summary>
        /// <param name="library">Library of this animation.</param>
        /// <param name="name">Name of this animation.</param>
        public AnimationData(string library, string name)
        {
            Guard.Argument(library, nameof(library)).NotNull();
            Guard.Argument(name, nameof(name)).NotNull();

            this.Library = library;
            this.Name = name;
        }

        /// <summary>
        /// Gets the library of this animation.
        /// </summary>
        public string Library { get; }

        /// <summary>
        /// Gets the name of this animation.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Deconstructs this data into <see cref="Library"/> and <see cref="Name"/>.
        /// </summary>
        /// <param name="library">Library of this animation.</param>
        /// <param name="name">Name of this animation.</param>
        public void Deconstruct(out string library, out string name)
        {
            library = this.Library;
            name = this.Name;
        }
    }
}
