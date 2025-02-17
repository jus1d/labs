CC = cc
CFLAGS = -Wall -Werror -pedantic
EXECUTABLES = fib

all: $(EXECUTABLES)

clean:
	rm -f $(EXECUTABLES)

sort: sort.c
	$(CC) $(CFLAGS) -o fib fib.c


.PHONY: all clean
