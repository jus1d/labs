import java.util.*;

public class MissionManager {

    private static Scanner scanner = new Scanner(System.in);
    private static List<SpaceMission> missions = new ArrayList<>();

    public static void main(String[] args) {
        System.out.println("=== Система управления космическими миссиями ===");
        System.out.println(
            "Отслеживайте исследовательские и горнодобывающие миссии по всей галактике!"
        );

        boolean running = true;
        while (running) {
            printMenu();
            int choice = getIntInput("Выберите действие: ");

            switch (choice) {
                case 1:
                    addMission();
                    break;
                case 2:
                    showAllMissions();
                    break;
                case 3:
                    groupByProfit();
                    break;
                case 4:
                    splitByType();
                    break;
                case 5:
                    running = false;
                    System.out.println(
                        "Центр управления миссиями завершает работу. Удачных полетов!"
                    );
                    break;
                default:
                    System.out.println("Неверный выбор. Попробуйте еще раз.");
            }
        }
    }

    private static void printMenu() {
        System.out.println("\n--- Меню центра управления миссиями ---");
        System.out.println("1. Добавить новую миссию");
        System.out.println("2. Показать все миссии");
        System.out.println("3. Группировать миссии по уровню прибыли");
        System.out.println("4. Разделить миссии по типу");
        System.out.println("5. Выйти из центра управления миссиями");
    }

    private static void addMission() {
        System.out.println("\nВыберите тип миссии:");
        System.out.println("1. Миссия исследования планет");
        System.out.println("2. Миссия добычи астероидов");

        int type = getIntInput("Ваш выбор: ");

        try {
            String missionName = getStringInput("Введите название миссии: ");
            int costPerUnit = getIntInput(
                "Введите стоимость за единицу (топливо/обслуживание): "
            );

            System.out.print("Введите количество целей (планет/астероидов): ");
            int count = scanner.nextInt();
            scanner.nextLine();

            int[] resources = new int[count];
            for (int i = 0; i < count; i++) {
                resources[i] = getIntInput(
                    "Доходность ресурсов для цели " + (i + 1) + ": "
                );
            }

            SpaceMission mission;
            if (type == 1) {
                StreamManager.setSpaceMissionFactory(
                    new PlanetExplorationMissionFactory()
                );
                mission = StreamManager.createInstance(
                    resources,
                    missionName,
                    costPerUnit
                );
            } else {
                StreamManager.setSpaceMissionFactory(
                    new AsteroidMiningMissionFactory()
                );
                mission = StreamManager.createInstance(
                    resources,
                    missionName,
                    costPerUnit
                );
            }

            missions.add(mission);
            System.out.println("Миссия успешно добавлена в базу данных!");
        } catch (MissionValidationException e) {
            System.out.println("Ошибка валидации: " + e.getMessage());
        } catch (Exception e) {
            System.out.println("Ошибка: " + e.getMessage());
            scanner.nextLine();
        }
    }

    private static void showAllMissions() {
        System.out.println("\n--- Все миссии ---");
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

    private static void groupByProfit() {
        System.out.println("\n--- Группировка миссий по уровню прибыли ---");

        Map<Integer, List<SpaceMission>> profitGroups = new HashMap<>();

        for (SpaceMission mission : missions) {
            try {
                int profit = mission.calculateNetProfit();
                profitGroups
                    .computeIfAbsent(profit, _ -> new ArrayList<>())
                    .add(mission);
            } catch (MissionBusinessException e) {
                System.out.println(
                    "Пропускаем миссию '" +
                        mission.getMissionName() +
                        "': " +
                        e.getMessage()
                );
            }
        }

        if (profitGroups.isEmpty()) {
            System.out.println("Нет миссий с корректными расчетами прибыли.");
            return;
        }

        for (Map.Entry<
            Integer,
            List<SpaceMission>
        > entry : profitGroups.entrySet()) {
            System.out.println(
                "\nГруппа прибыли: " + entry.getKey() + " кредитов"
            );
            for (SpaceMission mission : entry.getValue()) {
                System.out.println(
                    "  - " +
                        mission.getMissionName() +
                        " (" +
                        mission.getClass().getSimpleName() +
                        ")"
                );
            }
        }
    }

    private static void splitByType() {
        System.out.println("\n--- Классификация миссий по типу ---");

        List<PlanetExplorationMission> explorationMissions = new ArrayList<>();
        List<AsteroidMiningMission> miningMissions = new ArrayList<>();

        for (SpaceMission mission : missions) {
            if (mission instanceof PlanetExplorationMission) {
                explorationMissions.add((PlanetExplorationMission) mission);
            } else if (mission instanceof AsteroidMiningMission) {
                miningMissions.add((AsteroidMiningMission) mission);
            }
        }

        System.out.println(
            "Миссии исследования планет (" + explorationMissions.size() + "):"
        );
        for (PlanetExplorationMission mission : explorationMissions) {
            System.out.println("  - " + mission.getMissionName());
        }

        System.out.println(
            "Миссии добычи астероидов (" + miningMissions.size() + "):"
        );
        for (AsteroidMiningMission mission : miningMissions) {
            System.out.println("  - " + mission.getMissionName());
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
