run: build
	./main

build: ./main.cpp ./calculate.asm ./calculate.h
	nasm -felf64 ./calculate.asm -o calculate.o
	clang++ -c ./main.cpp -o main.o
	clang++ ./main.o ./calculate.o -o main