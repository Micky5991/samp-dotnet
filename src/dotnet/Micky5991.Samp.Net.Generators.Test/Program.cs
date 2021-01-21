using System.IO;

namespace Micky5991.Samp.Net.Generators.Test
{
    class Program
    {
        static void Main()
        {
            var builder = new SampNativeBuilder();

            var result = builder.GenerateCode(new []
            {
                Path.GetFullPath("Definitions/natives.a_samp.idl"),
                Path.GetFullPath("Definitions/natives.a_players.idl"),
                Path.GetFullPath("Definitions/natives.a_actor.idl"),
                Path.GetFullPath("Definitions/natives.a_objects.idl"),
                Path.GetFullPath("Definitions/natives.a_vehicles.idl"),

                Path.GetFullPath("Definitions/constants.a_objects.txt"),
                Path.GetFullPath("Definitions/constants.a_samp.txt"),
                Path.GetFullPath("Definitions/constants.a_players.txt"),
                Path.GetFullPath("Definitions/constants.a_vehicles.txt"),
            });

            File.WriteAllText("result.cs", result);
        }
    }
}
