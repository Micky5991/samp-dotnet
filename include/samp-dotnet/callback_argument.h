#pragma once

#include "sampgdk.h"

enum CallbackArgumentType : int {
    None = 0,
    Integer,
    Bool,
    Float,
    Cell,
    String,
    Array
};

#pragma pack(push, 1)
struct CallbackArgument {
    CallbackArgumentType type;

    union {
        int int_value;

        bool bool_value;

        float float_value;

        cell* cell_value;

        char* string_value;
    };
};
#pragma pack(pop)
