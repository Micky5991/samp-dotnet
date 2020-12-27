#pragma once

#include "host-coreclr/core_clr.h"

class ClrManager {

    CoreClr* coreclr;

public:
    ClrManager();
    ~ClrManager();

    bool start(const std::string& gamemode);
};



