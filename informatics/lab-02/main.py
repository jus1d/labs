import random


def greeting():
    print("ЛР 2: выполнил Фадеев Артем, студент группы 6101-020302D, вариант 21\n"
          "Задание 1\n"
          "Найти минимальный отрицательный элемент, некратный заданному числу\n"
          "Задание 2\n"
          "Найти индекс первого нулеого элемента\n"
          "Задание 3\n"
          "Отсортировать массив по убыванию (Сортировка Шелла)")


def print_first_task():
    print("Задание 1\n"
          "Найти минимальный отрицательный элемент, некратный заданному числу")


def print_second_task():
    print("Задание 2\n"
          "Найти индекс первого нулеого элемента")


def print_third_task():
    print("Задание 3\n"
          "Отсортировать массив по убыванию (Сортировка Шелла)")


def get_random_array(length: int, minimal_bound: int, maximal_bound: int) -> list[int]:
    if maximal_bound < minimal_bound:
        maximal_bound, minimal_bound = minimal_bound, maximal_bound

    return [random.randint(minimal_bound, maximal_bound) for _ in range(length)]


def get_array_from_user_input() -> list[int]:
    return [int(x) for x in input("Введите массив чисел: ").split()]


def find_minimal_negative_nonrecurring_to(array: list[int], value: int) -> int:
    if value == 0:
        return 0

    ans = 0
    for i in range(len(array)):
        if array[i] < ans and array[i] % value != 0:
            ans = array[i]

    return ans


def find_first_zero_element_index(array: list[int]) -> int:
    i = 0
    while i < len(array) and array[i] != 0:
        i += 1

    if i == len(array):
        return -1

    return i


def shell_sort(array: list[int]) -> list[int]:
    step = len(array) // 2
    n = len(array)

    while step > 0:
        for i in range(step, n):
            j = i
            delta = j - step
            while delta >= 0 and array[delta] < array[j]:
                array[delta], array[j] = array[j], array[delta]
                j = delta
                delta -= step
        step //= 2

    return array


def main():
    greeting()

    inp = ''
    arr = []

    while inp not in ['1', '2']:
        inp = input('Какой массив хотите использовать?\n\n'
                    '\t1 - Ручной ввод\n'
                    '\t2 - Случайный массив\n')

        if inp == '1':
            arr = get_array_from_user_input()
        elif inp == '2':
            length = int(input('Выберете длину случайного массива: '))
            minimal_bound = int(input('Выберете нижнюю границу массива: '))
            maximal_bound = int(input('Выберете верхнюю границу массива: '))

            arr = get_random_array(length, minimal_bound, maximal_bound)
        else:
            print('Нет такого варианта')

    print_first_task()

    num = int(input('Введите число на которое не должен делиться результат 1го задания: '))
    while num == 0:
        num = int(input('Деление на 0 запрещено\nВведите число на которое не должен делиться результат 1го задания: '))

    first_task_answer = find_minimal_negative_nonrecurring_to(arr, num)

    if first_task_answer == 0:
        print(f'Массив: {arr}\n'
              f'Ответ: В массиве нет подходящих элементов')
    else:
        print(f'Массив: {arr}\n'
              f'Ответ: {first_task_answer}\n')

    print_second_task()

    second_task_answer = find_first_zero_element_index(arr)

    if second_task_answer == -1:
        print(f'Массив: {arr}\n'
              f'Ответ: в массиве нет нулевых элементов\n')
    else:
        print(f'Массив: {arr}\n'
              f'Ответ: {second_task_answer}\n')

    print_third_task()

    print(f'Исходный массив: \n'
          f'{arr}')

    shell_sort(arr)

    print(f'Отсортированный массив:\n'
          f'{arr}')


if __name__ == '__main__':
    main()
