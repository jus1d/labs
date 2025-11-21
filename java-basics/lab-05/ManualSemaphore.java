import java.util.LinkedList;
import java.util.Queue;

public class ManualSemaphore {

    private int permits;
    private Queue<Thread> waitingQueue;

    public ManualSemaphore(int permits) {
        this.permits = permits;
        this.waitingQueue = new LinkedList<>();
    }

    public synchronized void acquire() throws InterruptedException {
        Thread currentThread = Thread.currentThread();
        waitingQueue.add(currentThread);

        while (permits <= 0 || waitingQueue.peek() != currentThread) {
            wait();
        }

        waitingQueue.poll();
        permits--;
    }

    public synchronized void release() {
        permits++;
        notifyAll();
    }

    public synchronized int availablePermits() {
        return permits;
    }
}
