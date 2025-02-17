#include <stdio.h>
#include <stdlib.h>
#include <time.h>

// #define DEBUG

void log_array(int *data, int size) {
    for (size_t i = 0; i < size; ++i) {
        printf("%d ", data[i]);
    }
    printf("\n");
}

int *generate_array(int size) {
    int* data = malloc(sizeof(int) * size);
    if (data == NULL) {
        printf("Buy more RAM :)");
        exit(1);
    }

    for (size_t i = 0; i < size; ++i) {
        data[i] = rand() % 100;
    }
    return data;
}

void bubblesort(int *data, int size) {
    for (size_t i = 0; i < size; ++i) {
        for (size_t j = i + 1; j < size; ++j) {
            if (data[i] > data[j]) {
                int tmp = data[i];
                data[i] = data[j];
                data[j] = tmp;
            }
        }
    }
}

void quicksort(int *data, int l, int r) {
    if (l > r) return;

    int i = l;
    int j = r;
    int pivot = data[(l + r) / 2];

    while (i <= j) {
        while (data[i] < pivot) i++;
        while (data[j] > pivot) j--;

        if (i <= j) {
            int tmp = data[i];
            data[i] = data[j];
            data[j] = tmp;

            i++;
            j--;
        }
    }

    quicksort(data, l, j);
    quicksort(data, i, r);
}

int main(void) {
    srand(time(NULL));
    const int SIZE = 100000;

    int *data = generate_array(SIZE);

#ifdef DEBUG
    printf("before sorting: ");
    log_array(data, SIZE);
#endif

    clock_t timer_start = clock();
    // bubblesort(data, SIZE);
    quicksort(data, 0, SIZE - 1);
    clock_t timer_end = clock();

#ifdef DEBUG
    printf("after sorting: ");
    log_array(data, SIZE);
#endif

    double millis = (timer_end - timer_start) / (double) CLOCKS_PER_SEC * 1000;
    printf("Sorting took %fms\n", millis);

    free(data);

    return 0;
}
