import java.util.Random;

public class WriterThread extends Thread {

    private SpaceMission mission;

    public WriterThread(SpaceMission mission) {
        this.mission = mission;
    }

    @Override
    public void run() {
        Random random = new Random();
        for (int i = 0; i < mission.size(); i++) {
            int value = random.nextInt(1000) + 1;
            try {
                mission.setArrayElement(i, value);
                System.out.println("Write: " + value + " to position " + i);
            } catch (MissionValidationException e) {
                System.err.println(
                    "Error writing at position " + i + ": " + e.getMessage()
                );
            }
        }
        System.out.println("WriterThread finished.");
    }
}
