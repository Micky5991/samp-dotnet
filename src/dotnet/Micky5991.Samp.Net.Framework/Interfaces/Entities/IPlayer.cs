using System;
using System.Drawing;
using System.Net;
using System.Numerics;
using System.Security.Claims;
using Micky5991.Samp.Net.Core.Natives.Players;
using Micky5991.Samp.Net.Core.Natives.Samp;
using Micky5991.Samp.Net.Framework.Data;
using Micky5991.Samp.Net.Framework.Interfaces.Permissions;

namespace Micky5991.Samp.Net.Framework.Interfaces.Entities
{
    /// <summary>
    /// Represents a player on the server.
    /// </summary>
    public interface IPlayer : IMovingWorldEntity, IPermissible
    {
        /// <summary>
        /// Gets the principal instance that should be used for authorization.
        /// </summary>
        ClaimsPrincipal Principal { get; }

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
        /// Gets the current animation of this player.
        /// </summary>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        AnimationData? Animation { get; }

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
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        TimeData Time { get; set; }

        /// <summary>
        /// Gets or sets the wanted level of this player.
        /// </summary>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        int WantedLevel { get; set; }

        /// <summary>
        /// Gets or sets the fighting style of this player.
        /// </summary>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        FightStyle FightStyle { get; set; }

        /// <summary>
        /// Gets or sets the special action of this player.
        /// </summary>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        SpecialAction SpecialAction { get; set; }

        /// <summary>
        /// Gets the id of the current vehicle this player is in.
        /// </summary>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        int? VehicleId { get; }

        /// <summary>
        /// Gets the seat this player is current in.
        /// </summary>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        int? VehicleSeat { get; }

        /// <summary>
        /// Gets a value indicating whether the player is currently in a vehicle.
        /// </summary>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        bool IsInAnyVehicle { get; }

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
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        void GetLastShotVectors(out Vector3 originPosition, out Vector3 hitPosition);

        /// <summary>
        /// Sets the specific <paramref name="modelid"/> on the bone <paramref name="bone"/>.
        /// </summary>
        /// <param name="index">Index of entry in player objects.</param>
        /// <param name="modelid">Model of the attached object.</param>
        /// <param name="bone">Player bone to attach the object to.</param>
        /// <param name="offset">Relative offset to the player bone.</param>
        /// <param name="rotation">Rotation of the object.</param>
        /// <param name="scale">3D scale of this object.</param>
        /// <param name="materialColor1">First color of the object material.</param>
        /// <param name="materialColor2">Second color of the object material.</param>
        /// <returns>true if successful, false otherwise.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is too high or too low.</exception>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        bool SetAttachedObject(
            int index,
            int modelid,
            int bone,
            Vector3 offset,
            Vector3 rotation,
            Vector3 scale,
            int materialColor1 = 0,
            int materialColor2 = 0);

        /// <summary>
        /// Removes the object on the specific <paramref name="index"/> from the player.
        /// </summary>
        /// <param name="index">Index of entry in player objects.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is too high or too low.</exception>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        void RemoveAttachedObject(int index);

        /// <summary>
        /// Returns if the specified <paramref name="index"/> is used.
        /// </summary>
        /// <param name="index">Index to check for existance.</param>
        /// <returns>true if the slot is used, false otherwise.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is too high or too low.</exception>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        bool IsAttachedObjectSlotUsed(int index);

        /// <summary>
        /// Starts the attached object editor for the specified <paramref name="index"/>.
        /// </summary>
        /// <param name="index">Index to edit.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is too high or too low.</exception>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        void EditAttachedObject(int index);

        /// <summary>
        /// Sets the string <paramref name="value"/> with the key <paramref name="varname"/>.
        /// </summary>
        /// <param name="varname">Unique name of this player variable.</param>
        /// <param name="value">Value to set this key to.</param>
        /// <exception cref="ArgumentException"><paramref name="varname"/> is null, empty or too long or <paramref name="value"/> is null.</exception>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        void SetPVar(string varname, string value);

        /// <summary>
        /// Sets the int <paramref name="value"/> with the key <paramref name="varname"/>.
        /// </summary>
        /// <param name="varname">Unique name of this player variable.</param>
        /// <param name="value">Value to set this key to.</param>
        /// <exception cref="ArgumentException"><paramref name="varname"/> is null, empty or too long.</exception>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        void SetPVar(string varname, int value);

        /// <summary>
        /// Sets the int <paramref name="value"/> with the key <paramref name="varname"/>.
        /// </summary>
        /// <param name="varname">Unique name of this player variable.</param>
        /// <param name="value">Value to set this key to.</param>
        /// <exception cref="ArgumentException"><paramref name="varname"/> is null, empty or too long.</exception>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        void SetPVar(string varname, float value);

        /// <summary>
        /// Sets the object <paramref name="value"/> with the key <paramref name="varname"/>.
        /// </summary>
        /// <param name="varname">Unique name of this player variable.</param>
        /// <param name="value">Value to set this key to.</param>
        /// <exception cref="ArgumentException"><paramref name="varname"/> is null, empty or too long or type of <paramref name="value"/> is not supported.</exception>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        void SetPVar(string varname, object value);

