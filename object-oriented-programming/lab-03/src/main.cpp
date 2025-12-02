#include <cstdio>
#include <cstring>
#include "ui.cpp"
#include "database.cpp"

#define MAX_STRING_LENGTH 256
#define MAX_RESULTS 100

int main() {
    intro();

    StudentDatabase db("students.dat", "groups.dat", "links.idx");

    char buffer[MAX_STRING_LENGTH];
    char students[MAX_STRING_LENGTH];
    char groups[MAX_STRING_LENGTH];
    int choice;

    char all_groups[MAX_RESULTS][MAX_STRING_LENGTH];
    char all_students[MAX_RESULTS][MAX_STRING_LENGTH];

    while (true) {
        print_menu();

        if (fgets(buffer, MAX_STRING_LENGTH, stdin) == nullptr) {
            printf("Ошибка ввода.\n");
            continue;
        }

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
            case 1: {
                input_string("Введите номер группы: ", groups, MAX_STRING_LENGTH);

                if (strlen(groups) > 0) {
                    db.add_group(groups);
                }
                break;
            }
            case 2: {
                int group_count = db.get_all_groups(all_groups, MAX_RESULTS);

                input_string("Введите фамилию студента: ", students, MAX_STRING_LENGTH);

                if (group_count > 0) {
                    input_with_autocomplete("Введите номер группы: ", groups, MAX_STRING_LENGTH, all_groups, group_count);
                } else {
                    input_string("Введите номер группы: ", groups, MAX_STRING_LENGTH);
                }

                if (strlen(students) > 0 && strlen(groups) > 0) {
                    db.add_student(students, groups);
                }
                break;
            }
            case 3: {
                int student_count = db.get_all_students(all_students, MAX_RESULTS);

                if (student_count > 0) {
                    input_with_autocomplete("Введите фамилию студента: ", students, MAX_STRING_LENGTH, all_students, student_count);
                } else {
                    input_string("Введите фамилию студента: ", students, MAX_STRING_LENGTH);
                }

                if (strlen(students) > 0) {
                    db.find_groups_by_student(students);
                }
                break;
            }
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
