#include <iostream>
#include <string>
#include <filesystem>

#include "samp-dotnet/samp-dotnet.h"
#include "samp-dotnet/clr_manager.h"

ClrManager::ClrManager() {
    coreclr = new CoreClr();
}

ClrManager::~ClrManager() {
    delete coreclr;

    coreclr = nullptr;
}

bool ClrManager::start(const std::string& gamemode) {
    std::filesystem::path gamemodes_path = sampdotnet::get_dotnet_gamemodes_path();
    std::filesystem::path plugins_path = sampdotnet::get_samp_plugins_path();

    std::filesystem::path runtime_path = sampdotnet::get_dotnet_runtime_path();
    std::filesystem::path assembly_path = gamemodes_path;

    assembly_path /= gamemode;
    const std::vector<std::filesystem::path> native_search_paths = {
            plugins_path
    };

    if(coreclr->initialize(runtime_path, gamemodes_path, native_search_paths) == false) {
        return false;
    }

    const char** arguments = nullptr;
    if(coreclr->start(assembly_path, 0, arguments) == false) {
        return false;
    }

    return true;
}