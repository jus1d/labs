#include <cstdio>
#include <cstdlib>
#include <cstring>
#include <assert.h>

#define MAX_STRING_LENGTH 256
#define MAX_RESULTS 100

// Структура для связи фамилии и группы
typedef struct {
    unsigned long name_idx;   // индекс записи в файле students.dat
    unsigned long groups_idx;  // индекс записи в файле groups.dat
} LinkRecord;

// Класс для работы с файлами
class DataFile {
private:
    FILE* file;
    char* filename;

public:
    DataFile(const char* name, const char* mode) {
        this->filename = (char*)malloc(strlen(name) + 1);
        assert(this->filename != nullptr);
        strcpy(this->filename, name);

        this->file = fopen(name, mode);
        if (this->file == nullptr) {
            printf("Ошибка: не удалось открыть файл '%s'\n", name);
            free(this->filename);
            this->filename = nullptr;
        }
    }

    ~DataFile() {
        if (this->file != nullptr) {
            fclose(this->file);
        }
        if (this->filename != nullptr) {
            free(this->filename);
        }
    }

    bool is_open() const {
        return this->file != nullptr;
    }

    FILE* get_file() {
        return this->file;
    }

    void close() {
        if (this->file != nullptr) {
            fclose(this->file);
            this->file = nullptr;
        }
    }

    // Получение текущей позиции в файле
    long get_position() {
        if (this->file != nullptr) {
            return ftell(this->file);
        }
        return -1;
    }

    // Установка позиции в файле
    bool set_position(long pos) {
        if (this->file != nullptr) {
            return fseek(this->file, pos, SEEK_SET) == 0;
        }
        return false;
    }

    // Переход в конец файла
    bool seek_to_end() {
        if (this->file != nullptr) {
            return fseek(this->file, 0, SEEK_END) == 0;
        }
        return false;
    }

    // Чтение строки
    char* read_line(char* buffer, int max_length) {
        if (this->file != nullptr) {
            return fgets(buffer, max_length, this->file);
        }
        return nullptr;
    }

    // Запись строки с переводом строки
    bool write_line(const char* str) {
        if (this->file != nullptr) {
            fputs(str, this->file);
            fputc('\n', this->file);
            return true;
        }
        return false;
    }

    // Чтение структуры LinkRecord
    bool read_link_record(LinkRecord* record) {
        if (this->file != nullptr) {
            return fread(record, sizeof(LinkRecord), 1, this->file) == 1;
        }
        return false;
    }

    // Запись структуры LinkRecord
    bool write_link_record(const LinkRecord* record) {
        if (this->file != nullptr) {
            return fwrite(record, sizeof(LinkRecord), 1, this->file) == 1;
        }
        return false;
    }
};

// Класс для управления базой данных студентов
class StudentDatabase {
private:
    const char* students_filename;
    const char* groups_filename;
    const char* link_filename;

    // Проверка существования строки в файле
    // Возвращает позицию начала строки или -1 если не найдена
    long find_string_in_file(const char* filename, const char* search_str) {
        DataFile file(filename, "r");
        if (!file.is_open()) {
            return -1;
        }

        char buffer[MAX_STRING_LENGTH];
        long position = 0;

        while (file.read_line(buffer, MAX_STRING_LENGTH) != nullptr) {
            // Удаляем символ новой строки
            int len = strlen(buffer);
            if (len > 0 && buffer[len - 1] == '\n') {
                buffer[len - 1] = '\0';
            }

            if (strcmp(buffer, search_str) == 0) {
                return position;
            }

            position = file.get_position();
        }

        return -1;
    }

    // Добавление строки в файл и возврат позиции
    long append_string_to_file(const char* filename, const char* str) {
        DataFile file(filename, "a+");
        if (!file.is_open()) {
            return -1;
        }

        file.seek_to_end();
        long position = file.get_position();
        file.write_line(str);

        return position;
    }

