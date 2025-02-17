#include <stdio.h>
#include <stdlib.h>
#include <stdint.h>
#include <time.h>

// #define CACHED

#ifdef CACHED
uint64_t *cache;
#endif

uint64_t rfib(int n) {
    if (n <= 1) return n;

#ifdef CACHED
    if (cache[n] != 0) {
        return cache[n];
    }

    cache[n] = rfib(n - 1) + rfib(n - 2);
    return cache[n];
#else
    return rfib(n - 1) + rfib(n - 2);
#endif
}

uint64_t fib(int n) {
    if (n == 0) return 0;

    uint64_t a = 0;
    uint64_t b = 1;

    uint64_t tmp;
    for (size_t i = 2; i <= n; ++i) {
        tmp = a + b;
        a = b;
        b = tmp;
    }

    return b;
}

int main(int argc, char **argv) {
    int n = 20;
    if (argc >= 2) {
        n = atoi(argv[1]);
    }

#ifdef CACHED
    cache = (uint64_t *)calloc(n + 1, sizeof(uint64_t));
    if (cache == NULL) {
        fprintf(stderr, "Memory allocation failed\n");
        return 1;
    }
#endif

    clock_t timer_start, timer_end;
    double millis;
    uint64_t fibn;

    timer_start = clock();
    fibn = fib(n);
    timer_end = clock();
    millis = (timer_end - timer_start) / (double) CLOCKS_PER_SEC * 1000;
    printf("fib(%d) = %llu, took %fms iteratively\n", n, fibn, millis);

    timer_start = clock();
    fibn = rfib(n);
    timer_end = clock();
    millis = (timer_end - timer_start) / (double) CLOCKS_PER_SEC * 1000;
#ifdef CACHED
    printf("fib(%d) = %llu, took %fms recursively + cached\n", n, fibn, millis);
#else
    printf("fib(%d) = %llu, took %fms recursively\n", n, fibn, millis);
#endif

#ifdef CACHED
    free(cache);
#endif

    return 0;
}
