using System;
using System.IO;

namespace Micky5991.Samp.Net.Generators.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new SampNativeBuilder();

            var result = builder.GenerateCode(new []
            {
                Path.GetFullPath("natives.a_samp.idl"),
                Path.GetFullPath("natives.a_players.idl"),
            });

            File.WriteAllText("result.cs", result);
        }
    }
}
