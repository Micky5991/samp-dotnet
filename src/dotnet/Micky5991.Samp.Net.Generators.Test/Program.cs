using System.IO;

namespace Micky5991.Samp.Net.Generators.Test
{
    class Program
    {
        static void Main()
        {
            var builder = new SampNativeBuilder();

            var filePaths = new[]
            {
                Path.GetFullPath("Definitions/natives.a_samp.idl"),
                Path.GetFullPath("Definitions/natives.a_players.idl"),
                Path.GetFullPath("Definitions/natives.a_actor.idl"),
                Path.GetFullPath("Definitions/natives.a_objects.idl"),
                Path.GetFullPath("Definitions/natives.a_vehicles.idl"),
            };

            foreach (var filePath in filePaths)
            {
                var result = builder.GenerateCode(filePath, out var idlNamespace);

                File.WriteAllText($"result_{idlNamespace.Name}.cs", result);
            }
        }
    }
}
