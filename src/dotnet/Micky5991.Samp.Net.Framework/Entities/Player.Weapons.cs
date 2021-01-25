using System.Numerics;
using Dawn;
using Micky5991.Samp.Net.Core.Natives.Players;
using Micky5991.Samp.Net.Core.Natives.Samp;
using Micky5991.Samp.Net.Framework.Data;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;

namespace Micky5991.Samp.Net.Framework.Entities
{
    /// <inheritdoc cref="IPlayer" />
    public partial class Player
    {
        /// <inheritdoc />
        public Weaponstate Weaponstate
        {
            get
            {
                Guard.Disposal(this.Disposed);

                return (Weaponstate)this.playersNatives.GetPlayerWeaponState(this.Id);
            }
        }

        /// <inheritdoc />
        public Weapon CurrentWeapon
        {
            get
            {
                Guard.Disposal(this.Disposed);

                return (Weapon)this.playersNatives.GetPlayerWeapon(this.Id);
            }

            set
            {
                Guard.Disposal(this.Disposed);

                this.playersNatives.SetPlayerArmedWeapon(this.Id, (int)value);
            }
        }

        /// <inheritdoc />
        public int GetCurrentWeaponAmmo()
        {
            Guard.Disposal(this.Disposed);

            return this.playersNatives.GetPlayerAmmo(this.Id);
        }

        /// <inheritdoc />
        public void SetWeaponAmmo(Weapon weapon, int ammo)
        {
            Guard.Disposal(this.Disposed);

            this.playersNatives.SetPlayerAmmo(this.Id, (int)weapon, ammo);
        }

        /// <inheritdoc />
        public void GivePlayerWeapon(Weapon weapon, int ammo)
        {
            Guard.Disposal(this.Disposed);

            this.playersNatives.GivePlayerWeapon(this.Id, (int)weapon, ammo);
        }

        /// <inheritdoc />
        public void ResetWeapons()
        {
            Guard.Disposal(this.Disposed);

            this.playersNatives.ResetPlayerWeapons(this.Id);
        }

        /// <inheritdoc />
        public WeaponData GetWeaponData(int slot)
        {
            Guard.Disposal(this.Disposed);

            this.playersNatives.GetPlayerWeaponData(this.Id, slot, out var weapon, out var ammo);

            return new WeaponData((Weapon)weapon, ammo);
        }

        /// <inheritdoc />
        public void SetSkillLevel(Weaponskill skill, int level)
        {
            Guard.Disposal(this.Disposed);

            this.playersNatives.SetPlayerSkillLevel(this.Id, (int)skill, level);
        }

        /// <inheritdoc />
        public void GetLastShotVectors(out Vector3 originPosition, out Vector3 hitPosition)
        {
            Guard.Disposal(this.Disposed);

            this.playersNatives.GetPlayerLastShotVectors(
                                                         this.Id,
                                                         out var originX,
                                                         out var originY,
                                                         out var originZ,
                                                         out var hitPosX,
                                                         out var hitPosY,
                                                         out var hitPosZ);

            originPosition = new Vector3(originX, originY, originZ);
            hitPosition = new Vector3(hitPosX, hitPosY, hitPosZ);
        }
    }
}
