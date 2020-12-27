#include <string>
#include <iostream>
#include <cstdio>
#include <sampgdk/sampgdk.h>
#include <host-coreclr/core_clr.h>

PLUGIN_EXPORT bool PLUGIN_CALL OnGameModeInit() {
    sampgdk::SetGameModeText("Hello, World!");
    sampgdk::AddPlayerClass(0, 1958.3783f, 1343.1572f, 15.3746f, 269.1425f,
                   0, 0, 0, 0, 0, 0);

//    sampgdk::CreateVehicle(541, 2036.1937f,1344.1145f,10.8203f,268.8108f, 0, 0, 0, true);

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
    return sampgdk::Load(ppData);
}

PLUGIN_EXPORT void PLUGIN_CALL Unload() {
    sampgdk::Unload();
}

PLUGIN_EXPORT void PLUGIN_CALL ProcessTick() {
    sampgdk::ProcessTick();
}