namespace Micky5991.Samp.Net.Framework.Data
{
    /// <summary>
    /// Holds information about game hour and minutes.
    /// </summary>
    public struct TimeData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TimeData"/> struct.
        /// </summary>
        /// <param name="hours">Hours to set.</param>
        /// <param name="minutes">Minutes to set.</param>
        public TimeData(int hours, int minutes)
        {
            this.Hours = hours;
            this.Minutes = minutes;
        }

        /// <summary>
        /// Gets the current hours.
        /// </summary>
        public int Hours { get; }

        /// <summary>
        /// Gets the current minutes.
        /// </summary>
        public int Minutes { get; }

        /// <summary>
        /// Deconstructs this data into <see cref="Hours"/> and <see cref="Minutes"/>.
        /// </summary>
        /// <param name="hours">Current weapon model.</param>
        /// <param name="minutes">Current ammo amount.</param>
        public void Deconstruct(out int hours, out int minutes)
        {
            hours = this.Hours;
            minutes = this.Minutes;
        }
    }
}
