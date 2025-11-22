public class ReaderRunnable implements Runnable {

    private SpaceMission mission;
    private Semaphore readSem;
    private Semaphore writeSem;

    public ReaderRunnable(
        SpaceMission mission,
        Semaphore readSem,
        Semaphore writeSem
    ) {
        this.mission = mission;
        this.readSem = readSem;
        this.writeSem = writeSem;
    }

    @Override
    public void run() {
        for (int i = 0; i < mission.size(); i++) {
            try {
                readSem.acquire();

                int value = mission.getArrayElement(i);
                System.out.println("Read: " + value + " from position " + i);

                writeSem.release();
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
