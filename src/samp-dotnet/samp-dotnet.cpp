#include <string>
#include <iostream>
#include <cstdio>

#include "sampgdk/sampgdk.h"
#include "samp-dotnet/samp-dotnet.h"
#include "samp-dotnet/clr_manager.h"

ClrManager* clr_manager = nullptr;

PLUGIN_EXPORT bool PLUGIN_CALL OnGameModeInit() {
    sampgdk::AddPlayerClass(0, 1958.3783f, 1343.1572f, 15.3746f, 269.1425f,
                   0, 0, 0, 0, 0, 0);

    AMX_NATIVE create_vehicle = sampgdk_FindNative("CreateVehicle");

    int vehicle_type = 541;
    float x = 2036.1937f;
    float y = 1344.1145f;
    float z = 10.8203f;
    float rot = 268.8108f;

    int color1 = 0;
    int color2 = 0;
    int respawn_delay = 0;
    int add_siren = 1;

    void* arguments[] = {
        &vehicle_type,
        &x,
        &y,
        &z,
        &rot,
        &color1,
        &color2,
        &respawn_delay,
        &add_siren,
    };

    sampgdk_InvokeNativeArray(create_vehicle, "iffffiiib", arguments);

    return true;
}

PLUGIN_EXPORT unsigned int PLUGIN_CALL Supports() {
    return sampgdk::Supports() | SUPPORTS_PROCESS_TICK;
}

PLUGIN_EXPORT bool PLUGIN_CALL Load(void **ppData) {
    bool loaded = sampgdk::Load(ppData);
    if(loaded == false) {
        return false;
    }

    clr_manager = new ClrManager();
    clr_manager->start("");

    return loaded;
}

PLUGIN_EXPORT void PLUGIN_CALL Unload() {
    sampgdk::Unload();
}

PLUGIN_EXPORT void PLUGIN_CALL ProcessTick() {
    sampgdk::ProcessTick();
}

namespace sampdotnet {

    std::filesystem::path get_base_path() {
        std::filesystem::path base_path = std::filesystem::current_path();

        base_path /= DOTNET_PATH;

        return base_path;
    }

}