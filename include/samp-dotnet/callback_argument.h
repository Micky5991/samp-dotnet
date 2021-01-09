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

    int size;

    union {
        int int_value;

        bool bool_value;

        float float_value;

        cell* cell_value;

        char* string_value;
    };

    CallbackArgument() = default;

    explicit CallbackArgument(int value) : CallbackArgument() {
        type = CallbackArgumentType::Integer;
        size = sizeof(int);

        int_value = value;
    }

    explicit CallbackArgument(bool value) : CallbackArgument() {
        type = CallbackArgumentType::Bool;
        size = sizeof(bool);

        bool_value = value;
    }

    explicit CallbackArgument(float value) : CallbackArgument() {
        type = CallbackArgumentType::Float;
        size = sizeof(float);

        float_value = value;
    }

    explicit CallbackArgument(cell* value) : CallbackArgument() {
        type = CallbackArgumentType::Cell;
        size = sizeof(cell*);

        cell_value = value;
    }

    explicit CallbackArgument(char* value, int value_size) : CallbackArgument() {
        type = CallbackArgumentType::String;
        size = value_size;

        string_value = value;
    }

    void dispose() const {
        if(type == CallbackArgumentType::String) {
            delete[] string_value;
        }
    }
};
#pragma pack(pop)
