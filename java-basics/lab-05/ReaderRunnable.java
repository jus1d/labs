public class ReaderRunnable implements Runnable {

    private SpaceMission mission;
    private ManualSemaphore writeSemaphore;
    private ManualSemaphore readSemaphore;

    public ReaderRunnable(
        SpaceMission mission,
        ManualSemaphore writeSemaphore,
        ManualSemaphore readSemaphore
    ) {
        this.mission = mission;
        this.writeSemaphore = writeSemaphore;
        this.readSemaphore = readSemaphore;
    }

    @Override
    public void run() {
        for (int i = 0; i < mission.size(); i++) {
            try {
                readSemaphore.acquire();

                int value = mission.getArrayElement(i);
                System.out.println("Read: " + value + " from position " + i);

                writeSemaphore.release();
            } catch (InterruptedException e) {
                System.err.println(
                    "ReaderRunnable interrupted: " + e.getMessage()
                );
                Thread.currentThread().interrupt();
                break;
            } catch (MissionValidationException e) {
                System.err.println(
                    "Error reading at position " + i + ": " + e.getMessage()
                );
            }
        }
        System.out.println("ReaderRunnable finished.");
    }
}