    // Чтение строки из файла по позиции
    bool read_string_at_position(const char* filename, long position, char* buffer) {
        DataFile file(filename, "r");
        if (!file.is_open()) {
            return false;
        }

        if (!file.set_position(position)) {
            return false;
        }

        if (file.read_line(buffer, MAX_STRING_LENGTH) == nullptr) {
            return false;
        }

        // Удаляем символ новой строки
        int len = strlen(buffer);
        if (len > 0 && buffer[len - 1] == '\n') {
            buffer[len - 1] = '\0';
        }

        return true;
    }

public:
    StudentDatabase(const char* students_file, const char* groups_file, const char* link_file) {
        this->students_filename = students_file;
        this->groups_filename = groups_file;
        this->link_filename = link_file;
    }

    // Добавление новой группы
    bool add_group(const char* groups_name) {
        // Проверяем, существует ли уже такая группа
        long existing_pos = find_string_in_file(this->groups_filename, groups_name);
        if (existing_pos != -1) {
            printf("Группа '%s' уже существует.\n", groups_name);
            return false;
        }

        // Добавляем группу
        long position = append_string_to_file(this->groups_filename, groups_name);
        if (position == -1) {
            printf("Ошибка при добавлении группы.\n");
            return false;
        }

        printf("Группа '%s' успешно добавлена.\n", groups_name);
        return true;
    }

    // Добавление нового студента с указанием группы
    bool add_student(const char* students, const char* groups_name) {
        // Проверяем, существует ли группа
        long groups_idx = find_string_in_file(this->groups_filename, groups_name);
        if (groups_idx == -1) {
            printf("Группа '%s' не существует. Сначала добавьте группу.\n", groups_name);
            return false;
        }

        // Добавляем фамилию в students.dat
        long name_idx = append_string_to_file(this->students_filename, students);
        if (name_idx == -1) {
            printf("Ошибка при добавлении фамилии.\n");
            return false;
        }

        // Создаём связь в links.idx
        LinkRecord link;
        link.name_idx = name_idx;
        link.groups_idx = groups_idx;

        DataFile link_file(this->link_filename, "ab");
        if (!link_file.is_open() || !link_file.write_link_record(&link)) {
            printf("Ошибка при создании связи.\n");
            return false;
        }

        printf("Студент '%s' добавлен в группу '%s'.\n", students, groups_name);
        return true;
    }

    // Поиск групп по фамилии студента
    void find_groups_by_student(const char* students) {
        // Находим все позиции с данной фамилией в students.dat
        unsigned long* name_positions = (unsigned long*)malloc(MAX_RESULTS * sizeof(unsigned long));
        assert(name_positions != nullptr);
        int name_count = 0;

        DataFile students_file(this->students_filename, "r");
        if (!students_file.is_open()) {
            printf("Ошибка при открытии файла `students.dat`\n");
            free(name_positions);
            return;
        }

        char buffer[MAX_STRING_LENGTH];
        unsigned long position = 0;

        while (students_file.read_line(buffer, MAX_STRING_LENGTH) != nullptr) {
            // Удаляем символ новой строки
            int len = strlen(buffer);
            if (len > 0 && buffer[len - 1] == '\n') {
                buffer[len - 1] = '\0';
            }

            if (strcmp(buffer, students) == 0) {
                if (name_count < MAX_RESULTS) {
                    name_positions[name_count] = position;
                    name_count++;
                }
            }

            position = students_file.get_position();
        }

        if (name_count == 0) {
            printf("Студент с фамилией '%s' не найден.\n", students);
            free(name_positions);
            return;
        }

        // Находим соответствующие группы в links.idx
        printf("Студент '%s' учится в группе(ах):\n", students);

        DataFile link_file(this->link_filename, "rb");
        if (!link_file.is_open()) {
            printf("Ошибка при открытии файла `links.idx`\n");
            free(name_positions);
            return;
        }

        LinkRecord link;
        while (link_file.read_link_record(&link)) {
            // Проверяем, соответствует ли name_idx одной из найденных позиций
            for (int i = 0; i < name_count; i++) {
                if (link.name_idx == name_positions[i]) {
                    // Читаем название группы по groups_idx
                    char groups_name[MAX_STRING_LENGTH];
                    if (read_string_at_position(this->groups_filename, link.groups_idx, groups_name)) {
                        printf("  - %s\n", groups_name);
                    }
                    break;
                }
            }
        }

        free(name_positions);
    }

