#pragma once

#include <filesystem>

#include "subhook/subhook.h"
#include "samp-dotnet/callback_argument.h"
#include "samp-dotnet/event_manager.h"
#include "samp-dotnet/clr_manager.h"

#define DOTNET_PATH "dotnet"
#define DOTNET_RUNTIME_PATH "runtime"
#define DOTNET_GAMEMODES_PATH "gamemodes"

#define SAMP_PLUGINS_PATH "plugins"

#define LOGPRINTF_BUFFER_SIZE 1024 // SEE SAMPGDK_LOGPRINTF_BUFFER_SIZE

typedef void (PLUGIN_CALL *tick_handler)();
typedef void (*logprintf)(const char* message, ...);
typedef void (PLUGIN_CALL *log_handler)(const char* message);

namespace sampdotnet {
    class SampNet {
        ClrManager* _clr_manager;
        EventManager* _event_manager;

        tick_handler _tick_handler;
        log_handler _log_handler;

        subhook::Hook* _logprintf_hook;
        logprintf _samp_logger;

        std::string _gamemode_path;

        void print_handler(const char* message);


    public:

        static SampNet* _instance;

        SampNet();

        void attach_tick_handler(tick_handler callback);
        void execute_tick();
        void set_samp_logger(logprintf samp_logger);
        void install_logger_hook();
        void attach_logger(log_handler samp_logger);

        bool register_event(const std::string& name, const std::string& format);
        bool attach_event_handler(event_handler event_handler);

        void print(const char* message);
        void print_samp(const char* message);

        bool check_startup(const std::string& gamemode_path);
        bool start_gamemode();

        bool handle_public_call(AMX* amx, const char* name, cell* params, cell* retval);

        void dispose();

        static void printf(const char* format, ...);
        static SampNet* instance();
    };

    std::filesystem::path get_dotnet_path();
    std::filesystem::path get_dotnet_runtime_path();
    std::filesystem::path get_dotnet_gamemodes_path();
    std::filesystem::path get_samp_plugins_path();
}

