import java.util.*;

public class Main {

    private static Scanner scanner = new Scanner(System.in);

    public static void main(String[] args) {
        System.out.println(
            "=== Лабораторная работа №6: Паттерны проектирования ==="
        );

        boolean running = true;
        while (running) {
            printMenu();
            int choice = getIntInput("Выберите действие: ");

            try {
                switch (choice) {
                    case 1:
                        testComparable();
                        break;
                    case 2:
                        testComparators();
                        break;
                    case 3:
                        testIterator();
                        break;
                    case 4:
                        testDecorator();
                        break;
                    case 5:
                        testFactory();
                        break;
                    case 6:
                        testThreads();
                        break;
                    case 7:
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
            "1. Задание 1: Comparable (сортировка по бизнес-методу)"
        );
        System.out.println(
            "2. Задание 2: Comparator (пользовательская сортировка)"
        );
        System.out.println("3. Задание 3: Iterator (обход элементов массива)");
        System.out.println("4. Задание 4: Decorator (неизменяемая оболочка)");
        System.out.println("5. Задания 5-7: Factory (фабричный метод)");
        System.out.println("6. Тест потоков (из лаб. 5)");
        System.out.println("7. Выход");
    }

    // Задание 1: Comparable - сортировка по возрастанию бизнес-метода
    private static void testComparable() {
        System.out.println("\n=== Задание 1: Comparable ===");
        System.out.println(
            "Сортировка по возрастанию результата calculateNetProfit()\n"
        );

        // Создаем массив миссий через фабрику
        SpaceMission[] missions = new SpaceMission[4];

        // Используем PlanetExplorationMission фабрику
        StreamManager.setSpaceMissionFactory(
            new PlanetExplorationMissionFactory()
        );
        missions[0] = StreamManager.createInstance(
            new int[] { 100, 150, 200 },
            "Альфа",
            20
        );
        missions[1] = StreamManager.createInstance(
            new int[] { 50, 60, 70 },
            "Бета",
            10
        );

        // Переключаемся на AsteroidMiningMission фабрику
        StreamManager.setSpaceMissionFactory(
            new AsteroidMiningMissionFactory()
        );
        missions[2] = StreamManager.createInstance(
            new int[] { 200, 300, 400 },
            "Гамма",
            30
        );
        missions[3] = StreamManager.createInstance(
            new int[] { 80, 90, 100 },
            "Дельта",
            15
        );

        System.out.println("До сортировки:");
        printMissionsWithProfit(missions);

        // Сортировка через Comparable (естественный порядок)
        StreamManager.sortSpaceMission(missions);

        System.out.println("\nПосле сортировки (по возрастанию прибыли):");
        printMissionsWithProfit(missions);
    }

    // Задание 2: Comparator - два компаратора
    private static void testComparators() {
        System.out.println("\n=== Задание 2: Comparator ===");

        SpaceMission[] missions = new SpaceMission[4];

        StreamManager.setSpaceMissionFactory(
            new PlanetExplorationMissionFactory()
        );
        missions[0] = StreamManager.createInstance(
            new int[] { 100, 150, 200 },
            "Альфа",
            25
        );
        missions[1] = StreamManager.createInstance(
            new int[] { 50, 60, 70 },
            "Бета",
            5
        );

        StreamManager.setSpaceMissionFactory(
            new AsteroidMiningMissionFactory()
        );
        missions[2] = StreamManager.createInstance(
            new int[] { 200, 300, 400 },
            "Гамма",
            40
        );
        missions[3] = StreamManager.createInstance(
            new int[] { 80, 90, 100 },
            "Дельта",
            10
        );

        System.out.println("Исходный массив:");
        printMissionsWithProfit(missions);

        // Сортировка по убыванию прибыли (компаратор 1)
        System.out.println("\n--- Компаратор 1: По убыванию прибыли ---");
        StreamManager.sortSpaceMission(
            missions,
            new SpaceMissionByProfitDesc()
        );
        printMissionsWithProfit(missions);

        // Сортировка по возрастанию costPerUnit (компаратор 2)
        System.out.println(
            "\n--- Компаратор 2: По возрастанию costPerUnit ---"
        );
        StreamManager.sortSpaceMission(missions, new SpaceMissionByCostAsc());
        for (SpaceMission m : missions) {
            System.out.println(
                "  " +
                    m.getMissionName() +
                    ", costPerUnit=" +
                    m.getCostPerUnit()
            );
        }
    }

    // Задание 3: Iterator - обход элементов массива
    private static void testIterator() {
        System.out.println("\n=== Задание 3: Iterator ===");

        StreamManager.setSpaceMissionFactory(
            new PlanetExplorationMissionFactory()
        );
        SpaceMission planetMission = StreamManager.createInstance(
            new int[] { 100, 150, 200, 250 },
            "PlanetIterTest",
            10
        );

        StreamManager.setSpaceMissionFactory(
            new AsteroidMiningMissionFactory()
        );
        SpaceMission asteroidMission = StreamManager.createInstance(
            new int[] { 50, 75, 100 },
            "AsteroidIterTest",
            5
        );

        // Тест 1: Улучшенный цикл for (for-each)
        System.out.println(
            "\n--- Тест 1: Улучшенный цикл for (PlanetExplorationMission) ---"
        );
        System.out.print("Элементы resourceYields: ");
        for (int value : planetMission) {
            System.out.print(value + " ");
        }
        System.out.println();

        // Тест 2: Явный итератор с while
        System.out.println(
            "\n--- Тест 2: Явный итератор с while (AsteroidMiningMission) ---"
        );
        System.out.print("Элементы mineralQuantities: ");
        Iterator<Integer> iterator = asteroidMission.iterator();
        while (iterator.hasNext()) {
            System.out.print(iterator.next() + " ");
        }
        System.out.println();
    }

    // Задание 4: Decorator - неизменяемая оболочка
    private static void testDecorator() {
        System.out.println("\n=== Задание 4: Decorator (Unmodifiable) ===");

        StreamManager.setSpaceMissionFactory(
            new PlanetExplorationMissionFactory()
        );
        SpaceMission original = StreamManager.createInstance(
            new int[] { 100, 200, 300 },
            "OriginalMission",
            15
        );

        System.out.println("Оригинальный объект: " + original);

        // Создаем неизменяемую оболочку
        SpaceMission unmodifiable = StreamManager.unmodifiableSpaceMission(
            original
        );

        System.out.println("\n--- Тест чтения из неизменяемого объекта ---");
        System.out.println(
            "getMissionName(): " + unmodifiable.getMissionName()
        );
        System.out.println(
            "getCostPerUnit(): " + unmodifiable.getCostPerUnit()
        );
        System.out.println(
            "getArrayElement(0): " + unmodifiable.getArrayElement(0)
        );
        try {
            System.out.println(
                "calculateNetProfit(): " + unmodifiable.calculateNetProfit()
            );
        } catch (MissionBusinessException e) {
            System.out.println("Ошибка бизнес-метода: " + e.getMessage());
        }

        System.out.println("\n--- Тест итератора неизменяемого объекта ---");
        System.out.print("Элементы: ");
        for (int value : unmodifiable) {
            System.out.print(value + " ");
        }
        System.out.println();

        System.out.println("\n--- Тест попытки изменения ---");

        try {
            unmodifiable.setMissionName("NewName");
            System.out.println(
                "ОШИБКА: Изменение должно было выбросить исключение!"
            );
        } catch (UnsupportedOperationException e) {
            System.out.println(
                "setMissionName() -> UnsupportedOperationException: " +
                    e.getMessage()
            );
        }

        try {
            unmodifiable.setCostPerUnit(999);
            System.out.println(
                "ОШИБКА: Изменение должно было выбросить исключение!"
            );
        } catch (UnsupportedOperationException e) {
            System.out.println(
                "setCostPerUnit() -> UnsupportedOperationException: " +
                    e.getMessage()
            );
        }

        try {
            unmodifiable.setArrayElement(0, 999);
            System.out.println(
                "ОШИБКА: Изменение должно было выбросить исключение!"
            );
        } catch (UnsupportedOperationException e) {
            System.out.println(
                "setArrayElement() -> UnsupportedOperationException: " +
                    e.getMessage()
            );
        }

        try {
            unmodifiable.setResourceData(new int[] { 1, 2, 3 });
            System.out.println(
                "ОШИБКА: Изменение должно было выбросить исключение!"
            );
        } catch (UnsupportedOperationException e) {
            System.out.println(
                "setResourceData() -> UnsupportedOperationException: " +
                    e.getMessage()
            );
        }

        System.out.println(
            "\nОригинальный объект остался без изменений: " + original
        );
    }

    // Задания 5-7: Factory - фабричный метод
    private static void testFactory() {
        System.out.println("\n=== Задания 5-7: Factory ===");

        // Задание 5: Интерфейс SpaceMissionFactory определен в SpaceMissionFactory.java

        // Задание 6: Демонстрация работы фабрик
        System.out.println(
            "\n--- Использование PlanetExplorationMissionFactory ---"
        );
        StreamManager.setSpaceMissionFactory(
            new PlanetExplorationMissionFactory()
        );

        // Задание 7: Создание через createInstance()
        SpaceMission planet1 = StreamManager.createInstance();
        SpaceMission planet2 = StreamManager.createInstance(
            new int[] { 500, 600 },
            "CustomPlanet",
            50
        );

        System.out.println("По умолчанию: " + planet1);
        System.out.println("С параметрами: " + planet2);

        System.out.println(
            "\n--- Переключение на AsteroidMiningMissionFactory ---"
        );
        StreamManager.setSpaceMissionFactory(
            new AsteroidMiningMissionFactory()
        );

        SpaceMission asteroid1 = StreamManager.createInstance();
        SpaceMission asteroid2 = StreamManager.createInstance(
            new int[] { 1000, 2000 },
            "CustomAsteroid",
            100
        );

        System.out.println("По умолчанию: " + asteroid1);
        System.out.println("С параметрами: " + asteroid2);

        // Демонстрация полиморфизма через фабрику
        System.out.println("\n--- Полиморфизм через фабрику ---");
        SpaceMission[] createdMissions = new SpaceMission[4];

        StreamManager.setSpaceMissionFactory(
            new PlanetExplorationMissionFactory()
        );
        createdMissions[0] = StreamManager.createInstance();
        createdMissions[1] = StreamManager.createInstance(
            new int[] { 100, 200 },
            "Planet2",
            10
        );

        StreamManager.setSpaceMissionFactory(
            new AsteroidMiningMissionFactory()
        );
        createdMissions[2] = StreamManager.createInstance();
        createdMissions[3] = StreamManager.createInstance(
            new int[] { 300, 400 },
            "Asteroid2",
            20
        );

        System.out.println("Созданные миссии:");
        for (SpaceMission m : createdMissions) {
            System.out.println(
                "  " + m.getClass().getSimpleName() + ": " + m.getMissionName()
            );
        }
    }

    // Тест потоков из лаб. 5
    private static void testThreads() throws InterruptedException {
        System.out.println("\n=== Тест потоков ===");

        System.out.println("\n--- Thread-классы ---");
        int[] initialData = new int[10];
        StreamManager.setSpaceMissionFactory(
            new PlanetExplorationMissionFactory()
        );
        SpaceMission mission = StreamManager.createInstance(
            initialData,
            "ThreadTestMission",
            5
        );

        WriterThread writerThread = new WriterThread(mission);
        ReaderThread readerThread = new ReaderThread(mission);

        writerThread.start();
        readerThread.start();

        writerThread.join();
        readerThread.join();

        System.out.println("\n--- Runnable с семафором ---");
        int[] initialData2 = new int[10];
        StreamManager.setSpaceMissionFactory(
            new AsteroidMiningMissionFactory()
        );
        SpaceMission mission2 = StreamManager.createInstance(
            initialData2,
            "SemaphoreTestMission",
            3
        );

        Semaphore writeSem = new Semaphore(1);
        Semaphore readSem = new Semaphore(0);

        WriterRunnable writerRunnable = new WriterRunnable(
            mission2,
            writeSem,
            readSem
        );
        ReaderRunnable readerRunnable = new ReaderRunnable(
            mission2,
            readSem,
            writeSem
        );

        Thread writer = new Thread(writerRunnable);
        Thread reader = new Thread(readerRunnable);

        writer.start();
        reader.start();

        writer.join();
        reader.join();

        System.out.println("\nТест потоков завершен.");
    }

    private static void printMissionsWithProfit(SpaceMission[] missions) {
        for (SpaceMission m : missions) {
            try {
                int profit = m.calculateNetProfit();
                System.out.println(
                    "  " +
                        m.getMissionName() +
                        " (" +
                        m.getClass().getSimpleName() +
                        "): прибыль = " +
                        profit
                );
            } catch (MissionBusinessException e) {
                System.out.println(
                    "  " + m.getMissionName() + ": " + e.getMessage()
                );
            }
        }
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
