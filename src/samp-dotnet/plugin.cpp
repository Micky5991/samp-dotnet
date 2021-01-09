#include <iostream>
#include <sstream>

#include "sampgdk/sampgdk.h"
#include "samp-dotnet/samp-dotnet.h"
#include "samp-dotnet/clr_manager.h"
#include "samp-dotnet/event_manager.h"

extern void *pAMXFunctions;

ClrManager* clr_manager = nullptr;
EventManager* event_manager = nullptr;

bool started = false;

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
    event_manager = new EventManager();

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
