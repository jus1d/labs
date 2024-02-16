#include <iostream>
#include <locale>
#include "./calculate.h"

int main() {
    setlocale(LC_ALL, "Russian");

    int a, b, c, d;
    std::cout << "Введите a, b, c, d: " << std::endl;
    std::cin >> a >> b >> c >> d;

    int result = calculate(a, b, c, d);

    // a = 1, b = 1, c = 
    // (1)/(-2 * c * c - 55 / d)

    printf("Выражение:\n(-2 * a + 6 - 3 * b)/(-2 * c * c - 55 / d)\n");
    printf("Ожидаемый результат:    %d\n", ((-2 * a) + 6 - (3 * b)) / ((-2 * c * c) - (55 / d)));
    printf("Полученный результат:   %d\n", result);

    return 0;
}