    // Вывод всех студентов и их групп
    void print_all_students() {
        DataFile link_file(this->link_filename, "rb");
        if (!link_file.is_open()) {
            printf("База данных пуста или файл не существует.\n");
            return;
        }

        printf("\n=== Список всех студентов ===\n");

        LinkRecord link;
        char students[MAX_STRING_LENGTH];
        char groups[MAX_STRING_LENGTH];
        int index = 0;

        while (link_file.read_link_record(&link)) {
            if (read_string_at_position(this->students_filename, link.name_idx, students) &&
                read_string_at_position(this->groups_filename, link.groups_idx, groups)) {
                printf("%d:\n", index);
                printf("- Фамилия: %s\n", students);
                printf("- Группа: %s\n\n", groups);
                index++;
            }
        }
    }
};

// Функция для ввода строки
void input_string(const char* prompt, char* buffer, int max_length) {
    printf("%s", prompt);
    if (fgets(buffer, max_length, stdin) == nullptr) {
        buffer[0] = '\0';
        return;
    }

    // Удаляем символ новой строки
    int len = strlen(buffer);
    if (len > 0 && buffer[len - 1] == '\n') {
        buffer[len - 1] = '\0';
    }
}

// Главное меню
void print_menu() {
    printf("\n=== Меню ===\n");
    printf("1. Добавить новую группу\n");
    printf("2. Добавить студента с указанием группы\n");
    printf("3. Найти группы студента по фамилии\n");
    printf("4. Показать всех студентов\n");
    printf("0/q. Выход\n");
    printf("Выберите действие: ");
}

void intro() {
    printf("Лабораторная работа 3. Вариант - 19\n"
        "  | Реализовать информационную базу, состоящую из трёх файлов:\n"
        "  | - `students.dat` - содержит список фамилий студентов;\n"
        "  | - `groups.dat` - содержит номера студенческих групп;\n"
        "  | - `links.idx` - содержит связи между фамилиями и группами.\n"
        "  | \n"
        "  | Функции программы:\n"
        "  | - Включение новой фамилии студента с указанием группы\n"
        "  | - Добавление новой группы\n"
        "  | - Вывод на экран номера группы по заданной фамилии студента\n\n");
}

int main() {
    intro();

    StudentDatabase db("students.dat", "groups.dat", "links.idx");

    char buffer[MAX_STRING_LENGTH];
    char students[MAX_STRING_LENGTH];
    char groups[MAX_STRING_LENGTH];
    int choice;

    while (true) {
        print_menu();

        if (fgets(buffer, MAX_STRING_LENGTH, stdin) == nullptr) {
            printf("Ошибка ввода.\n");
            continue;
        }

        // Проверяем на команды выхода
        int len = strlen(buffer);
        if (len > 0 && buffer[len - 1] == '\n') {
            buffer[len - 1] = '\0';
        }

        if (strcmp(buffer, "q") == 0 || strcmp(buffer, "quit") == 0 || strcmp(buffer, "exit") == 0) {
            printf("Выход из программы.\n");
            return 0;
        }

        if (sscanf(buffer, "%d", &choice) != 1) {
            printf("Ошибка ввода.\n");
            continue;
        }

        switch (choice) {
            case 1:
                input_string("Введите номер группы: ", groups, MAX_STRING_LENGTH);
                if (strlen(groups) > 0) {
                    db.add_group(groups);
                }
                break;

            case 2:
                input_string("Введите фамилию студента: ", students, MAX_STRING_LENGTH);
                input_string("Введите номер группы: ", groups, MAX_STRING_LENGTH);
                if (strlen(students) > 0 && strlen(groups) > 0) {
                    db.add_student(students, groups);
                }
                break;

            case 3:
                input_string("Введите фамилию студента: ", students, MAX_STRING_LENGTH);
                if (strlen(students) > 0) {
                    db.find_groups_by_student(students);
                }
                break;

            case 4:
                db.print_all_students();
                break;

            case 0:
                printf("Выход из программы.\n");
                return 0;

            default:
                printf("Неверный выбор. Попробуйте снова.\n");
                break;
        }
    }

    return 0;
}
