using System;
using Micky5991.Samp.Net.Core;
using Micky5991.Samp.Net.Core.Interop;

namespace Micky5991.Samp.Net.Example
{
    public class Program
    {
        public static NativeTypeConverter typeConverter = new NativeTypeConverter();

        public static void Main(string[] args)
        {
            try
            {
                var vehicle = SampNatives.Vehicles.CreateVehicle(411, 2036.1937f, 1344.1145f, 10.8203f, 268.8108f, 0, 0, 0, true);
                Console.WriteLine($"Created vehicle: {vehicle}");

                var result = SampNatives.Vehicles.GetVehicleZAngle(vehicle, out var zAngle);
                Console.WriteLine($"ZANGLE ({result}): {vehicle} -> {zAngle}");

                SampNatives.Vehicles.SetVehicleNumberPlate(vehicle, "LUL");

                SampNatives.Samp.SetGameModeText("MPlaying Test");

                // CallNative("CreateVehicle", "iffffiiib", 541, 2036.1937f, 1344.1145f, 10.8203f, 268.8108f, 0, 0, 0, true);
            }
            catch (Exception e)
            {
                Console.WriteLine($"ERROR: {e.Message}");
                Console.WriteLine(e.StackTrace);
            }
        }
    }
}
