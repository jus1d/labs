import java.io.*;
import java.util.Iterator;

public class SynchronizedSpaceMission implements SpaceMission {

    private SpaceMission mission;

    public SynchronizedSpaceMission(SpaceMission mission) {
        this.mission = mission;
    }

    @Override
    public synchronized int[] getResourceData() {
        return mission.getResourceData();
    }

    @Override
    public synchronized void setResourceData(int[] resourceData) {
        mission.setResourceData(resourceData);
    }

    @Override
    public synchronized int getArrayElement(int index)
        throws MissionValidationException {
        return mission.getArrayElement(index);
    }

    @Override
    public synchronized void setArrayElement(int index, int value)
        throws MissionValidationException {
        mission.setArrayElement(index, value);
    }

    @Override
    public synchronized int size() {
        return mission.size();
    }

    @Override
    public synchronized String getMissionName() {
        return mission.getMissionName();
    }

    @Override
    public synchronized void setMissionName(String missionName)
        throws MissionValidationException {
        mission.setMissionName(missionName);
    }

    @Override
    public synchronized int getCostPerUnit() {
        return mission.getCostPerUnit();
    }

    @Override
    public synchronized void setCostPerUnit(int costPerUnit) {
        mission.setCostPerUnit(costPerUnit);
    }

    @Override
    public synchronized int calculateNetProfit() throws MissionBusinessException {
        return mission.calculateNetProfit();
    }

    @Override
    public synchronized void output(OutputStream out) throws IOException {
        mission.output(out);
    }

    @Override
    public synchronized void write(Writer out) throws IOException {
        mission.write(out);
    }

    @Override
    public synchronized String toString() {
        return mission.toString();
    }

    @Override
    public synchronized boolean equals(Object obj) {
        return mission.equals(obj);
    }

    @Override
    public synchronized int hashCode() {
        return mission.hashCode();
    }

    @Override
    public synchronized int compareTo(SpaceMission other) {
        return mission.compareTo(other);
    }

    @Override
    public synchronized Iterator<Integer> iterator() {
        return mission.iterator();
    }
}
