import os.path


def print_task():
    print("ЛР3: Выполнил Фадеев Артем, студент группы 6101-020302D, вариант 21\n"
          "Задание:\n"
          "В каждой строке текстового файла хранится информация следующего вида: слово и число его повторений.\n"
          "Требуется написать программу, которая для каждой строки исходного файла будет печатать в результирующий\n"
          "файл слова, повторив их столько раз, сколько указано")


def read_input_data(filename: str) -> list[list]:
    if not os.path.exists(filename):
        raise Exception("Файла с таким именем не существует")

    input_data = []
    with open('input.txt', 'r') as file:
        strings = file.readlines()
        for s in strings:
            input_data.append(s.split())

    return input_data


def write_output_data(input_data: list[list], filename: str) -> None:
    with open(filename, 'w') as file:
        for i in range(len(input_data)):
            data = input_data[i]
            n = int(data[1])
            for j in range(n):
                file.write(data[0] + ' ')
            if i != len(input_data) - 1:
                file.write('\n')


def main():
    print_task()

    input_file_name = input("Введите название входного файла: ")
    while not os.path.exists(input_file_name):
        input_file_name = input("Введенного Вами файла не существует\n"
                                "Введите название входного файла: ")

    input_data = read_input_data(input_file_name)

    output_file_name = input("Введите название выходного файла: ")
    write_output_data(input_data, output_file_name)

    print("Задание выполнено")


if __name__ == '__main__':
    main()
