CC = cc
CFLAGS = -Wall -Werror -pedantic
EXECUTABLES = sort

all: sort

clean:
	rm -f $(EXECUTABLES)

sort: sort.c
	$(CC) $(CFLAGS) -o sort sort.c


.PHONY: all clean
