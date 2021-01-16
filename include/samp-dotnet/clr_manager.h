#pragma once

#include "host-coreclr/core_clr.h"

class ClrManager {

    CoreClr* coreclr;

public:
    ClrManager();
    ~ClrManager();

    std::string check_directories();

    bool check_gamemode(const std::string& gamemode, std::string& searched_location);
    bool start(const std::string& gamemode);
};



