using System;

namespace Micky5991.Samp.Net.Core
{
    public class Program
    {
        public static void test()
        {
            SampNatives.Samp.GetNetworkStats(out var ret, 2);
        }
    }
}
