#include <iostream>
#include <locale>
#include <vector>
#include <cmath>
#include "./calculate.h"

using namespace std;

double calculate_cpp(double a, double b)
{
    return 21 * M_PI * sin(a) + cos(b) * tan(a) - (1.0 / tan(b)) + M_PI * (b * b - a) / (-21 * a + b);
}

int main() {
    setlocale(LC_ALL, "Russian");

    int a, b;
    a = 2.2;
    b = 3.3;

    cout << "[C++] Количество отрицательных элементов массива, которые удовлетворяют условию: a[i] <= d: " << calculate_cpp(a, b) << endl;
    cout << "[ASM] Количество отрицательных элементов массива, которые удовлетворяют условию: a[i] <= d: " << calculate(a, b) << endl;

    return 0;
}