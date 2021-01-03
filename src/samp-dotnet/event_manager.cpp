#include <iostream>

#include "samp-dotnet/event_manager.h"
#include "sampgdk.h"

EventManager::EventManager() {
    _registered_events = std::unordered_map<std::string, std::string>();
    _event_handler = nullptr;
}

bool EventManager::register_event(const std::string& event_name, const std::string& format) {
    _registered_events[event_name] = format;

    return true;
}

bool EventManager::attach_event_handler(event_handler event_handler) {
    _event_handler = event_handler;

    return true;
}

bool EventManager::dispatch_event(const std::string& event_name, const CallbackArgument* arguments, int argument_amount) {
    _event_handler(event_name.c_str(), arguments, argument_amount);

    return true;
}

bool EventManager::format_event(AMX* amx, const std::string& event_name, const cell* params, CallbackArgument** arguments, int* argument_amount) {
    auto event = _registered_events.find(event_name);
    if(event == _registered_events.end()) {
        std::cout << "Could not find handler for event " << event_name << std::endl;

        (*arguments) = nullptr;
        (*argument_amount) = 0;

        return false;
    }

    cell argumentAmount = params[0] / sizeof(cell);
    (*argument_amount) = argumentAmount;

    auto result = new CallbackArgument[argumentAmount];

    if(argumentAmount == 0) {
        return true;
    }

    (*arguments) = result;

    std::string format = event->second;

    for (int i = 0; i < format.length(); ++i) {
        auto value = params[i + 1];
        auto argument = result[i];

        argument.type = CallbackArgumentType::None;

        switch (format[i]) {
            case 'i': {
                argument.type = CallbackArgumentType::Integer;
                argument.int_value = (int) value;

                result[i] = argument;

                break;
            }
            case 'f': {
                argument.type = CallbackArgumentType::Float;
                argument.float_value = (float) value;

                result[i] = argument;

                break;
            }

            case 's': {
                cell* address = nullptr;
                int length = 0;

                amx_GetAddr(amx, value, &address);
                if(address != nullptr) {
                    amx_StrLen(address, &length);
                }

                auto text = new char[length];

                amx_GetString(text, address, false, length);

                argument.type = CallbackArgumentType::String;
                argument.string_value = text;

                result[i] = argument;

                break;
            }

            default: {
                std::cerr << "Unable to format argument " << i << " with type " << format[i] << std::endl;

                break;
            }
        }
    }

    return true;
}