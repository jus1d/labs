import java.io.*;
import java.util.Iterator;

public class UnmodifiableSpaceMission implements SpaceMission {

    private final SpaceMission inner;

    public UnmodifiableSpaceMission(SpaceMission inner) {
        this.inner = inner;
    }

    @Override
    public int[] getResourceData() {
        return inner.getResourceData();
    }

    @Override
    public int getArrayElement(int index) throws MissionValidationException {
        return inner.getArrayElement(index);
    }

    @Override
    public int size() {
        return inner.size();
    }

    @Override
    public String getMissionName() {
        return inner.getMissionName();
    }

    @Override
    public int getCostPerUnit() {
        return inner.getCostPerUnit();
    }

    @Override
    public int calculateNetProfit() throws MissionBusinessException {
        return inner.calculateNetProfit();
    }

    @Override
    public void output(OutputStream out) throws IOException {
        inner.output(out);
    }

    @Override
    public void write(Writer out) throws IOException {
        inner.write(out);
    }

    @Override
    public int compareTo(SpaceMission other) {
        return inner.compareTo(other);
    }

    @Override
    public Iterator<Integer> iterator() {
        return inner.iterator();
    }

    @Override
    public String toString() {
        return inner.toString();
    }

    @Override
    public boolean equals(Object obj) {
        return inner.equals(obj);
    }

    @Override
    public int hashCode() {
        return inner.hashCode();
    }

    @Override
    public void setResourceData(int[] resourceData) {
        throw new UnsupportedOperationException(
            "Невозможно изменить неизменяемый объект SpaceMission"
        );
    }

    @Override
    public void setArrayElement(int index, int value)
        throws MissionValidationException {
        throw new UnsupportedOperationException(
            "Невозможно изменить неизменяемый объект SpaceMission"
        );
    }

    @Override
    public void setMissionName(String missionName)
        throws MissionValidationException {
        throw new UnsupportedOperationException(
            "Невозможно изменить неизменяемый объект SpaceMission"
        );
    }

    @Override
    public void setCostPerUnit(int costPerUnit) {
        throw new UnsupportedOperationException(
            "Невозможно изменить неизменяемый объект SpaceMission"
        );
    }
}
