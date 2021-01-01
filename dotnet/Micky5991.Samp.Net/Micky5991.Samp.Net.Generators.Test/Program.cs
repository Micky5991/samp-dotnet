﻿using System;
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
                Path.GetFullPath("Definitions/natives.a_samp.idl"),
                Path.GetFullPath("Definitions/natives.a_players.idl"),
                Path.GetFullPath("Definitions/natives.a_actor.idl"),
                Path.GetFullPath("Definitions/natives.a_objects.idl"),
                Path.GetFullPath("Definitions/natives.a_vehicles.idl"),
            });

            File.WriteAllText("result.cs", result);
        }
    }
}
