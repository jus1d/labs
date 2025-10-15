interface SpaceMission {
    int[] getResourceData();
    void setResourceData(int[] resourceData);

    int get(int index) throws MissionValidationException;
    void set(int index, int value) throws MissionValidationException;
    int size();

    String getMissionName();
    void setMissionName(String missionName) throws MissionValidationException;
    int getCostPerUnit();
    void setCostPerUnit(int costPerUnit);

    int calculateNetProfit() throws MissionBusinessException;
}
