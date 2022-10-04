#include <string>
#include <cstdarg>
#include <iostream>

#include "subhook/subhook.h"
#include "samp-dotnet/samp-dotnet.h"

namespace sampdotnet {

    SampNet::SampNet() {
        _instance = this;

        _tick_handler = nullptr;
        _log_handler = nullptr;
        _samp_logger = nullptr;

        _clr_manager = new ClrManager();
        _event_manager = new EventManager();

        _logprintf_hook = new subhook::Hook();
    }

    SampNet* SampNet::instance() {
        return _instance;
    }

    void SampNet::attach_tick_handler(tick_handler callback) {
        _tick_handler = callback;
    }

    void SampNet::attach_logger(log_handler callback) {
        _log_handler = callback;
    }

    bool SampNet::register_event(const std::string& name, const std::string& format) {
        return _event_manager->register_event(name, format);
    }

    bool SampNet::attach_event_handler(event_handler event_handler) {
        return _event_manager->attach_event_handler(event_handler);
    }

    bool SampNet::handle_public_call(AMX* amx, const char* name, cell* params, cell* retval) {
        CallbackArgument* arguments = nullptr;
        int argument_amount = 0;

        auto success = _event_manager->format_event(amx, name, params, &arguments, &argument_amount);
        if(success == false) {
            return true;
        }

        EventInvokeResult result = _event_manager->dispatch_event(name, arguments, argument_amount);

        for (int i = 0; i < argument_amount; ++i) {
            arguments[i].dispose();
        }

        delete arguments;

        if(retval != nullptr) {
            *retval = (cell) result.return_value;
        }

        return result.allow_execute;
    }

    void SampNet::execute_tick() {
        if(_tick_handler == nullptr) {
            return;
        }

        _tick_handler();
    }

    void SampNet::set_samp_logger(logprintf samp_logger) {
        _samp_logger = samp_logger;
    }

    void SampNet::install_logger_hook() {
        _logprintf_hook->Install((void*) _samp_logger, (void*) SampNet::printf);

        _samp_logger = nullptr;
    }

    void SampNet::print_samp(const char* format) {
        if (_samp_logger != nullptr) {
            _samp_logger(format);

            return;
        }

        ((logprintf) _logprintf_hook->GetTrampoline())(format);
    }

    void SampNet::print_handler(const char* message) {
        _log_handler(message);
    }

    bool SampNet::check_startup(const std::string& gamemode_path) {
        std::string missing_folder = _clr_manager->check_directories();
        if(missing_folder.empty() == false) {
            printf("[SAMP.Net] The directory \"%s\" is missing! Abort.", missing_folder.c_str());

            return false;
        }

        std::string searched_location;
        if(_clr_manager->check_gamemode(gamemode_path, searched_location) == false) {
            printf("[SAMP.Net] Unable to find gamemode in \"%s\".", searched_location.c_str());

            return false;
        }

        _gamemode_path = gamemode_path;

        return true;
    }

    bool SampNet::start_gamemode() {
        return _clr_manager->start(_gamemode_path);
    }

    void SampNet::print(const char* message) {
        if(_samp_logger != nullptr) {
            _samp_logger(message);

            return;
        }

        if(_log_handler != nullptr) {
            _log_handler(message);

            return;
        }

        print_samp(message);
    }

    void SampNet::printf(const char* format, ...) {
        char* buffer = new char[LOGPRINTF_BUFFER_SIZE];

        va_list va;
                va_start(va, format);
        vsnprintf(buffer, LOGPRINTF_BUFFER_SIZE, format, va);
                va_end(va);

        SampNet::instance()->print(buffer);

        delete[] buffer;
    }

    void SampNet::dispose() {
        delete _clr_manager;
        delete _event_manager;
        delete _logprintf_hook;
    }

    std::filesystem::path get_absolute_path(const std::string& addition,
                                            std::filesystem::path base_path = std::filesystem::current_path()) {
        base_path /= addition;

        return base_path;
    }

    std::filesystem::path get_dotnet_path() {
        return get_absolute_path(DOTNET_PATH);
    }

    std::filesystem::path get_dotnet_runtime_path() {
        return get_absolute_path(DOTNET_RUNTIME_PATH, get_dotnet_path());
    }

    std::filesystem::path get_dotnet_gamemodes_path() {
        return get_absolute_path(DOTNET_GAMEMODES_PATH, get_dotnet_path());
    }

    std::filesystem::path get_samp_plugins_path() {
        return get_absolute_path(SAMP_PLUGINS_PATH);
    }
}

sampdotnet::SampNet* sampdotnet::SampNet::_instance = nullptr;
