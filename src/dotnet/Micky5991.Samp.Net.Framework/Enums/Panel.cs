using System;

namespace Micky5991.Samp.Net.Framework.Enums
{
    /// <summary>
    /// List of all available panel locations and their nibble (4-bit) position.
    /// </summary>
    [Flags]
    public enum Panel
    {
        /// <summary>
        /// No specific location.
        /// </summary>
        None = -1,

        /// <summary>
        /// Front left panel on a car and left engine for a plane.
        /// </summary>
        FrontLeft = 0,

        /// <summary>
        /// Front right panel on a car and right engine for a plane.
        /// </summary>
        FrontRight = 1,

        /// <summary>
        /// Back left panel on a car and rudder on a plane.
        /// </summary>
        BackLeft = 2,

        /// <summary>
        /// Back right panel on a car and elevators for a plane.
        /// </summary>
        BackRight = 3,

        /// <summary>
        /// Windshield for a car and ailerons for a plane.
        /// </summary>
        WindShield = 4,

        /// <summary>
        /// Front bumper for a car.
        /// </summary>
        FrontBumper = 5,

        /// <summary>
        /// Back bumper for a car.
        /// </summary>
        BackBumper = 6,
    }
}
