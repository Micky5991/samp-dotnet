#include <iostream>
#include <sstream>

#include "sampgdk/interop.h"
#include "sampgdk/sdk.h"
#include "sampgdk/core.h"
#include "samp-dotnet/samp-dotnet.h"
#include "samp-dotnet/clr_manager.h"
#include "samp-dotnet/event_manager.h"

ClrManager* clr_manager = nullptr;
EventManager* event_manager = nullptr;

bool started = false;

PLUGIN_EXPORT unsigned int PLUGIN_CALL Supports() {
    return sampgdk::Supports() | SUPPORTS_PROCESS_TICK | SUPPORTS_AMX_NATIVES;
}

PLUGIN_EXPORT bool PLUGIN_CALL Load(void **ppData) {
    std::cout << "[SAMP.Net] Initializing SAMP.Net V1.0.0" << std::endl;

    bool loaded = sampgdk::Load(ppData);
    if(loaded == false) {
        std::cerr << "[SAMP.Net] Loading SAMPGDK failed" << std::endl;

        return false;
    }

    auto samp_logger = reinterpret_cast<logprintf>(ppData[PLUGIN_DATA_LOGPRINTF]);
    sampdotnet::hook_logger(samp_logger);

    std::cout << "[SAMP.Net] SA:MP logger has been hooked" << std::endl;

    clr_manager = new ClrManager();
    event_manager = new EventManager();

    std::cout << "[SAMP.Net] Components initialized" << std::endl;

    std::string missing_folder = clr_manager->check_directories();
    if(missing_folder.empty() == false) {
        std::cout << "[SAMP.Net] The directory \"" << missing_folder << "\" is missing! Abort." << std::endl;

        return false;
    }

    std::cout << "[SAMP.Net] SAMP.Net has been initialized" << std::endl;

    started = true;

    return true;
}

PLUGIN_EXPORT int PLUGIN_CALL AmxLoad(AMX* amx) {
    if(started == false) {
        return AMX_ERR_NONE;
    }

    std::cout << "[SAMP.Net] AMX loaded, starting .NET CoreCLR" << std::endl;

    clr_manager->start("net5.0/Micky5991.Samp.Net.Example.dll");

    std::cout << "[SAMP.Net] .NET CoreCLR has been started" << std::endl;

    return AMX_ERR_NONE;
}

PLUGIN_EXPORT int PLUGIN_CALL AmxUnload(AMX* amx) {
    delete clr_manager;
    delete event_manager;

    clr_manager = nullptr;
    event_manager = nullptr;

    return AMX_ERR_NONE;
}

PLUGIN_EXPORT void PLUGIN_CALL Unload() {
    sampgdk::Unload();
}

PLUGIN_EXPORT void PLUGIN_CALL ProcessTick() {
    if(started == false) {
        return;
    }

    sampgdk::ProcessTick();
    sampdotnet::execute_tick();
}

PLUGIN_EXPORT void PLUGIN_CALL AttachTickHandler(tick_handler callback) {
    sampdotnet::attach_tick_handler(callback);
}

PLUGIN_EXPORT bool PLUGIN_CALL RegisterEvent(const char* event_name, const char* format) {
    return event_manager->register_event(std::string(event_name), std::string(format));
}

PLUGIN_EXPORT void PLUGIN_CALL AttachEventHandler(event_handler callback) {
    event_manager->attach_event_handler(callback);
}

PLUGIN_EXPORT void PLUGIN_CALL AttachLoggerHandler(log_handler callback) {
    sampdotnet::attach_logger(callback);

    sampdotnet::print_samp("[SAMP.Net]  Log-redirection has been enabled!");
}

PLUGIN_EXPORT void PLUGIN_CALL LogMessage(const char* message) {
    sampdotnet::print_samp(message);
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
        return false;
    }

    CallbackArgument* arguments = nullptr;
    int argument_amount = 0;

    auto success = event_manager->format_event(amx, name, params, &arguments, &argument_amount);
    if(success == false) {
        return true;
    }

    EventInvokeResult result = event_manager->dispatch_event(name, arguments, argument_amount);

    for (int i = 0; i < argument_amount; ++i) {
        arguments[i].dispose();
    }
    delete arguments;

    if(retval != nullptr) {
        *retval = (cell) result.return_value;
    }

    return result.allow_execute;
}
