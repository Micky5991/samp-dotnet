#include <iostream>
#include <sstream>

#include "sampgdk/sampgdk.h"
#include "samp-dotnet/clr_manager.h"
#include "samp-dotnet/NativeArgument.h"

extern void *pAMXFunctions;

ClrManager* clr_manager = nullptr;
bool started = false;

PLUGIN_EXPORT bool PLUGIN_CALL OnGameModeInit() {
    sampgdk::AddPlayerClass(0, 1958.3783f, 1343.1572f, 15.3746f, 269.1425f,
                            0, 0, 0, 0, 0, 0);

    return true;
}

PLUGIN_EXPORT unsigned int PLUGIN_CALL Supports() {
    return sampgdk::Supports() | SUPPORTS_PROCESS_TICK | SUPPORTS_AMX_NATIVES;
}

PLUGIN_EXPORT bool PLUGIN_CALL Load(void **ppData) {
    pAMXFunctions = ppData[PLUGIN_DATA_AMX_EXPORTS];

    bool loaded = sampgdk::Load(ppData);
    if(loaded == false) {
        return false;
    }

    clr_manager = new ClrManager();

    return loaded;
}

PLUGIN_EXPORT int PLUGIN_CALL AmxLoad(AMX* amx) {
    std::cout << "AMX LOAD" << std::endl;
    started = true;

    clr_manager->start("net5.0/Micky5991.Samp.Net.Example.dll");

    return AMX_ERR_NONE;
}

PLUGIN_EXPORT int PLUGIN_CALL AmxUnload(AMX* amx) {
    std::cout << "AMX UNLOAD" << std::endl;
    started = false;

    return AMX_ERR_NONE;
}

PLUGIN_EXPORT void PLUGIN_CALL Unload() {
    sampgdk::Unload();
}

PLUGIN_EXPORT void PLUGIN_CALL ProcessTick() {
    sampgdk::ProcessTick();
}

PLUGIN_EXPORT cell PLUGIN_CALL InvokeNative(const char* native_name, const char* format, void** native_args) {
    AMX_NATIVE native = sampgdk::FindNative(native_name);
    if(native == nullptr) {
        std::cerr << "Unable to find native " << native_name << std::endl;

        return 0;
    }

    return sampgdk::InvokeNativeArray(native, format, native_args);
}

PLUGIN_EXPORT bool PLUGIN_CALL OnPublicCall(AMX* amx, const char* name, cell* params, cell* retval) {
    if(started == false) {
        std::cerr << "OnPublicCall before started!" << amx << std::endl;

        return true;
    }

    if(std::string(name) == "OnPlayerUpdate") {
//        std::cout << "--- Ignored public call " << name << std::endl;

        return true;
    }

    auto parameter_amount = params[0] / sizeof(cell);

    std::stringstream sstream;
    sstream << name << "[" << parameter_amount << "](";

    for (int i = 1; i <= parameter_amount; ++i) {
        sstream << params[i];

        sstream << ", ";
    }

    sstream << ") -> " << (*retval);

    if(std::string(name) == "OnPlayerText") {
        unsigned char text[128];
        cell* location;
        amx_GetAddr(amx, params[2], &location);

        amx_GetString((char*)text, location, false, sizeof(text));

        std::cout << "STRINGä: " << text << std::endl;

        for(auto value : text) {
            std::cout << (int) value << " ";
        }

        std::cout << std::endl;

    }

    std::cout << sstream.str() << std::endl;
    return true;
}
