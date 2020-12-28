#pragma once

#include <filesystem>

#define DOTNET_PATH "dotnet"
#define DOTNET_RUNTIME_PATH "runtime"
#define DOTNET_GAMEMODES_PATH "gamemodes"

#define SAMP_PLUGINS_PATH "plugins"
#define SAMP_GAMEMODES_PATH "gamemodes"

namespace sampdotnet {
    std::filesystem::path get_dotnet_path();
    std::filesystem::path get_dotnet_runtime_path();
    std::filesystem::path get_dotnet_gamemodes_path();
    std::filesystem::path get_samp_plugins_path();
    std::filesystem::path get_samp_gamemodes_path();
}

