all: run clean

test:
	javac *.java
	java Test
	$(MAKE) clean

run: AsteroidMiningMission.java MissionBusinessException.java MissionManager.java PlanetExplorationMission.java SpaceMission.java
	javac *.java
	java MissionManager

clean:
	rm -f *.class

.PHONY: run clean
