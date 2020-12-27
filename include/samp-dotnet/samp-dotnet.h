#pragma once

#include <filesystem>

#define DOTNET_PATH "dotnet/"

namespace sampdotnet {
    std::filesystem::path get_base_path();
}

