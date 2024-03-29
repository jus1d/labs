#include <iostream>
#include <locale>
#include "./calculate.h"

using namespace std;

int main() {
    setlocale(LC_ALL, "Russian");

    int a, b, c, d;
    cout << "Введите a: " << endl;
    cin >> a;
    cout << "Введите b: " << endl;
    cin >> b;
    cout << "Введите c: " << endl;
    cin >> c;
    do {
        cout << "Введите d != 0: " << endl;
        cin >> d;
    } while (d == 0);
    

    int result = calculate(a, b, c, d);

    printf("Выражение:\n(-2 * a + 6 - 3 * b)/(-2 * c * c - 55 / d)\n"); // = 7, if a = 1, b = -1, c = 0, d = -55 // 1 -1 0 -55
    printf("a = %d, b = %d, c = %d, d = %d\n", a, b, c, d);
    printf("Ожидаемый результат:    %d\n", ((-2 * a) + 6 - (3 * b)) / ((-2 * c * c) - (55 / d)));
    printf("Полученный результат:   %d\n", result);

    return 0;
}
