#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <time.h>

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

void swap(int *a, int *b) {
    int tmp = *a;
    *a = *b;
    *b = tmp;
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

int main(int argc, char **argv) {
    srand(time(NULL));
    const int OUTPUT_MAX_SIZE = 20;

    int size;
    printf("Enter array length (output only for N < %d) => ", OUTPUT_MAX_SIZE + 1);
    scanf("%d", &size);

    char *sort = "quicksort";
    if (argc > 1) {
        sort = argv[1];
    }

    int *data = generate_array(size);

    if (size <= OUTPUT_MAX_SIZE) {
        printf("Initial array: ");
        log_array(data, size);
    }

    clock_t timer_start = clock();
    if (strcmp(sort, "quicksort") == 0) {
        quicksort(data, 0, size - 1);
    }
    else if (strcmp(sort, "bubblesort") == 0) {
        bubblesort(data, size);
    }
    else if (strcmp(sort, "mergesort") == 0) {
        merge_sort(data, 0, size -1);
    }
    else {
        printf("Unknown sorting method: `%s`\n", sort);
        exit(1);
    }
    clock_t timer_end = clock();

    if (size <= OUTPUT_MAX_SIZE) {
        printf("Sorted array: ");
        log_array(data, size);
    }

    free(data);

    double millis = (timer_end - timer_start) / (double) CLOCKS_PER_SEC * 1000;
    printf("[%s] Sorting took %fms\n", sort, millis);

    return 0;
}
