#include <cstdio>
#include <cstdlib>
#include <cstring>
#include <assert.h>

#define MAX_STRING_LENGTH 256
#define MAX_RESULTS 100

// Структура для связи фамилии и группы
typedef struct {
    unsigned long name_idx;    // индекс записи в файле students.dat
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

    long get_position() {
        if (this->file != nullptr) {
            return ftell(this->file);
        }
        return -1;
    }

    bool set_position(long pos) {
        if (this->file != nullptr) {
            return fseek(this->file, pos, SEEK_SET) == 0;
        }
        return false;
    }

    bool seek_to_end() {
        if (this->file != nullptr) {
            return fseek(this->file, 0, SEEK_END) == 0;
        }
        return false;
    }

    char* read_line(char* buffer, int max_length) {
        if (this->file != nullptr) {
            return fgets(buffer, max_length, this->file);
        }
        return nullptr;
    }

    bool write_line(const char* str) {
        if (this->file != nullptr) {
            fputs(str, this->file);
            fputc('\n', this->file);
            return true;
        }
        return false;
    }

    bool read_link_record(LinkRecord* record) {
        if (this->file != nullptr) {
            return fread(record, sizeof(LinkRecord), 1, this->file) == 1;
        }
        return false;
    }

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

    long find_string_in_file(const char* filename, const char* search_str) {
        DataFile file(filename, "r");
        if (!file.is_open()) {
            return -1;
        }

        char buffer[MAX_STRING_LENGTH];
        long position = 0;

        while (file.read_line(buffer, MAX_STRING_LENGTH) != nullptr) {
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

    bool add_group(const char* groups_name) {
        long existing_pos = find_string_in_file(this->groups_filename, groups_name);
        if (existing_pos != -1) {
            printf("Группа '%s' уже существует.\n", groups_name);
            return false;
        }

        long position = append_string_to_file(this->groups_filename, groups_name);
        if (position == -1) {
            printf("Ошибка при добавлении группы.\n");
            return false;
        }

        printf("Группа '%s' успешно добавлена.\n", groups_name);
        return true;
    }

    bool add_student(const char* students, const char* groups_name) {
        long groups_idx = find_string_in_file(this->groups_filename, groups_name);
        if (groups_idx == -1) {
            printf("Группа '%s' не существует. Сначала добавьте группу.\n", groups_name);
            return false;
        }

        long name_idx = append_string_to_file(this->students_filename, students);
        if (name_idx == -1) {
            printf("Ошибка при добавлении фамилии.\n");
            return false;
        }

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

    void find_groups_by_student(const char* student) {
        unsigned long* name_positions = (unsigned long*)malloc(MAX_RESULTS * sizeof(unsigned long));
        assert(name_positions != nullptr);
        int name_count = 0;

        DataFile students_file(this->students_filename, "r");
        if (!students_file.is_open()) {
            printf("В базе данных нет студентов\n");
            free(name_positions);
            return;
        }

        char buffer[MAX_STRING_LENGTH];
        unsigned long position = 0;

        while (students_file.read_line(buffer, MAX_STRING_LENGTH) != nullptr) {
            int len = strlen(buffer);
            if (len > 0 && buffer[len - 1] == '\n') {
                buffer[len - 1] = '\0';
            }

            if (strcmp(buffer, student) == 0) {
                if (name_count < MAX_RESULTS) {
                    name_positions[name_count] = position;
                    name_count++;
                }
            }

            position = students_file.get_position();
        }

        if (name_count == 0) {
            printf("Студент с фамилией '%s' не найден.\n", student);
            free(name_positions);
            return;
        }

        printf("Студент '%s' учится в группе(ах):\n", student);

        DataFile link_file(this->link_filename, "rb");
        if (!link_file.is_open()) {
            printf("В базе данных нет связей студентов с группами\n");
            free(name_positions);
            return;
        }

        LinkRecord link;
        while (link_file.read_link_record(&link)) {
            for (int i = 0; i < name_count; i++) {
                if (link.name_idx == name_positions[i]) {
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

    void print_all_students() {
        DataFile link_file(this->link_filename, "rb");
        if (!link_file.is_open()) {
            printf("База данных пуста\n");
            return;
        }

        printf("\n--- Список всех студентов ---\n");

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

    int get_all_groups(char groups[][MAX_STRING_LENGTH], int max_groups) {
        DataFile file(this->groups_filename, "r");
        if (!file.is_open()) {
            return 0;
        }

        char buffer[MAX_STRING_LENGTH];
        int count = 0;

        while (file.read_line(buffer, MAX_STRING_LENGTH) != nullptr && count < max_groups) {
            int len = strlen(buffer);
            if (len > 0 && buffer[len - 1] == '\n') {
                buffer[len - 1] = '\0';
            }

            bool found = false;
            for (int i = 0; i < count; i++) {
                if (strcmp(groups[i], buffer) == 0) {
                    found = true;
                    break;
                }
            }

            if (!found && strlen(buffer) > 0) {
                strcpy(groups[count], buffer);
                count++;
            }
        }

        return count;
    }

    int get_all_students(char students[][MAX_STRING_LENGTH], int max_students) {
        DataFile file(this->students_filename, "r");
        if (!file.is_open()) {
            return 0;
        }

        char buffer[MAX_STRING_LENGTH];
        int count = 0;

        while (file.read_line(buffer, MAX_STRING_LENGTH) != nullptr && count < max_students) {
            int len = strlen(buffer);
            if (len > 0 && buffer[len - 1] == '\n') {
                buffer[len - 1] = '\0';
            }

            bool found = false;
            for (int i = 0; i < count; i++) {
                if (strcmp(students[i], buffer) == 0) {
                    found = true;
                    break;
                }
            }

            if (!found && strlen(buffer) > 0) {
                strcpy(students[count], buffer);
                count++;
            }
        }

        return count;
    }
};
