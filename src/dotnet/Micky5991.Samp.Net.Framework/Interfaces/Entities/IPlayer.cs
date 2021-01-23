using System;
using System.Drawing;
using System.Net;
using System.Numerics;
using Micky5991.Samp.Net.Core.Natives.Players;
using Micky5991.Samp.Net.Core.Natives.Samp;
using Micky5991.Samp.Net.Framework.Data;

namespace Micky5991.Samp.Net.Framework.Interfaces.Entities
{
    /// <summary>
    /// Represents a player on the server.
    /// </summary>
    public interface IPlayer : IWorldEntity
    {
        /// <summary>
        /// Gets or sets the current name of the player.
        /// </summary>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        /// <exception cref="ArgumentException">Value is empty or invalid length.</exception>
        string Name { get; set; }

        /// <summary>
        /// Gets the current IP address of this player.
        /// </summary>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        IPAddress Ip { get; }

        /// <summary>
        /// Gets the current ping of this player.
        /// </summary>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        int Ping { get; }

        /// <summary>
        /// Gets or sets the current velocity of this entity.
        /// </summary>
        /// <exception cref="ObjectDisposedException"><see cref="IWorldEntity"/> was disposed.</exception>
        public Vector3 Velocity { get; set; }

        /// <summary>
        /// Gets or sets the money of this player.
        /// </summary>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        int Money { get; set; }

        /// <summary>
        /// Gets or sets the current nametag color of this player.
        /// </summary>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        Color Color { get; set; }

        /// <summary>
        /// Gets or sets the current health of this player.
        /// </summary>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        float Health { get; set; }

        /// <summary>
        /// Gets or sets the current armor of this player.
        /// </summary>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        float Armor { get; set; }

        /// <summary>
        /// Gets the current animation index of this player.
        /// </summary>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        int AnimationIndex { get; }

        /// <summary>
        /// Gets or sets the current interior id of this player.
        /// </summary>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        int Interior { get; set; }

        /// <summary>
        /// Gets the current weapon state of this player.
        /// </summary>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        Weaponstate Weaponstate { get; }

        /// <summary>
        /// Gets or sets the current weapon of this player.
        /// </summary>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        Weapon CurrentWeapon { get; set; }

        /// <summary>
        /// Gets the id of the player this player targets. Null if no player is targeted.
        /// </summary>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        int? TargetPlayer { get; }

        /// <summary>
        /// Gets the id of the actor this player targets. Null if no actor is targeted.
        /// </summary>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        int? TargetActor { get; }

        /// <summary>
        /// Gets or sets the team id of this player.
        /// </summary>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        int Team { get; set; }

        /// <summary>
        /// Gets or sets the score of this player.
        /// </summary>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        int Score { get; set; }

        /// <summary>
        /// Gets or sets the drunk level of this player.
        /// </summary>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        int DrunkLevel { get; set; }

        /// <summary>
        /// Gets or sets the current skin of this player.
        /// </summary>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        int Skin { get; set; }

        /// <summary>
        /// Gets the current state of the player.
        /// </summary>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        PlayerState State { get; }

        /// <summary>
        /// Gets or sets the current time visible to this player.
        /// </summary>
        TimeData Time { get; set; }

        /// <summary>
        /// Gets or sets the wanted level of this player.
        /// </summary>
        int WantedLevel { get; set; }

        /// <summary>
        /// Gets or sets the fighting style of this player.
        /// </summary>
        FightStyle FightStyle { get; set; }

        /// <summary>
        /// Puts the current player into the specified vehicle.
        /// </summary>
        /// <param name="vehicle">Target vehicle to set the player in.</param>
        /// <param name="seat">Seat of the vehicle, 0 = driver.</param>
        /// <returns>true if the player was set into the vehicle, false otherwise.</returns>
        /// <exception cref="ObjectDisposedException"><paramref name="vehicle"/> or <see cref="IPlayer"/> was disposed.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="seat"/> is negative.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="vehicle"/> is null.</exception>
        bool PutPlayerIntoVehicle(IVehicle vehicle, int seat = 0);

        /// <summary>
        /// Sends a chat message to the player.
        /// </summary>
        /// <param name="color">Color of the message.</param>
        /// <param name="message">Message to send.</param>
        /// <exception cref="ArgumentNullException"><paramref name="message"/> is null.</exception>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> was disposed.</exception>
        void SendMessage(Color color, string message);

        /// <summary>
        /// Sets information about spawn location and equipment of a specific player.
        /// </summary>
        /// <param name="team">Team the player is currently in.</param>
        /// <param name="skin">Id of the skin model of this player.</param>
        /// <param name="position">Position to spawn the player on.</param>
        /// <param name="rotation">Rotation of the player on spawn.</param>
        /// <param name="weapons">List of weapons the player holds on spawn.</param>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="weapons"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="weapons"/> exceeds 3 elements.</exception>
        void SetSpawnInfo(int team, int skin, Vector3 position, float rotation, params WeaponData[] weapons);

