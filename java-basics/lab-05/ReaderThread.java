public class ReaderThread extends Thread {

    private SpaceMission mission;

    public ReaderThread(SpaceMission mission) {
        this.mission = mission;
    }

    @Override
    public void run() {
        for (int i = 0; i < mission.size(); i++) {
            try {
                int value = mission.getArrayElement(i);
                System.out.println("Read: " + value + " from position " + i);
            } catch (MissionValidationException e) {
                System.err.println("Error reading at position " + i + ": " + e.getMessage());
            }
        }
        System.out.println("ReaderThread finished.");
    }
}
