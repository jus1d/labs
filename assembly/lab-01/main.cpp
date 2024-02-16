#include "stdaf"
#include <iostream>
#include <locale>

float calculate_assembly(float a, float b, float c, float d) {
    float result = 0;
    __asm{
        mov eax, b
        mov result, eax
    }
    return result;
}

float calculate(float a, float b, float c, float d) {
    return ((-2 * a) + 6 - (3 * b)) / ((-2 * c * c) - (55 / d));
}

int main() {
    setlocale(LC_ALL, "Russian");

    float a, b, c, d;
    std::cout << "Введите a, b, c, d: " << std::endl;
    std::cin >> a >> b >> c >> d;

    float result = calculate(a, b, c, d);

    printf("(-2 * %f + 6 - 3 * %f)/(-2 * %f * %f - 55 / %f) = %f\n", a, b, c, c, d, result);

    return 0;
}
