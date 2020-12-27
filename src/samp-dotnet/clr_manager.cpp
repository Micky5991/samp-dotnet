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
    std::filesystem::path gamemodes_path = sampdotnet::get_base_path();
    std::filesystem::path runtime_path = sampdotnet::get_base_path();
    std::filesystem::path assembly_path = gamemodes_path;

    gamemodes_path /= "gamemodes";
    runtime_path /= "runtime";
    assembly_path /= "gamemodes";

    if(coreclr->initialize(runtime_path, gamemodes_path) == false) {
        return false;
    }

    if(coreclr->start(assembly_path, 0, nullptr) == false) {
        return false;
    }

    return true;
}