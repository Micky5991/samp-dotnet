using System;
using Micky5991.Samp.Net.Core;
using Micky5991.Samp.Net.Core.Interop;
using Micky5991.Samp.Net.Core.Interop.Events;
using Micky5991.Samp.Net.Core.Natives;

namespace Micky5991.Samp.Net.Example
{
    public class Program
    {
        public static NativeTypeConverter typeConverter = new NativeTypeConverter();

        public static void Main(string[] args)
        {
            try
            {
                var eventRegistry = new NativeEventRegistry();

                var sampEvents = new SampEventCollection(eventRegistry);
                sampEvents.RegisterEvents();

                IVehiclesNatives vehiclesNatives = new VehiclesNatives(typeConverter);
                var sampNatives = new SampNatives(typeConverter);

                var vehicle = vehiclesNatives.CreateVehicle(411, 0f, 0f, 0f, 268.8108f, 0, 0, 0, true);
                Console.WriteLine($"Created vehicle: {vehicle}");

                var result = vehiclesNatives.GetVehicleZAngle(vehicle, out var zAngle);
                Console.WriteLine($"ZANGLE ({result}): {vehicle} -> {zAngle}");

                vehiclesNatives.SetVehicleNumberPlate(vehicle, "LUL");

                sampNatives.SetGameModeText("MPlaying Test");

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
