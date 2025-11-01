import java.io.*;
import java.util.Scanner;

public class Test {

    public static void main(String[] args) {
        SpaceMission mission1 = new AsteroidMiningMission(
            new int[] { 69420, 10001, 15051 },
            "Альфа",
            50
        );

        SpaceMission mission2 = new PlanetExplorationMission(
            new int[] { 300, 250 },
            "Бета",
            80
        );

        System.out.println("Созданы тестовые миссии:");
        System.out.println("1. " + mission1);
        System.out.println("2. " + mission2);

        try {
            System.out.println(
                "\nПрибыль миссии 1: " + mission1.calculateNetProfit()
            );
            System.out.println(
                "Прибыль миссии 2: " + mission2.calculateNetProfit()
            );
        } catch (MissionBusinessException e) {
            System.out.println("Ошибка расчета прибыли: " + e.getMessage());
        }

        // Байтовые потоки
        System.out.println("\n--- ТЕСТ 1: Байтовые потоки ---");
        try {
            // Запись
            FileOutputStream fos = new FileOutputStream("test_byte.dat");
            StreamManager.outputSpaceMission(mission1, fos);
            StreamManager.outputSpaceMission(mission2, fos);
            fos.close();
            System.out.println("Запись в байтовый поток выполнена");

            // Чтение
            FileInputStream fis = new FileInputStream("test_byte.dat");
            SpaceMission loaded1 = StreamManager.inputSpaceMission(fis);
            SpaceMission loaded2 = StreamManager.inputSpaceMission(fis);
            fis.close();

            System.out.println("Чтение из байтового потока выполнено");
            System.out.println("  Прочитано 1: " + loaded1);
            System.out.println("  Прочитано 2: " + loaded2);
        } catch (IOException e) {
            System.out.println("Ошибка: " + e.getMessage());
        }

        // Текстовые потоки
        System.out.println("\n--- ТЕСТ 2: Текстовые потоки ---");
        try {
            // Запись
            FileWriter fw = new FileWriter("test_text.txt");
            StreamManager.writeSpaceMission(mission1, fw);
            StreamManager.writeSpaceMission(mission2, fw);
            fw.close();
            System.out.println("Запись в текстовый поток выполнена");

            // Чтение
            FileReader fr = new FileReader("test_text.txt");
            SpaceMission loaded1 = StreamManager.readSpaceMission(fr);
            SpaceMission loaded2 = StreamManager.readSpaceMission(fr);
            fr.close();

            System.out.println("Чтение из текстового потока выполнено");
            System.out.println("  Прочитано 1: " + loaded1);
            System.out.println("  Прочитано 2: " + loaded2);
        } catch (IOException e) {
            System.out.println("Ошибка: " + e.getMessage());
        }

        // Сериализация
        System.out.println("\n--- ТЕСТ 3: Сериализация ---");
        try {
            // Запись
            FileOutputStream fos = new FileOutputStream("test_serial.ser");
            StreamManager.serializeSpaceMission(mission1, fos);
            StreamManager.serializeSpaceMission(mission2, fos);
            fos.close();
            System.out.println("Сериализация выполнена");

            // Чтение
            FileInputStream fis = new FileInputStream("test_serial.ser");
            SpaceMission loaded1 = StreamManager.deserializeSpaceMission(fis);
            SpaceMission loaded2 = StreamManager.deserializeSpaceMission(fis);
            fis.close();

            System.out.println("Десериализация выполнена");
            System.out.println("  Прочитано 1: " + loaded1);
            System.out.println("  Прочитано 2: " + loaded2);
        } catch (IOException | ClassNotFoundException e) {
            System.out.println("Ошибка: " + e.getMessage());
        }

        // Форматный текстовый ввод/вывод
        System.out.println("\n--- ТЕСТ 4: Форматный текстовый ввод/вывод ---");
        try {
            // Запись
            FileWriter fw = new FileWriter("test_format.txt");
            StreamManager.writeFormatSpaceMission(mission1, fw);
            StreamManager.writeFormatSpaceMission(mission2, fw);
            fw.close();
            System.out.println("Форматная запись выполнена");

            // Чтение
            Scanner scanner = new Scanner(new File("test_format.txt"));
            scanner.useLocale(java.util.Locale.US);
            SpaceMission loaded1 = StreamManager.readFormatSpaceMission(
                scanner
            );
            SpaceMission loaded2 = StreamManager.readFormatSpaceMission(
                scanner
            );
            scanner.close();

            System.out.println("Форматное чтение выполнено");
            System.out.println("  Прочитано 1: " + loaded1);
            System.out.println("  Прочитано 2: " + loaded2);
        } catch (IOException e) {
            System.out.println("Ошибка: " + e.getMessage());
        }

        System.out.println("\n=== Все тесты выполнены ===");
        System.out.println(
            "\nДля интерактивного тестирования запустите: java Main"
        );
    }
}
