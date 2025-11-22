import java.io.*;

interface SpaceMission {
    int[] getResourceData();
    void setResourceData(int[] resourceData);

    int getArrayElement(int index) throws MissionValidationException;
    void setArrayElement(int index, int value)
        throws MissionValidationException;
    int size();

    String getMissionName();
    void setMissionName(String missionName) throws MissionValidationException;
    int getCostPerUnit();
    void setCostPerUnit(int costPerUnit);

    int calculateNetProfit() throws MissionBusinessException;

    void output(OutputStream out) throws IOException;
    void write(Writer out) throws IOException;
}
