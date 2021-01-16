# SAMP.Net

Use C# and .NET to create your own SA:MP gamemode.

Kartoffel 1234

## Getting started

> **DISCLAIMER**: Due to missing support by Microsoft there is currently no support for .NET on linux x86!
> This plugin will still be published for linux, but there is currently no .NET 5.0 runtime for linux x86.

### Installation

#### sampctl

`sampctl package install Micky5991/samp-dotnet`

#### Manual installation

1. Download the [latest zip release of SAMP.Net](https://github.com/Micky5991/samp-dotnet/releases/latest) for your platform.
2. Unpack the content of the plugin to your SA:MP server.
3. Install the .NET runtime of your choice. *(Will be provided on windows)*
3. Change the [configuration of the `server.cfg`](#configuration).
4. Start developing your first gamemode.

### Configuration

SAMP.Net provides some configurations in the `server.cfg` to change the behavior of this plugin.

| Key             | Type   | Default | Description                                                                                        |
|-----------------|--------|---------|----------------------------------------------------------------------------------------------------|
| dotnet_gamemode | string | *empty* | Specifies where your main DLL of your gamemode is, **relative to the `dotnet/gamemodes/` folder**. |

#### Example

In this example we specified the path of the main dll. This will look for your gamemode in `dotnet/gamemodes/net5.0/Micky5991.Samp.Net.Example.dll`.

```
echo Executing Server Config...
lanmode 0
rcon_password changeme
maxplayers 50
port 7777
hostname SA-MP 0.3 Server
gamemode0 grandlarc 1
filterscripts 
announce 0
chatlogging 0
weburl www.sa-mp.com
onfoot_rate 40
incar_rate 40
weapon_rate 40
stream_distance 300.0
stream_rate 1000
maxnpc 0
logtimeformat [%H:%M:%S]
language English

dotnet_gamemode net5.0/Micky5991.Samp.Net.Example.dll
```

### Your first gamemode

To develop your first SAMP.Net gamemode, you must fulfill these requirements:

- Knowledge in C#
- [.NET SDK](https://dotnet.microsoft.com/download/dotnet/5.0) installed on your system.
- IDE / Text editor to develop and test on. (Visual Studio Code / Visual Studio / JetBrains Rider)
- Recommended: [sampctl](https://github.com/Southclaws/sampctl)

#### NuGet Packages

| Package                       | Description                                                                                                                                    |
|-------------------------------|------------------------------------------------------------------------------------------------------------------------------------------------|
| [Micky5991.Samp.Net.Framework](https://www.nuget.org/packages/Micky5991.Samp.Net.Framework/)  | .NET Standard 2.0 / 2.1 library that provides types and tools for your first gamemode. You only need this package to start creating your first gamemode. |
| [Micky5991.Samp.Net.Core](https://www.nuget.org/packages/Micky5991.Samp.Net.Core/)       | Provides core functionality to other parts of SAMP.Net.                                                                                        |
| [Micky5991.Samp.Net.Generators](https://www.nuget.org/packages/Micky5991.Samp.Net.Generators/) | Generates APIs for natives, events and constants by reading sampgdk `.idl` files using C# source generators                                    |



## Special thanks to

- Zeex for [sampgdk](https://github.com/Zeex/sampgdk), [subhook](https://github.com/Zeex/subhook), [configreader](https://github.com/Zeex/configreader)
- SA:MP team for [San Andreas Multiplayer](https://www.sa-mp.com/)

## License

SAMP.Net is licensed under the [MIT](/LICENSE) license.
