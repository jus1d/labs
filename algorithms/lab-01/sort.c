#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <time.h>

#define OUTPUT_MAX_SIZE 20

void log_array(int *data, int size) {
    for (size_t i = 0; i < size; ++i) {
        printf("%d ", data[i]);
    }
    printf("\n");
}

void fill_random(int *a, int n, int upper) {
    for (size_t i = 0; i < n; ++i) {
        a[i] = rand() % (upper + 1);
    }
}

void swap(int *a, int *b) {
    int tmp = *a;
    *a = *b;
    *b = tmp;
}

void shuffle(int *array, int n) {
    for (int i = n - 1; i > 0; i--) {
        int j = rand() % (i + 1);
        swap(&array[i], &array[j]);
    }
}

void bubblesort(int *data, int size) {
    for (size_t i = 0; i < size; ++i) {
        for (size_t j = i + 1; j < size; ++j) {
            if (data[i] > data[j]) {
                swap(&data[i], &data[j]);
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
            swap(&data[i], &data[j]);

            i++;
            j--;
        }
    }

    quicksort(data, l, j);
    quicksort(data, i, r);
}

void merge(int arr[], int l, int mid, int r) {
    int n1 = mid - l + 1;
    int n2 = r - mid;

    int left[n1], right[n2];

    for (int i = 0; i < n1; i++)
        left[i] = arr[l + i];

    for (int j = 0; j < n2; j++)
        right[j] = arr[mid + 1 + j];

    int i = 0, j = 0, k = l;
    while (i < n1 && j < n2) {
        if (left[i] <= right[j]) {
            arr[k] = left[i];
            i++;
        } else {
            arr[k] = right[j];
            j++;
        }
        k++;
    }

    while (i < n1) {
        arr[k] = left[i];
        i++;
        k++;
    }
    while (j < n2) {
        arr[k] = right[j];
        j++;
        k++;
    }
}

void merge_sort(int arr[], int l, int r) {
    if (l < r) {
        int mid = l + (r - l) / 2;

        merge_sort(arr, l, mid);
        merge_sort(arr, mid + 1, r);

        merge(arr, l, mid, r);
    }
}

void measure_bubblesort(int a[], int n) {
    if (n <= OUTPUT_MAX_SIZE) {
        printf("[bubblesort] Initial array: ");
        log_array(a, n);
    }

    clock_t timer_start = clock();
    bubblesort(a, n);
    clock_t timer_end = clock();

    if (n <= OUTPUT_MAX_SIZE) {
        printf("[bubblesort] Sorted array:  ");
        log_array(a, n);
    }

    double millis = (double)(timer_end - timer_start) * 1000 / CLOCKS_PER_SEC;
    printf("[bubblesort] Sorting took %fms\n", millis);
}

void measure_quicksort(int a[], int n) {
    if (n <= OUTPUT_MAX_SIZE) {
        printf("[quicksort] Initial array: ");
        log_array(a, n);
    }

    clock_t timer_start = clock();
    quicksort(a, 0, n-1);
    clock_t timer_end = clock();

    if (n <= OUTPUT_MAX_SIZE) {
        printf("[quicksort] Sorted array:  ");
        log_array(a, n);
    }

    double millis = (double)(timer_end - timer_start) * 1000 / CLOCKS_PER_SEC;
    printf("[quicksort] Sorting took %fms\n", millis);
}

void measure_mergesort(int a[], int n) {
    if (n <= OUTPUT_MAX_SIZE) {
        printf("[mergesort] Initial array: ");
        log_array(a, n);
    }

    clock_t timer_start = clock();
    merge_sort(a, 0, n-1);
    clock_t timer_end = clock();

    if (n <= OUTPUT_MAX_SIZE) {
        printf("[mergesort] Sorted array:  ");
        log_array(a, n);
    }

    double millis = (double)(timer_end - timer_start) * 1000 / CLOCKS_PER_SEC;
    printf("[mergesort] Sorting took %fms\n", millis);
}

int main(int argc, char **argv) {
    srand(time(NULL));

    int n = 10;
    if (argc > 1) {
        n = atoi(argv[1]);
    }

    int *data1 = (int *)malloc(n * sizeof(int));
    int *data2 = (int *)malloc(n * sizeof(int));
    int *data3 = (int *)malloc(n * sizeof(int));

    if (argc > 2 && strcmp(argv[2], "rand") == 0) {
        fill_random(data1, n, 100);
    } else {
        for (int i = 0; i < n; i++) {
            data1[i] = i;
        }

        shuffle(data1, n);
    }

    for (int i = 0; i < n; i++) {
        data2[i] = data1[i];
        data3[i] = data1[i];
    }

    measure_quicksort(data1, n);
    if (n <= OUTPUT_MAX_SIZE) printf("\n");
    measure_mergesort(data3, n);
    if (n <= OUTPUT_MAX_SIZE) printf("\n");
    measure_bubblesort(data2, n);

    free(data1);
    free(data2);
    free(data3);

    return 0;
}
