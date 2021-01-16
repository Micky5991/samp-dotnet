cmake_minimum_required(VERSION 3.10)
project(samp-dotnet)

include_directories(
        ${CMAKE_CURRENT_SOURCE_DIR}/include
        ${CMAKE_CURRENT_SOURCE_DIR}/thirdparty/samp-plugin-sdk
        ${CMAKE_CURRENT_SOURCE_DIR}/thirdparty/samp-plugin-sdk/amx
        ${CMAKE_CURRENT_SOURCE_DIR}/thirdparty/host-coreclr/include
        ${CMAKE_CURRENT_SOURCE_DIR}/thirdparty/sampgdk/include
        ${CMAKE_CURRENT_SOURCE_DIR}/thirdparty
)

if(UNIX)
    # force 32bit compilation and hide non-exported symbols
    set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} -m32 -fvisibility=hidden")
    set(CMAKE_C_FLAGS "${CMAKE_C_FLAGS} -m32 -fvisibility=hidden")
    set_property(GLOBAL PROPERTY FIND_LIBRARY_USE_LIB64_PATHS OFF)
endif()

set(SAMPDOTNET_RUNTIME_VERSION 5.0.2)

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
add_subdirectory(thirdparty/configreader)
add_subdirectory(src)

install(DIRECTORY ${CMAKE_SOURCE_DIR}/runtime/shared/Microsoft.NETCore.App/${SAMPDOTNET_RUNTIME_VERSION}/ DESTINATION "${CMAKE_SOURCE_DIR}/server/dotnet/runtime/")