        /// <summary>
        /// Gets the saved player value by <paramref name="varname"/>.
        /// </summary>
        /// <param name="varname">Unique name of this player variable.</param>
        /// <typeparam name="T">Type of the variable. Can be int, float or string.</typeparam>
        /// <returns>The actual value, null if not set.</returns>
        /// <exception cref="ArgumentException"><paramref name="varname"/> is null, empty or too long.</exception>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        T? GetPVar<T>(string varname);

        /// <summary>
        /// Deletes the player variable on the specified <paramref name="varname"/>.
        /// </summary>
        /// <param name="varname">Unique name of this player variable.</param>
        /// <exception cref="ArgumentException"><paramref name="varname"/> is null, empty or too long.</exception>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        /// <returns>true on success, false otherwise.</returns>
        bool DeletePVar(string varname);

        /// <summary>
        /// Returns the currently highest set index for this players variables.
        /// </summary>
        /// <returns>Highest currently set index.</returns>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        int GetPVarsUpperIndex();

        /// <summary>
        /// Returns the anem of the player variable at the specified <paramref name="index"/>.
        /// </summary>
        /// <param name="index">Player variable index to get.</param>
        /// <param name="varname">Resulting varname to get.</param>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        void GetPVarNameAtIndex(int index, out string varname);

        /// <summary>
        /// Gets the type of the variable by the name <paramref name="varname"/>.
        /// </summary>
        /// <param name="varname">Unique name of this player variable.</param>
        /// <returns>The stored variable type.</returns>
        /// <exception cref="ArgumentException"><paramref name="varname"/> is null, empty or too long.</exception>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        PlayerVartype GetPVarType(string varname);

        /// <summary>
        /// Sets the chat bubble of the player to <paramref name="text"/>.
        /// </summary>
        /// <param name="text">New value of this chat bubble.</param>
        /// <param name="color">Color to display the chat bubble in.</param>
        /// <param name="drawDistance">Visible distance of this chat bubble.</param>
        /// <param name="expireTime">Time until the chat bubble automatically diappears.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="drawDistance"/> is negative.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="text"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="text"/> is is too long.</exception>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        void SetChatBubble(string text, Color color, float drawDistance, TimeSpan expireTime);

        /// <summary>
        /// Removes the current player from its current vehicle.
        /// </summary>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        void RemoveFromVehicle();

        /// <summary>
        /// Toggles if the player can control their character.
        /// </summary>
        /// <param name="controllable">true if the player can move, false otherwise.</param>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        void ToggleControllable(bool controllable);

        /// <summary>
        /// Plays the sound <paramref name="sound"/> on the position <paramref name="position"/>.
        /// </summary>
        /// <param name="sound">Sound to play.</param>
        /// <param name="position">Location where the sound should be played from.</param>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        void PlaySound(int sound, Vector3 position);

        /// <summary>
        /// Plays the sound <paramref name="sound"/>.
        /// </summary>
        /// <param name="sound">Sound to play.</param>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        void PlaySound(int sound);

        /// <summary>
        /// Plays the <paramref name="animation"/> on the player.
        /// </summary>
        /// <param name="animation">Animation to play.</param>
        /// <param name="delta">Speed of the animation. Usually 4.1.</param>
        /// <param name="loop">true if the animation should be looped, false otherwise.</param>
        /// <param name="lockX">true if the x part of the old position should not be reset after the animation was finished.</param>
        /// <param name="lockY">true if the y part of the old position should not be reset after the animation was finished.</param>
        /// <param name="freeze">true if the player should be frozen after the animation has ended.</param>
        /// <param name="time">Time of the animation.</param>
        /// <param name="forceSync">true if the animation should only be played to other players.</param>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        void ApplyAnimation(
            AnimationData animation,
            float delta,
            bool loop,
            bool lockX,
            bool lockY,
            bool freeze,
            TimeSpan time,
            bool forceSync = false);

        /// <summary>
        /// Clears the current animation of the player.
        /// </summary>
        /// <param name="forceSync">true if the animation should only be cleared to other players.</param>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        void ClearAnimations(bool forceSync = false);

        /// <summary>
        /// Disables all collisions of other vehicles to this player.
        /// </summary>
        /// <param name="disable">true if all vehicles do not have collisions, false otherwise.</param>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        void DisableRemoteVehicleCollisions(bool disable);

        /// <summary>
        /// Shows a dialogue to the player. The dialogid can contain 16bit (+32767), negative ids will close all visible dialogs.
        /// </summary>
        /// <param name="dialogid">Id that will be passed to the dialog response event.</param>
        /// <param name="style">Style of the dialog.</param>
        /// <param name="caption">Title caption of the dialog.</param>
        /// <param name="info">Content of the dialog. Accepts \t for new tab and \n for new line.</param>
        /// <param name="buttonLeft">Text on the left visible button.</param>
        /// <param name="buttonRight">Text on the right button. Leave empty to hide it.</param>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="dialogid"/> is negative.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="caption"/>, <paramref name="info"/>, <paramref name="buttonLeft"/>, <paramref name="buttonRight"/> is null.</exception>
        void ShowDialog(
            int dialogid,
            DialogStyle style,
            string caption,
            string info,
            string buttonLeft,
            string buttonRight = "");

        /// <summary>
        /// Hides all currently visible dialogs.
        /// </summary>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        void HideDialogs();

        /// <summary>
        /// Checks if the current player is logged into rcon.
        /// </summary>
        /// <returns>true if player is rcon admin, false otherwise.</returns>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        bool IsRconAdmin();
    }
}
