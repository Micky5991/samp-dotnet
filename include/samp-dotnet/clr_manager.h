#pragma once

#include "host-coreclr/core_clr.h"

class ClrManager {

    CoreClr* coreclr;

public:
    ClrManager();
    ~ClrManager();

    std::string check_directories();
    bool start(const std::string& gamemode);
};



