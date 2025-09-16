#include <cstdio>
#include <cstring>
#include <cstdlib>

using namespace std;

#define MAX_LENGTH 80
#define endl '\n';

class Sentence {
private:
    char* text;

public:
    char** words;
    int word_count;

    Sentence(size_t max_length) {
        text = (char*)malloc(max_length + 1);
        size_t i = 0;
        char c;
        while (i < max_length && (c = getchar()) != '\n') {
            text[i++] = c;
        }
        text[i] = '\0';

        split_words();
        sort_words();
    }

    void split_words() {
        words = (char**)malloc(40 * sizeof(char*));
        word_count = 0;
        char* token = strtok(text, " ");
        while (token != nullptr) {
            words[word_count++] = strdup(token);
            token = strtok(nullptr, " ");
        }
    }

    void sort_words() {
        for (int i = 0; i < word_count - 1; i++) {
            for (int j = i + 1; j < word_count; j++) {
                if (strlen(words[i]) > strlen(words[j])) {
                    char* tmp = words[i];
                    words[i] = words[j];
                    words[j] = tmp;
                }
            }
        }
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

class SentenceBuilder {
private:
    Sentence* first;
    Sentence* second;
    int total_length;

public:
    SentenceBuilder(Sentence* s1, Sentence* s2) : first(s1), second(s2) {
        char **w1 = first->words;
        int n1 = first->word_count;
        char **w2 = second->words;
        int n2 = second->word_count;

        for (int i = 0; i < n1; i++) {
            total_length += strlen(w1[i]);
        }
        for (int i = 0; i < n2; i++) {
            total_length += strlen(w2[i]);
        }

        if (n1 > 0) total_length += n1 - 1;
        if (n2 > 0) total_length += n2 - 1;
    }

    char* build_sentence() {
        int i = 0, j = 0, k = 0;
        int n1 = first->word_count;
        int n2 = second->word_count;
        int max_len = (n1 > n2 ? n1 : n2);
        char **w1 = first->words;
        char **w2 = second->words;

        char* result = (char*)malloc(total_length + 1);
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
};

int main() {
    printf("Введите первое предложение: ");
    Sentence first(MAX_LENGTH);

    printf("Введите второе предложение: ");
    Sentence second(MAX_LENGTH);

    SentenceBuilder sb(&first, &second);
    char *sentence = sb.build_sentence();
    printf("Результат: %s\n", sentence);
    free(sentence);

    return 0;
}
