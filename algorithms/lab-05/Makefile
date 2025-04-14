CC = clang++
CFLAGS = -Wall -Wextra --pedantic -std=c++17

all: huffman subpalindromes substring

huffman: huffman.cpp
	$(CC) $(CFLAGS) -o huffman huffman.cpp

subpalindromes: subpalindromes.cpp
	$(CC) $(CFLAGS) -o subpalindromes subpalindromes.cpp

substring: substring.cpp
	$(CC) $(CFLAGS) -o substring substring.cpp

clean:
	rm -f huffman subpalindromes substring

.PHONY: all clean
