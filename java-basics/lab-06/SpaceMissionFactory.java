interface SpaceMissionFactory {
    SpaceMission createInstance();
    SpaceMission createInstance(
        int[] resourceData,
        String missionName,
        int costPerUnit
    );
}

class PlanetExplorationMissionFactory implements SpaceMissionFactory {

    @Override
    public SpaceMission createInstance() {
        return new PlanetExplorationMission(
            new int[] { 100, 150, 200 },
            "DefaultPlanetMission",
            10
        );
    }

    @Override
    public SpaceMission createInstance(
        int[] resourceData,
        String missionName,
        int costPerUnit
    ) {
        return new PlanetExplorationMission(
            resourceData,
            missionName,
            costPerUnit
        );
    }
}

class AsteroidMiningMissionFactory implements SpaceMissionFactory {

    @Override
    public SpaceMission createInstance() {
        return new AsteroidMiningMission(
            new int[] { 50, 75, 100 },
            "DefaultAsteroidMission",
            5
        );
    }

    @Override
    public SpaceMission createInstance(
        int[] resourceData,
        String missionName,
        int costPerUnit
    ) {
        return new AsteroidMiningMission(
            resourceData,
            missionName,
            costPerUnit
        );
    }
}
