#include <iostream>
#include <locale>
#include "./calculate.h"

using namespace std;

int main() {
    setlocale(LC_ALL, "Russian");

    int a, b, c, d;
    cout << "Введите a, b, c, d: " << endl;
    // cin >> a >> b >> c >> d;
    a = 1;
    b = -1;
    c = 0;
    d = 5;

    int result = calculate(a, b, c, d);

    printf("Выражение:\n(-2 * a + 6 - 3 * b)/(-2 * c * c - 55 / d)\n"); // = 7, if a = 1, b = -1, c = 0, d = -55 // 1 -1 0 -55
    printf("a = %d, b = %d, c = %d, d = %d\n", a, b, c, d);
    printf("Ожидаемый результат:    %d\n", ((-2 * a) + 6 - (3 * b)) / ((-2 * c * c) - (55 / d)));
    printf("Полученный результат:   %d\n", result);

    return 0;
}
