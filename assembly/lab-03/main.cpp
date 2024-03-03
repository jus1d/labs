#include <iostream>
#include <locale>
#include <vector>
#include "./calculate.h"

using namespace std;

int calculate_cpp(vector<int> vec, size_t vec_sz, int d)
{
    int counter = 0;
    for (size_t i = 0; i < vec.size(); ++i)
    {
        // if (vec[i] < 0 && vec[i] <= d) counter++;
        if (vec[i] < 0) counter++;
    }
    return counter;
}

int main() {
    setlocale(LC_ALL, "Russian");

    vector<int> vec = {-1, 2, -1, 4, -5};
    int d = -2;

    cout << "[C++] Количество отрицательных элементов массива, которые удовлетворяют условию: a[i] <= d: " << calculate_cpp(vec, vec.size(), d) << endl;
    cout << "[ASM] Количество отрицательных элементов массива, которые удовлетворяют условию: a[i] <= d: " << calculate(vec, vec.size(), d) << endl;

    return 0;
}
