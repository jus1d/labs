import java.util.Random;

public class WriterRunnable implements Runnable {

    private SpaceMission mission;
    private Semaphore writeSem;
    private Semaphore readSem;

    public WriterRunnable(SpaceMission mission, Semaphore writeSem, Semaphore readSem) {
        this.mission = mission;
        this.writeSem = writeSem;
        this.readSem = readSem;
    }

    @Override
    public void run() {
        Random random = new Random();
        for (int i = 0; i < mission.size(); i++) {
            try {
                writeSem.acquire();

                int value = random.nextInt(1000) + 1;
                mission.setArrayElement(i, value);
                System.out.println("Write: " + value + " to position " + i);

                readSem.release();
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
