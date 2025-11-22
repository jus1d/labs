import java.util.Scanner;

public class Main {

    private static Scanner scanner = new Scanner(System.in);

    public static void main(String[] args) {
        System.out.println(
            "=== Лабораторная работа №5: Многопоточные приложения ==="
        );

        boolean running = true;
        while (running) {
            printMenu();
            int choice = getIntInput("Выберите действие: ");

            try {
                switch (choice) {
                    case 1:
                        testTask1();
                        break;
                    case 2:
                        testTask2();
                        break;
                    case 3:
                        testTask3();
                        break;
                    case 4:
                        running = false;
                        System.out.println("Завершение работы программы.");
                        break;
                    default:
                        System.out.println(
                            "Неверный выбор. Попробуйте еще раз."
                        );
                }
            } catch (Exception e) {
                System.out.println("Ошибка: " + e.getMessage());
                e.printStackTrace();
            }
        }

        scanner.close();
    }

    private static void printMenu() {
        System.out.println("\n--- Главное меню ---");
        System.out.println(
            "1. Задание 1: Thread-классы (write/read без синхронизации)"
        );
        System.out.println(
            "2. Задание 2: Runnable с ручным семафором (write-read-write-read)"
        );
        System.out.println("3. Задание 3: Тест синхронизированной оболочки");
        System.out.println("4. Выход");
    }

    private static void testTask1() throws InterruptedException {
        System.out.println("\n=== Задание 1: Thread-классы ===");
        System.out.println("Создание миссии с массивом из 100 элементов...");

        int[] initialData = new int[100];
        SpaceMission mission = new PlanetExplorationMission(
            initialData,
            "Task1Mission",
            10
        );

        System.out.println("\nЗапуск WriterThread и ReaderThread...");

        WriterThread writerThread = new WriterThread(mission);
        ReaderThread readerThread = new ReaderThread(mission);

        System.out.println("\n--- Тест 1: Приоритеты по умолчанию ---");
        writerThread.start();
        readerThread.start();

        writerThread.join();
        readerThread.join();

        System.out.println("\n--- Тест 2: Writer с высоким приоритетом ---");
        int[] initialData2 = new int[100];
        SpaceMission mission2 = new PlanetExplorationMission(
            initialData2,
            "Task1Mission2",
            10
        );

        WriterThread writerThread2 = new WriterThread(mission2);
        ReaderThread readerThread2 = new ReaderThread(mission2);

        writerThread2.setPriority(Thread.MAX_PRIORITY);
        readerThread2.setPriority(Thread.MIN_PRIORITY);

        writerThread2.start();
        readerThread2.start();

        writerThread2.join();
        readerThread2.join();

        System.out.println("\n--- Тест 3: Reader с высоким приоритетом ---");
        int[] initialData3 = new int[100];
        SpaceMission mission3 = new PlanetExplorationMission(
            initialData3,
            "Task1Mission3",
            10
        );

        WriterThread writerThread3 = new WriterThread(mission3);
        ReaderThread readerThread3 = new ReaderThread(mission3);

        writerThread3.setPriority(Thread.MIN_PRIORITY);
        readerThread3.setPriority(Thread.MAX_PRIORITY);

        writerThread3.start();
        readerThread3.start();

        writerThread3.join();
        readerThread3.join();

        System.out.println("\nЗадание 1 завершено.");
    }

    private static void testTask2() throws InterruptedException {
        System.out.println("\n=== Задание 2: Runnable с ручным семафором ===");
        System.out.println("Создание миссии с массивом из 100 элементов...");

        int[] initialData = new int[100];
        SpaceMission mission = new AsteroidMiningMission(
            initialData,
            "Task2Mission",
            5
        );

        Semaphore semaphore = new Semaphore(1);

        System.out.println("\nЗапуск WriterRunnable и ReaderRunnable...");
        System.out.println("Ожидается порядок: write-read-write-read-...\n");

        WriterRunnable writerRunnable = new WriterRunnable(mission, semaphore);
        ReaderRunnable readerRunnable = new ReaderRunnable(mission, semaphore);

        Thread writerThread = new Thread(writerRunnable);
        Thread readerThread = new Thread(readerRunnable);

        writerThread.start();
        readerThread.start();

        writerThread.join();
        readerThread.join();

        System.out.println("\nЗадание 2 завершено.");
    }

    private static void testTask3() throws InterruptedException {
        System.out.println(
            "\n=== Задание 3: Тест синхронизированной оболочки ==="
        );
        System.out.println(
            "Создание обычной миссии и синхронизированной оболочки..."
        );

        int[] initialData = new int[100];
        SpaceMission baseMission = new PlanetExplorationMission(
            initialData,
            "Task3Mission",
            8
        );

        SpaceMission synchronizedMission =
            StreamManager.synchronizedSpaceMission(baseMission);

        System.out.println(
            "\nЗапуск нескольких потоков с синхронизированной оболочкой..."
        );
        System.out.println("Все операции должны выполняться безопасно.\n");

        WriterThread writer1 = new WriterThread(synchronizedMission);
        WriterThread writer2 = new WriterThread(synchronizedMission);
        ReaderThread reader1 = new ReaderThread(synchronizedMission);
        ReaderThread reader2 = new ReaderThread(synchronizedMission);

        writer1.start();
        writer2.start();

        writer1.join();
        writer2.join();

        reader1.start();
        reader2.start();

        reader1.join();
        reader2.join();

        System.out.println("\nПроверка целостности данных:");
        System.out.println(
            "Название миссии: " + synchronizedMission.getMissionName()
        );
        System.out.println("Размер массива: " + synchronizedMission.size());

        try {
            int profit = synchronizedMission.calculateNetProfit();
            System.out.println("Чистая прибыль: " + profit + " кредитов");
        } catch (MissionBusinessException e) {
            System.out.println("Чистый убыток: " + e.getMessage());
        }

        System.out.println("\nЗадание 3 завершено.");
    }

    private static int getIntInput(String prompt) {
        while (true) {
            try {
                System.out.print(prompt);
                int value = scanner.nextInt();
                scanner.nextLine();
                return value;
            } catch (Exception e) {
                System.out.println(
                    "Ошибка: Пожалуйста, введите корректное целое число."
                );
                scanner.nextLine();
            }
        }
    }
}
