using Micky5991.Samp.Net.Core.Natives.Samp;

namespace Micky5991.Samp.Net.Framework.Data
{
    /// <summary>
    /// Holds information about weapon model and ammo.
    /// </summary>
    public struct WeaponData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WeaponData"/> struct.
        /// </summary>
        /// <param name="model">Model of the weapon.</param>
        /// <param name="ammo">Ammo amount of this weapon.</param>
        public WeaponData(Weapon model, int ammo)
        {
            this.Model = model;
            this.Ammo = ammo;
        }

        /// <summary>
        /// Gets the model of this weapon.
        /// </summary>
        public Weapon Model { get; }

        /// <summary>
        /// Gets the ammo of this weapon.
        /// </summary>
        public int Ammo { get; }

        /// <summary>
        /// Deconstructs this data into <see cref="Model"/> and <see cref="Ammo"/>.
        /// </summary>
        /// <param name="model">Current weapon model.</param>
        /// <param name="ammo">Current ammo amount.</param>
        public void Deconstruct(out Weapon model, out int ammo)
        {
            model = this.Model;
            ammo = this.Ammo;
        }
    }
}
