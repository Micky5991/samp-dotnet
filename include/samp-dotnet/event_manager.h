#pragma once

#include <string>
#include <unordered_map>

#include "samp-dotnet/callback_argument.h"
#include "sampgdk/sampgdk.h"

typedef void (PLUGIN_CALL *event_handler)(const char* event_name, const CallbackArgument* arguments, int argument_amount);

class EventManager {
    std::unordered_map<std::string, std::string> _registered_events;
    event_handler _event_handler;

public:
    EventManager();

    bool register_event(const std::string& event_name, const std::string& format);
    bool attach_event_handler(event_handler event_handler);

    bool format_event(AMX* amx, const std::string& event_name, const cell* params, CallbackArgument** arguments, int* argument_amount);
    bool dispatch_event(const std::string& event_name, const CallbackArgument* arguments, int argument_amount);
};
