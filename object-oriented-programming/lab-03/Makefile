all: build/legacy build/main

build/legacy: legacy/main.cpp
	clang++ -Wall -Wextra --pedantic -o build/legacy legacy/main.cpp

build/main: src/main.cpp src/database.cpp src/ui.cpp
	clang++ -Wall -Wextra --pedantic -o build/main src/main.cpp

clean:
	rm -f *.o build/main build/legacy *.dat *.idx

.PHONY: all clean
