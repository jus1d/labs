#include <cstdio>
#include <algorithm>
#include <cstring>
#include <cstdlib>
#include <assert.h>

using namespace std;

#define MAX_LENGTH 80
#define endl '\n';

class Sentence {
private:
    char* text;

public:
    char** words;
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
        text = (char*)realloc(text, i);

        split_words();
        std::sort(words, words + word_count, [](const char* a, const char* b) {
            return strlen(a) < strlen(b);
        });
    }

    void split_words() {
        size_t max_words = sentence_max_length / 2;
        words = (char**)malloc(max_words * sizeof(char*));
        assert(words != nullptr);
        word_count = 0;
        char* token = strtok(text, " ");
        while (token != nullptr) {
            words[word_count++] = strdup(token);
            token = strtok(nullptr, " ");
        }
        words = (char**)realloc(words, word_count * sizeof(char*));
    }

    ~Sentence() {
        if (words) {
            for (int i = 0; i < word_count; i++) {
                free(words[i]);
            }
            free(words);
        }
        free(text);
    }
};

size_t compute_total_length(Sentence *a, Sentence *b) {
    char **w1 = a->words;
    int n1 = a->word_count;
    char **w2 = b->words;
    int n2 = b->word_count;
    size_t total_length = 0;

    for (int i = 0; i < n1; i++) {
        total_length += strlen(w1[i]);
    }
    for (int i = 0; i < n2; i++) {
        total_length += strlen(w2[i]);
    }

    if (n1 > 0) total_length += n1 - 1;
    if (n2 > 0) total_length += n2 - 1;

    return total_length;
}

char* build_sentence(Sentence *a, Sentence *b) {
    int i = 0, j = 0, k = 0;
    int n1 = a->word_count;
    int n2 = b->word_count;
    int max_len = (n1 > n2 ? n1 : n2);
    char **w1 = a->words;
    char **w2 = b->words;
    size_t total_length = compute_total_length(a, b);

    char* result = (char*)malloc(total_length + 1);
    assert(result != nullptr);
    result[0] = '\0';

    i = j = k = 0;
    while (k < max_len * 2) {
        if (k % 2 == 0) {
            strcat(result, w1[i]);
            i = (i + 1) % n1;
        } else {
            strcat(result, w2[j]);
            j = (j + 1) % n2;
        }
        k++;
        if ((i == 0 && j == 0 && k > 0) || (k >= n1 + n2)) break;
        strcat(result, " ");
    }

    return result;
}

int main() {
    printf("Введите первое предложение: ");
    Sentence first(MAX_LENGTH);

    printf("Введите второе предложение: ");
    Sentence second(MAX_LENGTH);

    char *sentence = build_sentence(&first, &second);
    printf("Результат: %s\n", sentence);
    free(sentence);

    return 0;
}
