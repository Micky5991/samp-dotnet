using System;

namespace Micky5991.Samp.Net.Framework.Interfaces.Entities.Factories
{
    public interface IMainTimerFactory
    {
        IMainTimer CreateTimer(TimeSpan interval, bool repeating = true);
    }
}