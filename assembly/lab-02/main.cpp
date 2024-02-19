#include <iostream>
#include <locale>
#include "./calculate.h"

using namespace std;

int calculate_cpp(int a, int b)
{
    if (a > b) 
    {
        return (b + 1) / a + 1;
    }
    else if (a < b) 
    {
        return (a - 5) / (a + b);
    }
    else 
    {
        return -b;
    }
}

int main() {
    setlocale(LC_ALL, "Russian");

    printf("Выражение:\n(b + 1)/a + 1,   a > b\n-b,              a = b\n(a - 5)/(a + b), a < b\n\n");

    int a, b, c, d;

    // do {
    //     cout << "Введите a: " << endl;
    //     cin >> a;
    //     cout << "Введите b: " << endl;
    //     cin >> b;
    // } while (a == -b);
    
    cout << "Введите a: " << endl;
    cin >> a;
    cout << "Введите b: " << endl;
    cin >> b;

    printf("a = %d, b = %d\n", a, b);
    printf("Ожидаемый результат:    %d\n", calculate_cpp(a, b));
    printf("Полученный результат:   %d\n", calculate(a, b));

    return 0;
}
