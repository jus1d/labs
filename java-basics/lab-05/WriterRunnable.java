import java.util.Random;

public class WriterRunnable implements Runnable {

    private SpaceMission mission;
    private ManualSemaphore writeSemaphore;
    private ManualSemaphore readSemaphore;
    private Random random;

    public WriterRunnable(
        SpaceMission mission,
        ManualSemaphore writeSemaphore,
        ManualSemaphore readSemaphore
    ) {
        this.mission = mission;
        this.writeSemaphore = writeSemaphore;
        this.readSemaphore = readSemaphore;
        this.random = new Random();
    }

    @Override
    public void run() {
        for (int i = 0; i < mission.size(); i++) {
            try {
                writeSemaphore.acquire();

                int value = random.nextInt(1000) + 1;
                mission.setArrayElement(i, value);
                System.out.println("Write: " + value + " to position " + i);

                readSemaphore.release();
            } catch (InterruptedException e) {
                System.err.println("WriterRunnable interrupted: " + e.getMessage());
                Thread.currentThread().interrupt();
                break;
            } catch (MissionValidationException e) {
                System.err.println("Error writing at position " + i + ": " + e.getMessage());
            }
        }
        System.out.println("WriterRunnable finished.");
    }
}
