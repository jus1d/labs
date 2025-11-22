public class ReaderRunnable implements Runnable {

    private SpaceMission mission;
    private Semaphore semaphore;

    public ReaderRunnable(SpaceMission mission, Semaphore semaphore) {
        this.mission = mission;
        this.semaphore = semaphore;
    }

    @Override
    public void run() {
        for (int i = 0; i < mission.size(); i++) {
            try {
                semaphore.acquire();

                int value = mission.getArrayElement(i);
                System.out.println("Read: " + value + " from position " + i);

                semaphore.release();
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
