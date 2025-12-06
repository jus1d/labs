SOURCES = AsteroidMiningMission.java \
          Comparators.java \
          Main.java \
          MissionBusinessException.java \
          MissionDatabaseGUI.java \
          PlanetExplorationMission.java \
          ReaderRunnable.java \
          ReaderThread.java \
          Semaphore.java \
          SpaceMission.java \
          SpaceMissionFactory.java \
          StreamManager.java \
          SynchronizedSpaceMission.java \
          UnmodifiableSpaceMission.java \
          WriterRunnable.java \
          WriterThread.java

all: compile

compile: $(SOURCES)
	javac $(SOURCES)

run: compile
	java Main
	$(MAKE) clean

gui: compile
	java MissionDatabaseGUI
	$(MAKE) clean

clean:
	rm -f *.class

.PHONY: all compile run gui clean
