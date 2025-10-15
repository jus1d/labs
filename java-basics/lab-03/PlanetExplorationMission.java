import java.util.Arrays;

class PlanetExplorationMission implements SpaceMission {

    private int[] resourceYields;
    private String missionName;
    private int fuelCostPerPlanet;

    public PlanetExplorationMission() {
        this.resourceYields = new int[0];
        this.missionName = "Безымянная миссия";
        this.fuelCostPerPlanet = 0;
    }

    public PlanetExplorationMission(
        int[] resourceYields,
        String missionName,
        int fuelCostPerPlanet
    ) {
        setResourceYields(resourceYields);
        setMissionName(missionName);
        setFuelCostPerPlanet(fuelCostPerPlanet);
    }

    @Override
    public int[] getResourceData() {
        return resourceYields.clone();
    }

    @Override
    public void setResourceData(int[] resourceData) {
        setResourceYields(resourceData);
    }

    public void setResourceYields(int[] resourceYields) {
        if (resourceYields == null) {
            throw new MissionValidationException(
                "Массив доходности ресурсов не может быть null"
            );
        }
        if (resourceYields.length == 0) {
            throw new MissionValidationException(
                "Миссия должна исследовать хотя бы одну планету"
            );
        }
        for (int yield : resourceYields) {
            if (yield < 0) {
                throw new MissionValidationException(
                    "Доходность ресурсов не может быть отрицательной: " + yield
                );
            }
        }
        this.resourceYields = resourceYields.clone();
    }

    @Override
    public int get(int index) {
        if (index < 0 || index >= resourceYields.length) {
            throw new MissionValidationException(
                "Неверный индекс планеты: " + index
            );
        }
        return resourceYields[index];
    }

    @Override
    public void set(int index, int value) {
        if (index < 0 || index >= resourceYields.length) {
            throw new MissionValidationException(
                "Неверный индекс планеты: " + index
            );
        }
        if (value < 0) {
            throw new MissionValidationException(
                "Доходность ресурсов не может быть отрицательной: " + value
            );
        }
        resourceYields[index] = value;
    }

    @Override
    public int size() {
        return resourceYields.length;
    }

    @Override
    public String getMissionName() {
        return missionName;
    }

    @Override
    public void setMissionName(String missionName) {
        if (missionName == null || missionName.trim().isEmpty()) {
            throw new MissionValidationException(
                "Название миссии не может быть пустым"
            );
        }
        this.missionName = missionName;
    }

    @Override
    public int getCostPerUnit() {
        return fuelCostPerPlanet;
    }

    @Override
    public void setCostPerUnit(int costPerUnit) {
        setFuelCostPerPlanet(costPerUnit);
    }

    public void setFuelCostPerPlanet(int fuelCostPerPlanet) {
        if (fuelCostPerPlanet < 0) {
            throw new MissionValidationException(
                "Стоимость топлива не может быть отрицательной: " +
                    fuelCostPerPlanet
            );
        }
        this.fuelCostPerPlanet = fuelCostPerPlanet;
    }

    @Override
    public int calculateNetProfit() throws MissionBusinessException {
        if (fuelCostPerPlanet < 0) {
            throw new MissionBusinessException(
                "Неверная стоимость топлива за планету: " + fuelCostPerPlanet
            );
        }

        int totalResources = 0;
        for (int yield : resourceYields) {
            if (yield < 0) {
                throw new MissionBusinessException(
                    "Найдена неверная доходность ресурсов: " + yield
                );
            }
            totalResources += yield;
        }

        int totalFuelCost = fuelCostPerPlanet * resourceYields.length;
        int netProfit = totalResources - totalFuelCost;

        if (netProfit < 0) {
            throw new MissionBusinessException(
                "Миссия не прибыльна! Чистый убыток: " + Math.abs(netProfit)
            );
        }

        return netProfit;
    }

    @Override
    public String toString() {
        return (
            "Миссия исследования планет: '" +
            missionName +
            "', планеты: " +
            resourceYields.length +
            ", стоимость топлива за планету: " +
            fuelCostPerPlanet +
            ", доходности ресурсов: " +
            Arrays.toString(resourceYields)
        );
    }

    @Override
    public boolean equals(Object obj) {
        if (this == obj) return true;
        if (obj == null || getClass() != obj.getClass()) return false;
        PlanetExplorationMission that = (PlanetExplorationMission) obj;
        return (
            fuelCostPerPlanet == that.fuelCostPerPlanet &&
            Arrays.equals(resourceYields, that.resourceYields) &&
            missionName.equals(that.missionName)
        );
    }

    @Override
    public int hashCode() {
        int result = Arrays.hashCode(resourceYields);
        result = 31 * result + missionName.hashCode();
        result = 31 * result + fuelCostPerPlanet;
        return result;
    }
}
