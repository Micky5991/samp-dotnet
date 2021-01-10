cmake_minimum_required(VERSION 3.10)
project(mplaying-cpp)

include_directories(
        ${CMAKE_CURRENT_SOURCE_DIR}/include
        ${CMAKE_CURRENT_SOURCE_DIR}/thirdparty/samp-plugin-sdk
        ${CMAKE_CURRENT_SOURCE_DIR}/thirdparty/host-coreclr/include
        ${CMAKE_CURRENT_SOURCE_DIR}/thirdparty/sampgdk/include
        ${CMAKE_CURRENT_SOURCE_DIR}/thirdparty
)

set(SAMP_SDK_ROOT ${CMAKE_CURRENT_SOURCE_DIR}/thirdparty/samp-plugin-sdk)
set(CMAKE_INSTALL_PREFIX ${CMAKE_CURRENT_SOURCE_DIR}/tmp/)
set(SAMPSDK_DIR ${SAMP_SDK_ROOT})

set(SUBHOOK_STATIC ON)
set(SUBHOOK_FORCE_32BIT ON)
set(SUBHOOK_TESTS OFF)
set(SUBHOOK_INSTALL OFF)

set(SAMPGDK_STATIC ON)
set(SAMPGDK_BUILD_PLUGINS OFF)
set(SAMPGDK_BUILD_AMALGAMATION OFF)
set(SAMPGDK_BUILD_DOCS OFF)

list(APPEND CMAKE_MODULE_PATH ${CMAKE_CURRENT_SOURCE_DIR}/cmake)
list(APPEND CMAKE_MODULE_PATH ${CMAKE_CURRENT_SOURCE_DIR}/cmake/cmake-modules)

add_subdirectory(thirdparty/host-coreclr)
add_subdirectory(thirdparty/subhook)
add_subdirectory(thirdparty/sampgdk)
add_subdirectory(src)
