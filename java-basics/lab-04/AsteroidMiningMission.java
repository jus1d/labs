import java.util.Arrays;

class AsteroidMiningMission implements SpaceMission {

    private int[] mineralQuantities;
    private String missionName;
    private int equipmentMaintenanceCost;

    public AsteroidMiningMission(
        int[] mineralQuantities,
        String missionName,
        int equipmentMaintenanceCost
    ) {
        if (mineralQuantities == null) {
            throw new MissionValidationException(
                "Массив минералов не может быть пустым"
            );
        }

        if (mineralQuantities.length == 0) {
            throw new MissionValidationException(
                "Миссия должна добывать хотя бы один астероид"
            );
        }

        for (int quantity : mineralQuantities) {
            if (quantity < 0) {
                throw new MissionValidationException(
                    "Количество минералов не может быть отрицательным: " +
                        quantity
                );
            }
        }

        if (missionName == null || missionName.trim().isEmpty()) {
            throw new MissionValidationException(
                "Название миссии не может быть пустым"
            );
        }

        if (equipmentMaintenanceCost < 0) {
            throw new MissionValidationException(
                "Стоимость обслуживания оборудования не может быть отрицательной: " +
                    equipmentMaintenanceCost
            );
        }

        this.mineralQuantities = mineralQuantities.clone();
        this.missionName = missionName;
        this.equipmentMaintenanceCost = equipmentMaintenanceCost;
    }

    @Override
    public int[] getResourceData() {
        return mineralQuantities.clone();
    }

    @Override
    public void setResourceData(int[] resourceData) {
        setMineralQuantities(resourceData);
    }

    public void setMineralQuantities(int[] mineralQuantities) {
        if (mineralQuantities == null) {
            throw new MissionValidationException(
                "Массив количеств минералов не может быть null"
            );
        }
        if (mineralQuantities.length == 0) {
            throw new MissionValidationException(
                "Миссия должна добывать хотя бы один астероид"
            );
        }
        for (int quantity : mineralQuantities) {
            if (quantity < 0) {
                throw new MissionValidationException(
                    "Количество минералов не может быть отрицательным: " +
                        quantity
                );
            }
        }
        this.mineralQuantities = mineralQuantities.clone();
    }

    @Override
    public int getArrayElement(int index) {
        if (index < 0 || index >= mineralQuantities.length) {
            throw new MissionValidationException(
                "Неверный индекс астероида: " + index
            );
        }
        return mineralQuantities[index];
    }

    @Override
    public void setArrayElement(int index, int value) {
        if (index < 0 || index >= mineralQuantities.length) {
            throw new MissionValidationException(
                "Неверный индекс астероида: " + index
            );
        }
        if (value < 0) {
            throw new MissionValidationException(
                "Количество минералов не может быть отрицательным: " + value
            );
        }
        mineralQuantities[index] = value;
    }

    @Override
    public int size() {
        return mineralQuantities.length;
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
        return equipmentMaintenanceCost;
    }

    @Override
    public void setCostPerUnit(int costPerUnit) {
        setEquipmentMaintenanceCost(costPerUnit);
    }

    public void setEquipmentMaintenanceCost(int equipmentMaintenanceCost) {
        if (equipmentMaintenanceCost < 0) {
            throw new MissionValidationException(
                "Стоимость обслуживания оборудования не может быть отрицательной: " +
                    equipmentMaintenanceCost
            );
        }
        this.equipmentMaintenanceCost = equipmentMaintenanceCost;
    }

    @Override
    public int calculateNetProfit() throws MissionBusinessException {
        if (equipmentMaintenanceCost < 0) {
            throw new MissionBusinessException(
                "Неверная стоимость обслуживания оборудования: " +
                    equipmentMaintenanceCost
            );
        }

        int totalMinerals = 0;
        for (int quantity : mineralQuantities) {
            if (quantity < 0) {
                throw new MissionBusinessException(
                    "Найдено неверное количество минералов: " + quantity
                );
            }
            totalMinerals += quantity;
        }

        int mineralValue = totalMinerals * 2;
        int totalMaintenanceCost =
            equipmentMaintenanceCost * mineralQuantities.length;
        int netProfit = mineralValue - totalMaintenanceCost;

        if (netProfit < 0) {
            throw new MissionBusinessException(
                "Миссия добычи не прибыльна! Чистый убыток: " +
                    Math.abs(netProfit)
            );
        }

        return netProfit;
    }

    @Override
    public String toString() {
        return (
            "Миссия добычи астероидов: '" +
            missionName +
            "', астероиды: " +
            mineralQuantities.length +
            ", стоимость обслуживания за астероид: " +
            equipmentMaintenanceCost +
            ", количества минералов: " +
            Arrays.toString(mineralQuantities)
        );
    }

    @Override
    public boolean equals(Object obj) {
        if (this == obj) return true;
        if (obj == null || getClass() != obj.getClass()) return false;
        AsteroidMiningMission that = (AsteroidMiningMission) obj;
        return (
            equipmentMaintenanceCost == that.equipmentMaintenanceCost &&
            Arrays.equals(mineralQuantities, that.mineralQuantities) &&
            missionName.equals(that.missionName)
        );
    }

    @Override
    public int hashCode() {
        int result = Arrays.hashCode(mineralQuantities);
        result = 31 * result + missionName.hashCode();
        result = 31 * result + equipmentMaintenanceCost;
        return result;
    }
}
