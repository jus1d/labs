#include <cstdio>
#include <algorithm>
#include <cstring>
#include <cstdlib>
#include <assert.h>

using namespace std;

#define MAX_LENGTH 80
#define endl '\n';

class Word {
public:
    char* buffer;
    size_t length;
};

class Sentence {
private:
    char* text;

public:
    Word** words;
    int word_count;
    size_t sentence_max_length;

    Sentence(size_t max_length) {
        sentence_max_length = max_length;
        text = (char*)malloc(max_length + 1);
        assert(text != nullptr);
        size_t i = 0;
        char c;
        while (i < max_length && (c = getchar()) != '\n') {
            text[i++] = c;
        }
        text[i] = '\0';
        text = (char*)realloc(text, i + 1);

        split_words();
        std::sort(words, words + word_count, [](const Word* a, const Word* b) {
            return a->length < b->length;
        });
    }

    void split_words() {
        size_t max_words = sentence_max_length / 2;
        words = (Word**)malloc(max_words * sizeof(Word*));
        assert(words != nullptr);
        word_count = 0;

        char* token = strtok(text, " ");
        while (token != nullptr) {
            words[word_count] = (Word*)malloc(sizeof(Word));
            assert(words[word_count] != nullptr);

            words[word_count]->length = strlen(token);
            words[word_count]->buffer = (char*)malloc(words[word_count]->length + 1);
            assert(words[word_count]->buffer != nullptr);
            strcpy(words[word_count]->buffer, token);

            word_count++;
            token = strtok(nullptr, " ");
        }
        words = (Word**)realloc(words, word_count * sizeof(Word*));
    }

    ~Sentence() {
        if (words) {
            for (int i = 0; i < word_count; i++) {
                if (words[i]) {
                    if (words[i]->buffer) free(words[i]->buffer);
                    free(words[i]);
                }
            }
            free(words);
        }
        free(text);
    }
};

char* build_sentence(Sentence* first, Sentence* second) {
    int total_length = 0;

    for (int i = 0; i < first->word_count; i++) {
        total_length += first->words[i]->length;
    }
    for (int i = 0; i < second->word_count; i++) {
        total_length += second->words[i]->length;
    }

    if (first->word_count > 0) total_length += first->word_count - 1;
    if (second->word_count > 0) total_length += second->word_count - 1;

    int n1 = first->word_count;
    int n2 = second->word_count;
    int max_len = (n1 > n2 ? n1 : n2);

    char* result = (char*)malloc(total_length + 1);
    assert(result != nullptr);
    result[0] = '\0';

    int i = 0, j = 0, k = 0;
    while (k < max_len * 2) {
        if (k % 2 == 0 && n1 > 0) {
            strcat(result, first->words[i]->buffer);
            i = (i + 1) % n1;
        } else if (n2 > 0) {
            strcat(result, second->words[j]->buffer);
            j = (j + 1) % n2;
        }
        k++;
        if ((i == 0 && j == 0 && k > 0) || (k >= n1 + n2)) break;
        strcat(result, " ");
    }

    return result;
}

void intro() {
    printf("Лабораторная работа 1. Вариант - 19\n"
        "  | Ввести с клавиатуры два произвольных абстрактных предложения каждое длиной не более 80 символов,\n"
        "  | состоящие из абстрактных слов. Слова в предложении отделяются друг от друга как минимум одним символом «пробел».\n"
        "  | Предложение должно быть введено с помощью функции getchar(). Используя исходные предложения, построить новое\n"
        "  | предложение, в котором на нечётных местах находятся упорядоченные по возрастанию их длин слова первого предложения,\n"
        "  | а на чётных местах – упорядоченные по возрастанию их длин слова второго предложения. Если слова в одном из исходных\n"
        "  | предложений закончатся раньше, его используют с начала. Построение результирующего предложения завершается, когда\n"
        "  | закончатся слова в самом длинном из исходных предложений.\n\n");
}

int main() {
    intro();
    printf("Введите первое предложение: ");
    Sentence first(MAX_LENGTH);

    printf("Введите второе предложение: ");
    Sentence second(MAX_LENGTH);

    char *sentence = build_sentence(&first, &second);
    printf("Результат: %s\n", sentence);
    free(sentence);

    return 0;
}
