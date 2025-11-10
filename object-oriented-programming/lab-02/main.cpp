#include <cstdio>
#include <cstdlib>
#include <cstring>
#include <assert.h>

#define MAX_FILENAME_LENGTH 256
#define MAX_SENTENCE_LENGTH 512
#define INITIAL_ARRAY_SIZE 10

// Структура для хранения информации о количестве слов
typedef struct {
    int word_count;           // количество слов в предложении
    int sentence_count;       // количество предложений с таким количеством слов
} WordCountRecord;

// Класс для работы с файлами
class TextFile {
private:
    FILE* file;
    char* filename;

public:
    TextFile(const char* name, const char* mode) {
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

    ~TextFile() {
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

    void write_sentence(const char* sentence) {
        if (this->file != nullptr) {
            fputs(sentence, this->file);
            fputc('\n', this->file);
        }
    }

    char* read_sentence(char* buffer, int max_length) {
        if (this->file != nullptr) {
            return fgets(buffer, max_length, this->file);
        }
        return nullptr;
    }

    bool end_of_file() const {
        return this->file != nullptr && feof(this->file);
    }

    void close() {
        if (this->file != nullptr) {
            fclose(this->file);
            this->file = nullptr;
        }
    }
};

// Класс для анализа предложений
class SentenceAnalyzer {
private:
    WordCountRecord* records;
    int records_count;
    int records_capacity;

    // Подсчёт количества слов в предложении
    int count_words_in_sentence(const char* sentence) {
        int word_count = 0;
        bool in_word = false;

        for (int i = 0; sentence[i] != '\0'; i++) {
            if (sentence[i] != ' ' && sentence[i] != '\t' &&
                sentence[i] != '\n' && sentence[i] != '\r') {
                if (!in_word) {
                    word_count++;
                    in_word = true;
                }
            } else {
                in_word = false;
            }
        }

        return word_count;
    }

    // Поиск существующей записи с данным количеством слов
    int find_record_index(int word_count) {
        for (int i = 0; i < this->records_count; i++) {
            if (this->records[i].word_count == word_count) {
                return i;
            }
        }
        return -1;
    }

    // Добавление или обновление записи
    void add_or_update_record(int word_count) {
        int index = find_record_index(word_count);

        if (index != -1) {
            // Запись найдена - увеличиваем счётчик предложений
            this->records[index].sentence_count++;
        } else {
            // Запись не найдена - добавляем новую
            if (this->records_count >= this->records_capacity) {
                // Расширяем массив
                this->records_capacity *= 2;
                this->records = (WordCountRecord*)realloc(
                    this->records,
                    this->records_capacity * sizeof(WordCountRecord)
                );
                assert(this->records != nullptr);
            }

            this->records[this->records_count].word_count = word_count;
            this->records[this->records_count].sentence_count = 1;
            this->records_count++;
        }
    }

public:
    SentenceAnalyzer() {
        this->records_capacity = INITIAL_ARRAY_SIZE;
        this->records = (WordCountRecord*)malloc(
            this->records_capacity * sizeof(WordCountRecord)
        );
        assert(this->records != nullptr);
        this->records_count = 0;
    }

    ~SentenceAnalyzer() {
        if (this->records != nullptr) {
            free(this->records);
        }
    }

    // Анализ файла и построение массива записей
    void analyze_file(const char* filename) {
        TextFile file(filename, "r");

        if (!file.is_open()) {
            return;
        }

        char buffer[MAX_SENTENCE_LENGTH];

        while (file.read_sentence(buffer, MAX_SENTENCE_LENGTH) != nullptr) {
            // Удаляем символ новой строки в конце
            int len = strlen(buffer);
            if (len > 0 && buffer[len - 1] == '\n') {
                buffer[len - 1] = '\0';
            }

            // Пропускаем пустые строки
            if (strlen(buffer) == 0) {
                continue;
            }

            int word_count = count_words_in_sentence(buffer);
            if (word_count > 0) {
                add_or_update_record(word_count);
            }
        }
    }

    // Функция сравнения для qsort (по убыванию количества предложений)
    static int compare_records(const void* a, const void* b) {
        WordCountRecord* rec_a = (WordCountRecord*)a;
        WordCountRecord* rec_b = (WordCountRecord*)b;

        // Сортировка по убыванию количества предложений
        if (rec_b->sentence_count != rec_a->sentence_count) {
            return rec_b->sentence_count - rec_a->sentence_count;
        }

        // При равенстве - по возрастанию количества слов
        return rec_a->word_count - rec_b->word_count;
    }

    // Сортировка массива записей
    void sort_records() {
        qsort(this->records, this->records_count,
              sizeof(WordCountRecord), compare_records);
    }

    // Вывод результатов на экран
    void print_results() const {
        printf("|  Количество слов  |  Количество предложений  |\n");
        printf("|-------------------|--------------------------|\n");
        for (int i = 0; i < this->records_count; i++) {
            printf("|       %-12d|       %-19d|\n",
                   this->records[i].word_count,
                   this->records[i].sentence_count);
        }
    }
};

// Функция для ввода имени файла
void input_filename(char* filename) {
    printf("Введите имя файла: ");
    if (scanf("%255s", filename) != 1) {
        filename[0] = '\0';
    }
    // Очистка буфера ввода
    while (getchar() != '\n');
}

// Функция для ввода предложений в файл
void input_sentences_to_file(const char* filename) {
    TextFile file(filename, "w");

    if (!file.is_open()) {
        return;
    }

    printf("\nВведите предложения (для завершения введите пустую строку):\n");

    char buffer[MAX_SENTENCE_LENGTH];
    int sentence_number = 1;

    while (true) {
        printf("%d> ", sentence_number);

        if (fgets(buffer, MAX_SENTENCE_LENGTH, stdin) == nullptr) {
            break;
        }

        // Удаляем символ новой строки
        int len = strlen(buffer);
        if (len > 0 && buffer[len - 1] == '\n') {
            buffer[len - 1] = '\0';
        }

        // Проверка на пустую строку (завершение ввода)
        if (strlen(buffer) == 0) {
            break;
        }

        file.write_sentence(buffer);
        sentence_number++;
    }

    file.close();
    printf("\nФайл '%s' успешно создан и заполнен.\n", filename);
}

void intro() {
    printf("Лабораторная работа 2. Вариант - 19\n"
        "  | Написать программу, которая создаёт в текстовом режиме файл в текущем\n"
        "  | каталоге с именем, вводимым с клавиатуры, и заполняет его произвольными\n"
        "  | предложениями, которые также вводятся с клавиатуры. После ввода последнего\n"
        "  | предложения файл закрывается. Построить в оперативной памяти массив записей\n"
        "  | (структур) вида <количество_слов_в_предложении, количество_предложений>,\n"
        "  | анализируя строки файла. Отсортировать полученный массив по убыванию\n"
        "  | значений поля \"количество_предложений\", результирующий массив вывести на\n"
        "  | экран. Для сортировки элементов массива использовать стандартную функцию в\n"
        "  | стиле языка С qsort(). \n");
}

int main() {
    intro();

    char filename[MAX_FILENAME_LENGTH];

    input_filename(filename);

    if (strlen(filename) == 0) {
        printf("Ошибка: некорректное имя файла.\n");
        return 1;
    }

    input_sentences_to_file(filename);

    SentenceAnalyzer analyzer;
    analyzer.analyze_file(filename);

    analyzer.sort_records();

    analyzer.print_results();

    return 0;
}
