import java.util.Random;

public class WriterRunnable implements Runnable {

    private SpaceMission mission;
    private Semaphore semaphore;

    public WriterRunnable(SpaceMission mission, Semaphore semaphore) {
        this.mission = mission;
        this.semaphore = semaphore;
    }

    @Override
    public void run() {
        Random random = new Random();
        for (int i = 0; i < mission.size(); i++) {
            try {
                semaphore.acquire();

                int value = random.nextInt(1000) + 1;
                mission.setArrayElement(i, value);
                System.out.println("Write: " + value + " to position " + i);

                semaphore.release();
            } catch (InterruptedException e) {
                System.err.println(
                    "WriterRunnable interrupted: " + e.getMessage()
                );
                Thread.currentThread().interrupt();
                break;
            } catch (MissionValidationException e) {
                System.err.println(
                    "Error writing at position " + i + ": " + e.getMessage()
                );
            }
        }
        System.out.println("WriterRunnable finished.");
    }
}
