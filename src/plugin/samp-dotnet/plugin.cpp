#include <iostream>

#include "sampgdk/interop.h"
#include "sampgdk/core.h"
#include "samp-dotnet/samp-dotnet.h"
#include "configreader/configreader.h"

extern void *pAMXFunctions;

sampdotnet::SampNet* samp_net = nullptr;

std::string get_config_gamemode_path() {
    ConfigReader c;
    c.ReadFromFile("server.cfg");

    std::string gamemode_path;
    c.GetValue("dotnet_gamemode", gamemode_path);

    return gamemode_path;
}

PLUGIN_EXPORT unsigned int PLUGIN_CALL Supports() {
    return sampgdk::Supports() | SUPPORTS_AMX_NATIVES | SUPPORTS_PROCESS_TICK;
}

PLUGIN_EXPORT bool PLUGIN_CALL Load(void **ppData) {
    std::cout << "[SAMP.Net] Initializing SAMP.Net V1.0.0" << std::endl;
    std::cout << "[SAMP.Net] Latest .NET Runtime Version: " << SAMPDOTNET_RUNTIME_VERSION << std::endl;

    samp_net = new sampdotnet::SampNet();

    auto samp_logger = reinterpret_cast<logprintf>(ppData[PLUGIN_DATA_LOGPRINTF]);
    samp_net->set_samp_logger(samp_logger);

    if(samp_net->check_startup(get_config_gamemode_path()) == false) {
        return false;
    }

    sampdotnet::SampNet::printf("[SAMP.Net] Loading sampgdk");

    pAMXFunctions = ppData[PLUGIN_DATA_AMX_EXPORTS];
    bool loaded = sampgdk::Load(ppData);
    if(loaded == false) {
        std::cerr << "[SAMP.Net] Loading SAMPGDK failed" << std::endl;

        return false;
    }

    sampdotnet::SampNet::printf("[SAMP.Net] SAMP.Net has been initialized");

    return true;
}

PLUGIN_EXPORT int PLUGIN_CALL AmxLoad(AMX* amx) {
    if(samp_net == nullptr) {
        return AMX_ERR_NONE;
    }

    sampdotnet::SampNet::printf("[SAMP.Net] Starting gamemode");

    samp_net->start_gamemode();

    return AMX_ERR_NONE;
}

PLUGIN_EXPORT int PLUGIN_CALL AmxUnload(AMX* amx) {
    if(samp_net == nullptr) {
        return AMX_ERR_NONE;
    }

    samp_net->dispose();
    samp_net = nullptr;

    return AMX_ERR_NONE;
}

PLUGIN_EXPORT void PLUGIN_CALL Unload() {
    sampgdk::Unload();
}

PLUGIN_EXPORT void PLUGIN_CALL ProcessTick() {
    if(samp_net == nullptr) {
        return;
    }

    sampgdk::ProcessTick();
    samp_net->execute_tick();
}

PLUGIN_EXPORT void PLUGIN_CALL AttachTickHandler(tick_handler callback) {
    if(samp_net == nullptr) {
        return;
    }

    samp_net->attach_tick_handler(callback);
}

PLUGIN_EXPORT bool PLUGIN_CALL RegisterEvent(const char* event_name, const char* format) {
    if(samp_net == nullptr) {
        return false;
    }

    return samp_net->register_event(std::string(event_name), std::string(format));
}

PLUGIN_EXPORT void PLUGIN_CALL AttachEventHandler(event_handler callback) {
    if(samp_net == nullptr) {
        return;
    }

    samp_net->attach_event_handler(callback);
}

PLUGIN_EXPORT void PLUGIN_CALL AttachLoggerHandler(log_handler callback) {
    if(samp_net == nullptr) {
        return;
    }

    samp_net->install_logger_hook();
    samp_net->attach_logger(callback);

    sampdotnet::SampNet::printf("Log-redirection has been enabled!");
}

PLUGIN_EXPORT void PLUGIN_CALL LogMessage(const char* message) {
    if(samp_net == nullptr) {
        return;
    }

    samp_net->print_samp(message);
}

PLUGIN_EXPORT cell PLUGIN_CALL InvokeNative(const char* native_name, const char* format, void** native_args) {
    if(samp_net == nullptr) {
        return 0;
    }

    AMX_NATIVE native = sampgdk::FindNative(native_name);
    if(native == nullptr) {
        sampdotnet::SampNet::printf("[SAMP.Net] Unable to find native \"%s\"", native_name);

        return 0;
    }

    return sampgdk::InvokeNativeArray(native, format, native_args);
}

PLUGIN_EXPORT bool PLUGIN_CALL OnPublicCall(AMX* amx, const char* name, cell* params, cell* retval) {
    if(samp_net == nullptr) {
        return false;
    }

    return samp_net->handle_public_call(amx, name, params, retval);
}
