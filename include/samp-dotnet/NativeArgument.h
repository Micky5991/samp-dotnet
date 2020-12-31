#pragma once

struct NativeArgument {
    union {
        int* int_value;

        bool* bool_value;

        double* double_value;

        const cell** const_cell_value;

        cell** cell_value;

        const char** const_string_value;

        char** string_value;

        const cell** const_array_value;

        cell** array_value;

    };
};
