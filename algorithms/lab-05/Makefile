CC = clang++
CFLAGS = -Wall -Wextra --pedantic -std=c++17

all: huffman palindromes substring graph

huffman: huffman.cpp
	$(CC) $(CFLAGS) -o huffman huffman.cpp

palindromes: palindromes.cpp
	$(CC) $(CFLAGS) -o palindromes palindromes.cpp

substring: substring.cpp
	$(CC) $(CFLAGS) -o substring substring.cpp

graph: graph.cpp
	$(CC) $(CFLAGS) -o graph graph.cpp

clean:
	rm -f huffman palindromes substring graph

.PHONY: all clean
