CC = cc
CFLAGS = -Wall -Werror -pedantic
EXECUTABLES = main

all: $(EXECUTABLES)

clean:
	rm -f $(EXECUTABLES)

main: main.c
	$(CC) $(CFLAGS) -o main main.c


.PHONY: all clean
