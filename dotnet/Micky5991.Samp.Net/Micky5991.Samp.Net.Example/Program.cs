using System;
using System.Runtime.InteropServices;
using Micky5991.Samp.Net.Core.Native;

namespace Micky5991.Samp.Net.Example
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CallNative("CreateVehicle", "iffffiiib", 541, 2036.1937f, 1344.1145f, 10.8203f, 268.8108f, 0, 0, 0, true);
        }

        public static unsafe int CallNative(string native, string format, params object[] arguments)
        {
            try
            {
                var converter = new NativeTypeConverter();
                var location = Marshal.AllocHGlobal(sizeof(IntPtr) * arguments.Length);

                for (int i = 0; i < arguments.Length; i++)
                {
                    var (success, argumentLocation) = converter.ConvertTypeToNative(arguments[i]);
                    if (success == false)
                    {
                        return default;
                    }

                    Marshal.WriteIntPtr(location, i * sizeof(IntPtr), argumentLocation);
                }

                var result = Native.InvokeNative(native, format, location);

                Console.WriteLine($"Result: {result:X}");

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine($"ERROR: {e.Message} {e.StackTrace}");

                return default;
            }
        }
    }
}
