cmake_minimum_required(VERSION 3.10)

include(AMXConfig)
include(AddSAMPPlugin)

set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED ON)

add_definitions(-DSAMPGDK_AMALGAMATION)
add_definitions(-DSAMPGDK_CPP_WRAPPERS)

add_samp_plugin(${PROJECT_NAME}
        plugin.cpp
        samp-dotnet.cpp
        samp-dotnet.def
        clr_manager.cpp
        event_manager.cpp
        ${SAMPSDK_DIR}/amxplugin.cpp
        )

target_compile_features(${PROJECT_NAME} PUBLIC cxx_std_17)

target_link_libraries(${PROJECT_NAME} PRIVATE coreclr subhook sampgdk)

if(WIN32)
    install(FILES ${CMAKE_BINARY_DIR}/src/plugin/samp-dotnet/samp-dotnet.dll DESTINATION "${CMAKE_SOURCE_DIR}/server/plugins")
else()
    install(FILES ${CMAKE_BINARY_DIR}/src/plugin/samp-dotnet/samp-dotnet.so DESTINATION "${CMAKE_SOURCE_DIR}/server/plugins")
endif()
