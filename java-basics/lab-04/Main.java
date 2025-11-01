import java.io.*;
import java.util.*;

public class Main {

    private static Scanner scanner = new Scanner(System.in);
    private static List<SpaceMission> missions = new ArrayList<>();

    public static void main(String[] args) {
        System.out.println(
            "=== Лабораторная работа №4: Работа с потоками данных ==="
        );

        boolean running = true;
        while (running) {
            printMenu();
            int choice = getIntInput("Выберите действие: ");

            try {
                switch (choice) {
                    case 1:
                        fillDatabase();
                        break;
                    case 2:
                        testByteStreams();
                        break;
                    case 3:
                        testTextStreams();
                        break;
                    case 4:
                        testSerialization();
                        break;
                    case 5:
                        testFormattedTextIO();
                        break;
                    case 6:
                        showAllMissions();
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
        System.out.println("1. Заполнить базу элементов с консоли");
        System.out.println("2. Тест байтовых потоков (запись/чтение)");
        System.out.println("3. Тест текстовых потоков (запись/чтение)");
        System.out.println("4. Тест сериализации (запись/чтение)");
        System.out.println("5. Тест форматного текстового ввода/вывода");
        System.out.println("6. Показать все миссии в памяти");
        System.out.println("7. Выход");
    }

    private static void fillDatabase() {
        System.out.println("\n--- Заполнение базы миссий ---");
        System.out.print("Сколько миссий вы хотите добавить? ");
        int count = scanner.nextInt();
        scanner.nextLine();

        for (int i = 0; i < count; i++) {
            System.out.println("\nМиссия #" + (i + 1));
            System.out.println("Выберите тип миссии:");
            System.out.println("1. Миссия исследования планет");
            System.out.println("2. Миссия добычи астероидов");

            int type = getIntInput("Ваш выбор: ");

            try {
                String missionName = getStringInput(
                    "Введите название миссии: "
                );
                int costPerUnit = getIntInput(
                    "Введите стоимость за единицу (топливо/обслуживание): "
                );

                System.out.print(
                    "Введите количество целей (планет/астероидов): "
                );
                int size = scanner.nextInt();
                scanner.nextLine();

                int[] resources = new int[size];
                for (int j = 0; j < size; j++) {
                    resources[j] = getIntInput(
                        "Доходность ресурсов для цели " + (j + 1) + ": "
                    );
                }

                SpaceMission mission;
                if (type == 1) {
                    mission = new PlanetExplorationMission(
                        resources,
                        missionName,
                        costPerUnit
                    );
                } else {
                    mission = new AsteroidMiningMission(
                        resources,
                        missionName,
                        costPerUnit
                    );
                }

                missions.add(mission);
                System.out.println("Миссия успешно добавлена!");
            } catch (MissionValidationException e) {
                System.out.println("Ошибка валидации: " + e.getMessage());
                i--;
            }
        }

        System.out.println("\nВсего добавлено миссий: " + missions.size());
    }

    private static void testByteStreams() throws IOException {
        if (missions.isEmpty()) {
            System.out.println("База миссий пуста! Сначала заполните её.");
            return;
        }

        String filename = "missions_byte.dat";

        System.out.println("\n--- Тест байтовых потоков (Задание 1) ---");

        System.out.println("Запись миссий в байтовый файл: " + filename);
        try (FileOutputStream fos = new FileOutputStream(filename)) {
            for (SpaceMission mission : missions) {
                StreamManager.outputSpaceMission(mission, fos);
            }
        }
        System.out.println("Записано миссий: " + missions.size());

        System.out.println("\nЧтение миссий из байтового файла: " + filename);
        List<SpaceMission> loadedMissions = new ArrayList<>();
        try (FileInputStream fis = new FileInputStream(filename)) {
            for (int i = 0; i < missions.size(); i++) {
                SpaceMission mission = StreamManager.inputSpaceMission(fis);
                loadedMissions.add(mission);
            }
        }
        System.out.println("Прочитано миссий: " + loadedMissions.size());

        System.out.println("\nПрочитанные миссии:");
        for (int i = 0; i < loadedMissions.size(); i++) {
            System.out.println((i + 1) + ". " + loadedMissions.get(i));
        }
    }

    private static void testTextStreams() throws IOException {
        if (missions.isEmpty()) {
            System.out.println("База миссий пуста! Сначала заполните её.");
            return;
        }

        String filename = "missions_text.txt";

        System.out.println("\n--- Тест текстовых потоков (Задание 1) ---");

        System.out.println("Запись миссий в текстовый файл: " + filename);
        try (FileWriter fw = new FileWriter(filename)) {
            for (SpaceMission mission : missions) {
                StreamManager.writeSpaceMission(mission, fw);
            }
        }
        System.out.println("Записано миссий: " + missions.size());

        System.out.println("\nЧтение миссий из текстового файла: " + filename);
        List<SpaceMission> loadedMissions = new ArrayList<>();
        try (FileReader fr = new FileReader(filename)) {
            for (int i = 0; i < missions.size(); i++) {
                SpaceMission mission = StreamManager.readSpaceMission(fr);
                loadedMissions.add(mission);
            }
        }
        System.out.println("Прочитано миссий: " + loadedMissions.size());

        System.out.println("\nПрочитанные миссии:");
        for (int i = 0; i < loadedMissions.size(); i++) {
            System.out.println((i + 1) + ". " + loadedMissions.get(i));
        }
    }

    private static void testSerialization()
        throws IOException, ClassNotFoundException {
        if (missions.isEmpty()) {
            System.out.println("База миссий пуста! Сначала заполните её.");
            return;
        }

        String filename = "missions_serialized.ser";

        System.out.println("\n--- Тест сериализации (Задание 2) ---");

        System.out.println("Сериализация миссий в файл: " + filename);
        try (FileOutputStream fos = new FileOutputStream(filename)) {
            for (SpaceMission mission : missions) {
                StreamManager.serializeSpaceMission(mission, fos);
            }
        }
        System.out.println("Сериализовано миссий: " + missions.size());

        System.out.println("\nДесериализация миссий из файла: " + filename);
        List<SpaceMission> loadedMissions = new ArrayList<>();
        try (FileInputStream fis = new FileInputStream(filename)) {
            for (int i = 0; i < missions.size(); i++) {
                SpaceMission mission = StreamManager.deserializeSpaceMission(
                    fis
                );
                loadedMissions.add(mission);
            }
        }
        System.out.println("Десериализовано миссий: " + loadedMissions.size());

        System.out.println("\nДесериализованные миссии:");
        for (int i = 0; i < loadedMissions.size(); i++) {
            System.out.println((i + 1) + ". " + loadedMissions.get(i));
        }
    }

    private static void testFormattedTextIO() throws IOException {
        if (missions.isEmpty()) {
            System.out.println("База миссий пуста! Сначала заполните её.");
            return;
        }

        String filename = "missions_formatted.txt";

        System.out.println(
            "\n--- Тест форматного текстового ввода/вывода (Задание 3) ---"
        );

        System.out.println("Форматная запись миссий в файл: " + filename);
        try (FileWriter fw = new FileWriter(filename)) {
            for (SpaceMission mission : missions) {
                StreamManager.writeFormatSpaceMission(mission, fw);
            }
        }
        System.out.println("Записано миссий: " + missions.size());

        System.out.println("\nФорматное чтение миссий из файла: " + filename);
        List<SpaceMission> loadedMissions = new ArrayList<>();
        try (Scanner fileScanner = new Scanner(new File(filename))) {
            fileScanner.useLocale(Locale.US);

            while (
                fileScanner.hasNextLine() &&
                loadedMissions.size() < missions.size()
            ) {
                SpaceMission mission = StreamManager.readFormatSpaceMission(
                    fileScanner
                );
                loadedMissions.add(mission);
            }
        }
        System.out.println("Прочитано миссий: " + loadedMissions.size());

        System.out.println("\nПрочитанные миссии:");
        for (int i = 0; i < loadedMissions.size(); i++) {
            System.out.println((i + 1) + ". " + loadedMissions.get(i));
        }
    }

    private static void showAllMissions() {
        System.out.println("\n--- Все миссии в памяти ---");
        if (missions.isEmpty()) {
            System.out.println("В базе данных нет миссий.");
            return;
        }

        for (int i = 0; i < missions.size(); i++) {
            SpaceMission mission = missions.get(i);
            System.out.println((i + 1) + ". " + mission);

            try {
                int profit = mission.calculateNetProfit();
                System.out.println(
                    "   Чистая прибыль: " + profit + " кредитов"
                );
            } catch (MissionBusinessException e) {
                System.out.println("   Бизнес-ошибка: " + e.getMessage());
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
            } catch (InputMismatchException e) {
                System.out.println(
                    "Ошибка: Пожалуйста, введите корректное целое число."
                );
                scanner.nextLine();
            }
        }
    }

    private static String getStringInput(String prompt) {
        System.out.print(prompt);
        return scanner.nextLine();
    }
}
