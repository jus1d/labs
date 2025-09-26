run: build FirstArchive.jar
	java -jar ./FirstArchive.jar

interpret: build
	java -classpath . FirstClass

build:
	javac FirstPackage/SecondClass.java FirstProgram.java

FirstArchive.jar: manifest.txt FirstPackage/SecondClass.class FirstClass.class
	jar cfm FirstArchive.jar manifest.txt FirstPackage/*.class FirstClass.class

clean:
	rm -f FirstPackage/*.class *.class *.jar

.PHONY: build run clean
