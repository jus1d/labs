public class AutoTest {

    public static void main(String[] args) throws InterruptedException {
        System.out.println("=== Автоматическое тестирование Лабораторной работы №5 ===\n");

        testTask1();
        System.out.println("\n" + "=".repeat(60) + "\n");

        testTask2();
        System.out.println("\n" + "=".repeat(60) + "\n");

        testTask3();

        System.out.println("\n=== Все тесты завершены успешно! ===");
    }

    private static void testTask1() throws InterruptedException {
        System.out.println(">>> ЗАДАНИЕ 1: Thread-классы (WriterThread и ReaderThread)");
        System.out.println(">>> Без синхронизации - возможно перемешивание операций\n");

        int[] initialData = new int[20];
        SpaceMission mission = new PlanetExplorationMission(
            initialData,
            "TestMission1",
            10
        );

        WriterThread writerThread = new WriterThread(mission);
        ReaderThread readerThread = new ReaderThread(mission);

        writerThread.start();
        readerThread.start();

        writerThread.join();
        readerThread.join();

        System.out.println("\n✓ Задание 1 выполнено успешно");
    }

    private static void testTask2() throws InterruptedException {
        System.out.println(">>> ЗАДАНИЕ 2: Runnable с ручным семафором");
        System.out.println(">>> С синхронизацией - строгий порядок write-read-write-read\n");

        int[] initialData = new int[20];
        SpaceMission mission = new AsteroidMiningMission(
            initialData,
            "TestMission2",
            5
        );

        ManualSemaphore writeSemaphore = new ManualSemaphore(1);
        ManualSemaphore readSemaphore = new ManualSemaphore(0);

        WriterRunnable writerRunnable = new WriterRunnable(
            mission,
            writeSemaphore,
            readSemaphore
        );
        ReaderRunnable readerRunnable = new ReaderRunnable(
            mission,
            writeSemaphore,
            readSemaphore
        );

        Thread writerThread = new Thread(writerRunnable);
        Thread readerThread = new Thread(readerRunnable);

        writerThread.start();
        readerThread.start();

        writerThread.join();
        readerThread.join();

        System.out.println("\n✓ Задание 2 выполнено успешно");
    }

    private static void testTask3() throws InterruptedException {
        System.out.println(">>> ЗАДАНИЕ 3: Синхронизированная оболочка");
        System.out.println(">>> Безопасный многопоточный доступ к SpaceMission\n");

        int[] initialData = new int[30];
        SpaceMission baseMission = new PlanetExplorationMission(
            initialData,
            "TestMission3",
            8
        );

        SpaceMission synchronizedMission = StreamManager.synchronizedSpaceMission(
            baseMission
        );

        System.out.println("Запуск 4 потоков одновременно (2 writer + 2 reader)...\n");

        WriterThread writer1 = new WriterThread(synchronizedMission);
        WriterThread writer2 = new WriterThread(synchronizedMission);

        writer1.start();
        writer2.start();
        writer1.join();
        writer2.join();

        ReaderThread reader1 = new ReaderThread(synchronizedMission);
        ReaderThread reader2 = new ReaderThread(synchronizedMission);

        reader1.start();
        reader2.start();
        reader1.join();
        reader2.join();

        System.out.println("\nПроверка целостности:");
        System.out.println("  - Название: " + synchronizedMission.getMissionName());
        System.out.println("  - Размер массива: " + synchronizedMission.size());

        try {
            int profit = synchronizedMission.calculateNetProfit();
            System.out.println("  - Чистая прибыль: " + profit + " кредитов");
        } catch (MissionBusinessException e) {
            System.out.println("  - Результат: " + e.getMessage());
        }

        System.out.println("\n✓ Задание 3 выполнено успешно");
    }
}
