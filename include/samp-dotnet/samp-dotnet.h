#pragma once

#include <filesystem>
#include "samp-dotnet/callback_argument.h"

#define DOTNET_PATH "dotnet"
#define DOTNET_RUNTIME_PATH "runtime"
#define DOTNET_GAMEMODES_PATH "gamemodes"

#define SAMP_PLUGINS_PATH "plugins"
#define SAMP_GAMEMODES_PATH "gamemodes"

#define LOGPRINTF_BUFFER_SIZE 1024 // SEE SAMPGDK_LOGPRINTF_BUFFER_SIZE

typedef void (PLUGIN_CALL *tick_handler)();
typedef void (*logprintf)(const char* message, ...);
typedef void (PLUGIN_CALL *log_handler)(const char* message);

namespace sampdotnet {
    std::filesystem::path get_dotnet_path();
    std::filesystem::path get_dotnet_runtime_path();
    std::filesystem::path get_dotnet_gamemodes_path();
    std::filesystem::path get_samp_plugins_path();
    std::filesystem::path get_samp_gamemodes_path();

    void attach_tick_handler(tick_handler callback);
    void execute_tick();
    void printf(const char* format, ...);
    void hook_logger(logprintf samp_logger);
    void attach_logger(log_handler samp_logger);
    void print_samp(const char* message);
}
