#include <string>

#include "sampgdk/sampgdk.h"
#include "samp-dotnet/samp-dotnet.h"

namespace sampdotnet {

    std::filesystem::path get_absolute_path(const std::string& addition,
                                            std::filesystem::path base_path = std::filesystem::current_path()) {
        base_path /= addition;

        return base_path;
    }

    std::filesystem::path get_dotnet_path() {
        return get_absolute_path(DOTNET_PATH);
    }

    std::filesystem::path get_dotnet_runtime_path() {
        return get_absolute_path(DOTNET_RUNTIME_PATH, get_dotnet_path());
    }

    std::filesystem::path get_dotnet_gamemodes_path() {
        return get_absolute_path(DOTNET_GAMEMODES_PATH, get_dotnet_path());
    }

    std::filesystem::path get_samp_plugins_path() {
        return get_absolute_path(SAMP_PLUGINS_PATH);
    }

    std::filesystem::path get_samp_gamemodes_path() {
        return get_absolute_path(SAMP_GAMEMODES_PATH);
    }

}