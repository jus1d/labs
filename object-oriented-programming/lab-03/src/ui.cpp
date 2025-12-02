#include <cstdio>
#include <cstring>
#include <termios.h>
#include <unistd.h>
#include <strings.h>

#define MAX_STRING_LENGTH 256
#define MAX_SUGGESTIONS 10

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

// Функция для поиска совпадающих строк по префиксу (case-insensitive)
static int find_matches(const char* prefix, char suggestions[][MAX_STRING_LENGTH], int total_count,
                 char matches[][MAX_STRING_LENGTH], int max_matches) {
    int match_count = 0;
    int prefix_len = strlen(prefix);

    // Если префикс пустой, возвращаем все варианты (до max_matches)
    if (prefix_len == 0) {
        for (int i = 0; i < total_count && match_count < max_matches; i++) {
            strcpy(matches[match_count], suggestions[i]);
            match_count++;
        }
        return match_count;
    }

    for (int i = 0; i < total_count && match_count < max_matches; i++) {
        // Case-insensitive сравнение
        if (strncasecmp(suggestions[i], prefix, prefix_len) == 0) {
            strcpy(matches[match_count], suggestions[i]);
            match_count++;
        }
    }

    return match_count;
}

// Интерактивный ввод с автодополнением
void input_with_autocomplete(const char* prompt, char* buffer, int max_length, char suggestions[][MAX_STRING_LENGTH], int suggestion_count) {
    printf("%s", prompt);
    fflush(stdout);

    char input[MAX_STRING_LENGTH] = "";
    int input_len = 0;
    int ch;
    bool has_suggestions = false;

    // Настройка терминала для посимвольного ввода
    struct termios old_tio, new_tio;
    tcgetattr(STDIN_FILENO, &old_tio);
    new_tio = old_tio;
    new_tio.c_lflag &= ~(ICANON | ECHO);
    tcsetattr(STDIN_FILENO, TCSANOW, &new_tio);

    // Показать начальные подсказки (все доступные варианты, максимум 10)
    if (suggestion_count > 0) {
        char matches[MAX_SUGGESTIONS][MAX_STRING_LENGTH];
        int match_count = find_matches("", suggestions, suggestion_count, matches, MAX_SUGGESTIONS);

        if (match_count > 0) {
            printf("\033[s");  // Сохранить позицию курсора
            printf("\n");
            for (int i = 0; i < match_count; i++) {
                printf("  %d) %s", i + 1, matches[i]);
                if (i < match_count - 1) printf(", ");
            }
            printf("\033[u");  // Восстановить позицию курсора
            has_suggestions = true;
            fflush(stdout);
        }
    }

    while (true) {
        ch = getchar();

        if (ch == '\n' || ch == EOF) {
            // Завершение ввода
            break;
        } else if (ch == 127 || ch == 8) {
            // Backspace - удаляем UTF-8 символ (может быть многобайтовым)
            if (input_len > 0) {
                // Очистить старые подсказки если они есть
                if (has_suggestions) {
                    printf("\033[1B");  // Вниз на 1 линию
                    printf("\033[2K");  // Очистить линию подсказок
                    printf("\033[1A");  // Вверх на 1 линию
                    has_suggestions = false;
                }

                // Удаляем UTF-8 символ (может занимать несколько байт)
                // Проверяем сколько байт занимает последний символ
                int bytes_to_remove = 1;
                if (input_len >= 2 && (unsigned char)input[input_len - 2] >= 0xC0) {
                    // Двухбайтовый UTF-8 символ
                    bytes_to_remove = 2;
                } else if (input_len >= 3 && (unsigned char)input[input_len - 3] >= 0xE0) {
                    // Трёхбайтовый UTF-8 символ
                    bytes_to_remove = 3;
                } else if (input_len >= 4 && (unsigned char)input[input_len - 4] >= 0xF0) {
                    // Четырёхбайтовый UTF-8 символ
                    bytes_to_remove = 4;
                }

                input_len -= bytes_to_remove;
                input[input_len] = '\0';
                printf("\b \b");  // Стереть символ
                fflush(stdout);

                // Показать новые подсказки (включая случай когда input_len == 0)
                char matches[MAX_SUGGESTIONS][MAX_STRING_LENGTH];
                int match_count = find_matches(input, suggestions, suggestion_count, matches, MAX_SUGGESTIONS);

                if (match_count > 0) {
                    printf("\033[s");  // Сохранить позицию курсора
                    printf("\n");
                    for (int i = 0; i < match_count; i++) {
                        printf("  %d) %s", i + 1, matches[i]);
                        if (i < match_count - 1) printf(", ");
                    }
                    printf("\033[u");  // Восстановить позицию курсора
                    has_suggestions = true;
                    fflush(stdout);
                }
            }
        } else if (input_len < max_length - 4) {
            // Обычный символ или UTF-8 символ
            // Очистить старые подсказки если они есть
            if (has_suggestions) {
                printf("\033[1B");  // Вниз на 1 линию
                printf("\033[2K");  // Очистить линию подсказок
                printf("\033[1A");  // Вверх на 1 линию
                has_suggestions = false;
            }

            // Добавляем байт
            input[input_len++] = ch;
            input[input_len] = '\0';
            printf("%c", ch);

            // Если это начало многобайтового UTF-8 символа, читаем остальные байты
            if ((unsigned char)ch >= 0xC0) {
                int extra_bytes = 0;
                if ((unsigned char)ch >= 0xF0) extra_bytes = 3;
                else if ((unsigned char)ch >= 0xE0) extra_bytes = 2;
                else if ((unsigned char)ch >= 0xC0) extra_bytes = 1;

                for (int i = 0; i < extra_bytes && input_len < max_length - 1; i++) {
                    ch = getchar();
                    input[input_len++] = ch;
                    input[input_len] = '\0';
                    printf("%c", ch);
                }
            }

            fflush(stdout);

            // Найти и показать подсказки
            char matches[MAX_SUGGESTIONS][MAX_STRING_LENGTH];
            int match_count = find_matches(input, suggestions, suggestion_count, matches, MAX_SUGGESTIONS);

            if (match_count > 0) {
                printf("\033[s");  // Сохранить позицию курсора
                printf("\n");
                for (int i = 0; i < match_count; i++) {
                    printf("  %d) %s", i + 1, matches[i]);
                    if (i < match_count - 1) printf(", ");
                }
                printf("\033[u");  // Восстановить позицию курсора
                has_suggestions = true;
                fflush(stdout);
            }
        }
    }

    // Очистить подсказки перед выходом
    if (has_suggestions) {
        printf("\033[1B");  // Вниз на 1 линию
        printf("\033[2K");  // Очистить линию подсказок
        printf("\033[1A");  // Вверх на 1 линию
    }

    // Восстановить настройки терминала
    tcsetattr(STDIN_FILENO, TCSANOW, &old_tio);

    printf("\n");
    strcpy(buffer, input);
}

// Главное меню
void print_menu() {
    printf("\n--- Меню ---\n");
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
