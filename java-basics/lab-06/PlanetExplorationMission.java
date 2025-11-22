import java.io.*;
import java.util.Arrays;
import java.util.Iterator;

class PlanetExplorationMission implements SpaceMission, Serializable {

    private static final long serialVersionUID = 1L;

    private int[] resourceYields;
    private String missionName;
    private int fuelCostPerPlanet;

    public PlanetExplorationMission(
        int[] resourceYields,
        String missionName,
        int fuelCostPerPlanet
    ) {
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

        if (missionName == null || missionName.trim().isEmpty()) {
            throw new MissionValidationException(
                "Название миссии не может быть пустым"
            );
        }

        if (fuelCostPerPlanet < 0) {
            throw new MissionValidationException(
                "Стоимость топлива не может быть отрицательной: " +
                    fuelCostPerPlanet
            );
        }

        this.resourceYields = resourceYields.clone();
        this.missionName = missionName;
        this.fuelCostPerPlanet = fuelCostPerPlanet;
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
    public int getArrayElement(int index) {
        if (index < 0 || index >= resourceYields.length) {
            throw new MissionValidationException(
                "Неверный индекс планеты: " + index
            );
        }
        return resourceYields[index];
    }

    @Override
    public void setArrayElement(int index, int value) {
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

    @Override
    public void output(OutputStream out) throws IOException {
        DataOutputStream dos = new DataOutputStream(out);

        dos.writeUTF(missionName);
        dos.writeInt(fuelCostPerPlanet);
        dos.writeInt(resourceYields.length);
        for (int yield : resourceYields) {
            dos.writeInt(yield);
        }

        dos.flush();
    }

    @Override
    public void write(Writer out) throws IOException {
        out.write(missionName + " ");
        out.write(fuelCostPerPlanet + " ");
        out.write(resourceYields.length + " ");

        for (int i = 0; i < resourceYields.length; i++) {
            out.write(resourceYields[i] + "");
            if (i < resourceYields.length - 1) {
                out.write(" ");
            }
        }
        out.write("\n");
        out.flush();
    }

    @Override
    public int compareTo(SpaceMission other) {
        try {
            return Integer.compare(
                this.calculateNetProfit(),
                other.calculateNetProfit()
            );
        } catch (MissionBusinessException e) {
            throw new RuntimeException(e);
        }
    }

    @Override
    public Iterator<Integer> iterator() {
        return new Iterator<>() {
            private int idx = 0;

            @Override
            public boolean hasNext() {
                return idx < resourceYields.length;
            }

            @Override
            public Integer next() {
                return resourceYields[idx++];
            }
        };
    }
}
