#include <cstdio>
#include <algorithm>
#include <cstring>
#include <cstdlib>
#include <assert.h>

using namespace std;

#define DEFAULT_MAX_LENGTH 80

class Word {
public:
    char* data;
    size_t length;
};

class Sentence {
private:
    char* data;
    Word** words;
    int word_count;
    size_t sentence_max_length;

public:
    Word **get_words() {
        return this->words;
    }

    int get_word_count() {
        return this->word_count;
    }

    Sentence(size_t max_length) {
        this->sentence_max_length = max_length;
        this->data = (char*)malloc(max_length + 1);
        assert(this->data != nullptr);
        size_t i = 0;
        char c;
        while (i < max_length && (c = getchar()) != '\n') {
            this->data[i++] = c;
        }
        this->data[i] = '\0';
        this->data = (char*)realloc(this->data, i + 1);

        split_words();
        std::sort(words, words + word_count, [](const Word* a, const Word* b) {
            return a->length < b->length;
        });
    }

    Sentence(char *data) {
        this->sentence_max_length = DEFAULT_MAX_LENGTH;
        strcpy(this->data, data);

        split_words();
        std::sort(words, words + word_count, [](const Word* a, const Word* b) {
            return a->length < b->length;
        });
    }

    void split_words() {
        size_t max_words = this->sentence_max_length / 2;
        this->words = (Word**)malloc(max_words * sizeof(Word*));
        assert(this->words != nullptr);
        word_count = 0;

        char* token = strtok(this->data, " ");
        while (token != nullptr) {
            this->words[word_count] = (Word*)malloc(sizeof(Word));
            assert(this->words[word_count] != nullptr);

            this->words[word_count]->length = strlen(token);
            this->words[word_count]->data = (char*)malloc(words[word_count]->length + 1);
            assert(this->words[word_count]->data != nullptr);
            strcpy(this->words[word_count]->data, token);

            word_count++;
            token = strtok(nullptr, " ");
        }
        this->words = (Word**)realloc(this->words, word_count * sizeof(Word*));
    }

    ~Sentence() {
        if (this->words) {
            for (int i = 0; i < this->word_count; i++) {
                if (this->words[i]) {
                    if (this->words[i]->data) free(this->words[i]->data);
                    free(this->words[i]);
                }
            }
            free(this->words);
        }
        free(this->data);
    }
};

class CompoundSentence {
public:
    char *data;

    CompoundSentence(Sentence* a, Sentence* b) {
        int total_length = 0;

        Word **ws1 = a->get_words();
        int n1 = a->get_word_count();
        Word **ws2 = b->get_words();
        int n2 = b->get_word_count();

        for (int i = 0; i < n1; i++) {
            total_length += ws1[i]->length;
        }
        for (int i = 0; i < n2; i++) {
            total_length += ws2[i]->length;
        }

        if (n1 > 0) total_length += n1 - 1;
        if (n2 > 0) total_length += n2 - 1;

        int max_len = (n1 > n2) ? n1 : n2;

        this->data = (char*)malloc(total_length + 1);
        assert(this->data != nullptr);
        this->data[0] = '\0';

        for (int k = 0; k < max_len; k++) {
            if (n1 > 0) {
                strcat(this->data, ws1[k % n1]->data);
                if (!(k == max_len - 1 && n2 == 0)) {
                    strcat(this->data, " ");
                }
            }
            if (n2 > 0) {
                strcat(this->data, ws2[k % n2]->data);
                if (k != max_len - 1) {
                    strcat(this->data, " ");
                }
            }
        }
    }

    ~CompoundSentence() {
        free(this->data);
    }
};

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
    Sentence first(DEFAULT_MAX_LENGTH);

    printf("Введите второе предложение: ");
    Sentence second(DEFAULT_MAX_LENGTH);

    CompoundSentence cs(&first, &second);
    printf("\n\nРезультат: %s\n", cs.data);

    return 0;
}
