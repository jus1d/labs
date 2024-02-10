def greeting():
    print("ЛР4: Выполнил Фадеев Артем, студент группы 6101-020302D, вариант 21")
    print('Задание 1:\n'
          'Найти количество натуральных значений n из отрезка [1; 1000], для которых все цифры значения F(n) - четные')
    print('Задание 2:\n'
          'Найти количество четных цифр результата вычисления F(x)')


def are_all_digits_even(num: int) -> bool:
    num = abs(num)
    if num < 10:
        return num % 2 == 0

    if (num % 10) % 2 != 0:
        return False

    return are_all_digits_even(num // 10)


def count_even_digits(n: int) -> int:
    if n < 10:
        if n % 2 == 0:
            return 1
        else:
            return 0
    else:
        if (n % 10) % 2 == 0:
            return count_even_digits(n // 10) + 1
        else:
            return count_even_digits(n // 10)


def f(n: int) -> int:
    if n <= 15:
        return n * n + 3 * n + 9
    if n > 15 and n % 3 == 0:
        return f(n - 1) + n - 2
    if n > 15 and n % 3 != 0:
        return f(n - 2) + n + 2


def first_task() -> int:
    counter = 0
    for i in range(1, 1001):
        n = f(i)
        if are_all_digits_even(n):
            counter += 1
    return counter


def second_task(x: int) -> int:
    n = f(x)
    print(f'f({x}) = {n}')
    return count_even_digits(n)


def main():
    greeting()

    first_task_result = first_task()

    print(f'Ответ к заданию 1: {first_task_result}')

    second_task_input = int(input('Введите Х для выполнения 2го задания: '))

    second_task_result = second_task(second_task_input)

    print(f'Ответ к заданию 2: {second_task_result}')


if __name__ == '__main__':
    main()
