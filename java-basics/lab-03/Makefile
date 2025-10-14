all: run clean

run: InvalidAlbumDataException.java InvalidDurationException.java LiveAlbum.java MusicCollection.java MusicCollectionApp.java StudioAlbum.java
	javac *.java
	java MusicCollectionApp

clean:
	rm -f *.class

.PHONY: run clean
