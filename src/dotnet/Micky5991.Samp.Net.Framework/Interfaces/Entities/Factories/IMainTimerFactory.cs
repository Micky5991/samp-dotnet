using System;

namespace Micky5991.Samp.Net.Framework.Interfaces.Entities.Factories
{
    /// <summary>
    /// Factory that creates an instance of <see cref="IMainTimer"/>.
    /// </summary>
    public interface IMainTimerFactory
    {
        /// <summary>
        /// Creates instance that implements <see cref="IMainTimer"/>. Timer must be started seperately.
        /// </summary>
        /// <param name="interval">Time between each timer run.</param>
        /// <param name="repeating">Value indicating if the timer should restart after each run.</param>
        /// <returns>Newly created <see cref="IMainTimer"/> instance.</returns>
        IMainTimer CreateTimer(TimeSpan interval, bool repeating = true);
    }
}