        /// <summary>
        /// Spawns the player on the server spawn point or the position set in <see cref="SetSpawnInfo"/>.
        /// </summary>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        void Spawn();

        /// <summary>
        /// Returns the current ammo amount of the weapon the player holds.
        /// </summary>
        /// <returns>Amount of ammo.</returns>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        int GetCurrentWeaponAmmo();

        /// <summary>
        /// Sets the amount of ammo the player has for the specified <paramref name="weapon"/>.
        /// </summary>
        /// <param name="weapon">Target weapon to set the ammo of.</param>
        /// <param name="ammo">Amount of ammo to set.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="ammo"/> was negative.</exception>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        void SetWeaponAmmo(Weapon weapon, int ammo);

        /// <summary>
        /// Adds the specific weapon to the player. If the player already owns this weapon, <paramref name="ammo"/> will
        /// be added to the total amount.
        /// </summary>
        /// <param name="weapon"><see cref="Weapon"/> to give the player to.</param>
        /// <param name="ammo">Non-negative amount of ammo to give.</param>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        void GivePlayerWeapon(Weapon weapon, int ammo);

        /// <summary>
        /// Removes all weapons of this player.
        /// </summary>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        void ResetWeapons();

        /// <summary>
        /// Gets the <see cref="WeaponData"/> of the weapon in the specified <paramref name="slot"/>.
        /// </summary>
        /// <param name="slot">Slot to get from.</param>
        /// <returns>Created <see cref="WeaponData"/>.</returns>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        WeaponData GetWeaponData(int slot);

        /// <summary>
        /// Toggles the visible clock on the screen of this player.
        /// </summary>
        /// <param name="visible">true if the clock should be visible, false otherwise.</param>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        void ToggleClock(bool visible);

        /// <summary>
        /// Forces class selection on next respawn.
        /// </summary>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        void ForceClassSelection();

        /// <summary>
        /// Plays a crime report for the player about the <paramref name="suspect"/>.
        /// </summary>
        /// <param name="suspect">Target suspect that will be descriped in the report.</param>
        /// <param name="crime">10-code crime report.</param>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        void PlayCrimeReport(IPlayer suspect, int crime);

        /// <summary>
        /// Plays the specifed audio stream for this player without a position or distance.
        /// To use relative audio position, use <see cref="PlayAudioStream(string,Vector3,float)"/> instead.
        /// </summary>
        /// <param name="url">Url to play.</param>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="url"/> is null.</exception>
        void PlayAudioStream(string url);

        /// <summary>
        /// Plays the specified audio stream for this player in the 3d world.
        /// To play audio without position in the world, use <see cref="PlayAudioStream(string)"/> instead.
        /// </summary>
        /// <param name="url">Url to play.</param>
        /// <param name="position">Position in the world.</param>
        /// <param name="distance">Distance where this stream is hearable.</param>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="url"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="distance"/> is negative.</exception>
        void PlayAudioStream(string url, Vector3 position, float distance = 50);

        /// <summary>
        /// Stops the audio stream for this plays.
        /// </summary>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        void StopAudioStream();

        /// <summary>
        /// Sets the shop name and loads its script, like the ammunation menu.
        /// </summary>
        /// <param name="shopname">Name of the shop to load.</param>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="shopname"/> is null.</exception>
        void SetShopName(string shopname);

        /// <summary>
        /// Sets the skill level for the specified <paramref name="skill"/>.
        /// </summary>
        /// <param name="skill">Skill to change.</param>
        /// <param name="level">Level to set between 0 and 999.</param>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        void SetSkillLevel(Weaponskill skill, int level);

        /// <summary>
        /// Gets the vehicle id of the vehicle vhere the player stands on.
        /// </summary>
        /// <returns>Id of the vehicle the player stands on, null otherwise.</returns>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        int? GetSurfingVehicleId();

        /// <summary>
        /// Gets the id of the object this player stands on.
        /// </summary>
        /// <returns>Id of the vehicle this player stands on, null otherwise.</returns>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        int? GetSurfingObjectId();

        /// <summary>
        /// Removes the building <paramref name="model"/> for this player.
        /// </summary>
        /// <param name="model">Model of the building to remove.</param>
        /// <param name="position">Middle position of the circle to remove the objects in.</param>
        /// <param name="radius">Radius of the removal circle.</param>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="radius"/> is negative.</exception>
        void RemoveBuildingForPlayer(int model, Vector3 position, float radius);

        /// <summary>
        /// Gets the last vectors of the shot this player did.
        /// </summary>
        /// <param name="originPosition">Origin position where the shot has been sent from.</param>
        /// <param name="hitPosition">Hit position where the shot landed.</param>
        void GetLastShotVectors(out Vector3 originPosition, out Vector3 hitPosition);
    }
}
