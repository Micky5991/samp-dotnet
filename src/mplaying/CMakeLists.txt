cmake_minimum_required(VERSION 3.10)
project(mplaying
        VERSION 1.0.0
        LANGUAGES CXX)

include(AMXConfig)
include(AddSAMPPlugin)

set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED ON)

add_definitions(-DSAMPGDK_AMALGAMATION)
add_definitions(-DSAMPGDK_CPP_WRAPPERS)

add_samp_plugin(mplaying
        mplaying.cpp
        mplaying.def
        sampgdk.c
        ${SAMPSDK_DIR}/amxplugin.cpp
        )

target_compile_features(mplaying PUBLIC cxx_std_17)

target_link_libraries(mplaying PRIVATE coreclr)

if(WIN32)
install(FILES ${CMAKE_BINARY_DIR}/src/mplaying/mplaying.dll DESTINATION "${CMAKE_SOURCE_DIR}/samp/plugins")
install(DIRECTORY ${CMAKE_SOURCE_DIR}/runtime/ DESTINATION "${CMAKE_SOURCE_DIR}/samp/dotnet/runtime/")
endif()