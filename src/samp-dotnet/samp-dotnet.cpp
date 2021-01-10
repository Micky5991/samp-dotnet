#include <string>
#include <iostream>

#include "sampgdk/sampgdk.h"
#include "subhook/subhook.h"
#include "samp-dotnet/samp-dotnet.h"

namespace sampdotnet {

    tick_handler _tick_handler = nullptr;
    log_handler _log_handler = nullptr;

    subhook::Hook logprintf_hook;

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

    std::filesystem::path get_samp_gamemodes_path() {
        return get_absolute_path(SAMP_GAMEMODES_PATH);
    }

    void attach_tick_handler(tick_handler callback) {
        _tick_handler = callback;
    }

    void attach_logger(log_handler callback) {
        _log_handler = callback;
    }

    void execute_tick() {
        if(_tick_handler == nullptr) {
            return;
        }

        _tick_handler();
    }

    void hook_logger(logprintf samp_logger) {
        logprintf_hook.Install((void*) samp_logger, (void*) printf);
    }

    void printf(const char* format, ...) {
        char* buffer = new char[LOGPRINTF_BUFFER_SIZE];

        va_list va;
        va_start(va, format);
        vsnprintf(buffer, LOGPRINTF_BUFFER_SIZE, format, va);
        va_end(va);

        if(_log_handler != nullptr) {
            _log_handler(buffer);
        } else {
            print_samp(buffer);
        }

        delete[] buffer;
    }

    void print_samp(const char* format) {
        ((logprintf) logprintf_hook.GetTrampoline())(format);
    }
}