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
        if (/*vec[i] < 0 && */vec[i] <= d) counter++;
    }
    return counter;
}

int main() {
    setlocale(LC_ALL, "Russian");

    vector<int> vec = {1, -2, -8, 9, 4};
    int d = -9;

    cout << "Количество отрицательных элементов массива, которые удовлетворяют условию: a[i] <= d: " << calculate_cpp(vec, vec.size(), d) << endl;
    cout << "Количество отрицательных элементов массива, которые удовлетворяют условию: a[i] <= d: " << calculate(vec, vec.size(), d) << endl;

    return 0;
}
