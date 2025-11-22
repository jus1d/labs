SOURCES = AsteroidMiningMission.java \
          Comparators.java \
          Main.java \
          MissionBusinessException.java \
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

run: $(SOURCES)
	javac *.java
	java Main
	$(MAKE) clean

clean:
	rm -f *.class

.PHONY: all run clean
