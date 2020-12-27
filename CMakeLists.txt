cmake_minimum_required(VERSION 3.10)
project(mplaying-cpp)

include_directories(
        ${CMAKE_CURRENT_SOURCE_DIR}/include
        ${CMAKE_CURRENT_SOURCE_DIR}/include/sampgdk
        ${CMAKE_CURRENT_SOURCE_DIR}/thirdparty/samp-plugin-sdk
        ${CMAKE_CURRENT_SOURCE_DIR}/thirdparty/host-coreclr/include
)

set(SAMP_SDK_ROOT ${CMAKE_CURRENT_SOURCE_DIR}/thirdparty/samp-plugin-sdk)
set(SAMPSDK_DIR ${SAMP_SDK_ROOT})

list(APPEND CMAKE_MODULE_PATH ${CMAKE_CURRENT_SOURCE_DIR}/cmake)
list(APPEND CMAKE_MODULE_PATH ${CMAKE_CURRENT_SOURCE_DIR}/cmake/cmake-modules)

add_subdirectory(src)
add_subdirectory(thirdparty/host-coreclr)