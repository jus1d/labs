#include <iostream>
#include <locale>
#include <vector>
#include "./calculate.h"

using namespace std;

int calculate_cpp(vector<int> vec, size_t vec_sz, int d)
{
    
}

int main() {
    setlocale(LC_ALL, "Russian");

    int n, d;
    vector<int> vec;
    cout << "Введите количество элементов массива: ";
    cin >> n;
    for (int i = 0; i < n; ++i) {
        cin >> vec[i];
    }
    cout << "Введите значение D: ";
    cin >> d;

    for (int i = 0; i < n; ++i) {
        cout << vec[i] << endl;
    }
    cout << d << endl;
    cout << "[C++] Количество отрицательных элементов массива, которые удовлетворяют условию: a[i] <= d: " << calculate_cpp(vec, vec.size(), d) << endl;
    cout << "[ASM] Количество отрицательных элементов массива, которые удовлетворяют условию: a[i] <= d: " << calculate(vec, vec.size(), d) << endl;

    return 0;
}