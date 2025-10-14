import java.util.*;

public class MusicCollectionApp {

    private static Scanner scanner = new Scanner(System.in);
    private static List<MusicCollection> collections = new ArrayList<>();

    public static void main(String[] args) {
        System.out.println(
            "=== Система управления музыкальными коллекциями ===\n"
        );

        boolean running = true;
        while (running) {
            System.out.println("\n--- Главное меню ---");
            System.out.println("1. Добавить студийный альбом");
            System.out.println("2. Добавить концертный альбом");
            System.out.println("3. Показать все коллекции");
            System.out.println("4. Найти альбомы с одинаковой длительностью");
            System.out.println("5. Разделить по типам");
            System.out.println("6. Выход");
            System.out.print("Выберите действие: ");

            try {
                int choice = Integer.parseInt(scanner.nextLine());

                switch (choice) {
                    case 1:
                        addStudioAlbum();
                        break;
                    case 2:
                        addLiveAlbum();
                        break;
                    case 3:
                        displayAllCollections();
                        break;
                    case 4:
                        findSameDuration();
                        break;
                    case 5:
                        splitByType();
                        break;
                    case 6:
                        running = false;
                        System.out.println("Программа завершена.");
                        break;
                    default:
                        System.out.println("Неверный выбор. Попробуйте снова.");
                }
            } catch (NumberFormatException e) {
                System.out.println("Ошибка: введите число.");
            } catch (Exception e) {
                System.out.println("Ошибка: " + e.getMessage());
            }
        }
    }

    private static void addStudioAlbum() {
        try {
            System.out.print("Название альбома: ");
            String title = scanner.nextLine();

            System.out.print("Год выпуска: ");
            int year = Integer.parseInt(scanner.nextLine());

            System.out.print("Количество треков: ");
            int trackCount = Integer.parseInt(scanner.nextLine());

            if (trackCount <= 0) {
                throw new InvalidAlbumDataException(
                    "Количество треков должно быть положительным"
                );
            }

            int[] durations = new int[trackCount];
            for (int i = 0; i < trackCount; i++) {
                System.out.print(
                    "Длительность трека " + (i + 1) + " (в секундах): "
                );
                durations[i] = Integer.parseInt(scanner.nextLine());
            }

            System.out.print("Минимальная длительность трека (сек): ");
            int minDuration = Integer.parseInt(scanner.nextLine());

            StudioAlbum album = new StudioAlbum(
                durations,
                title,
                year,
                minDuration
            );
            collections.add(album);

            System.out.println("✓ Студийный альбом успешно добавлен!");
        } catch (InvalidAlbumDataException e) {
            System.out.println("Ошибка валидации: " + e.getMessage());
        } catch (NumberFormatException e) {
            System.out.println("Ошибка: введите корректное число.");
        }
    }

    private static void addLiveAlbum() {
        try {
            System.out.print("Название концерта: ");
            String title = scanner.nextLine();

            System.out.print("Год записи: ");
            int year = Integer.parseInt(scanner.nextLine());

            System.out.print("Количество песен: ");
            int songCount = Integer.parseInt(scanner.nextLine());

            if (songCount <= 0) {
                throw new InvalidAlbumDataException(
                    "Количество песен должно быть положительным"
                );
            }

            int[] durations = new int[songCount];
            for (int i = 0; i < songCount; i++) {
                System.out.print(
                    "Длительность песни " + (i + 1) + " (в секундах): "
                );
                durations[i] = Integer.parseInt(scanner.nextLine());
            }

            System.out.print("Длительность вступления/окончания (сек): ");
            int introOutro = Integer.parseInt(scanner.nextLine());

            LiveAlbum album = new LiveAlbum(durations, title, year, introOutro);
            collections.add(album);

            System.out.println("✓ Концертный альбом успешно добавлен!");
        } catch (InvalidAlbumDataException e) {
            System.out.println("Ошибка валидации: " + e.getMessage());
        } catch (NumberFormatException e) {
            System.out.println("Ошибка: введите корректное число.");
        }
    }

    private static void displayAllCollections() {
        if (collections.isEmpty()) {
            System.out.println("Коллекция пуста.");
            return;
        }

        System.out.println("\n=== Все музыкальные коллекции ===");
        for (int i = 0; i < collections.size(); i++) {
            MusicCollection collection = collections.get(i);
            System.out.println((i + 1) + ". " + collection.toString());

            try {
                int duration = collection.calculateTotalDuration();
                System.out.println(
                    "   Общая длительность: " +
                        duration +
                        " сек (" +
                        formatTime(duration) +
                        ")"
                );
            } catch (InvalidDurationException e) {
                System.out.println(
                    "   Ошибка расчета длительности: " + e.getMessage()
                );
            }
        }
    }

    private static void findSameDuration() {
        if (collections.isEmpty()) {
            System.out.println("Коллекция пуста.");
            return;
        }

        System.out.println(
            "\n=== Поиск альбомов с одинаковой длительностью ==="
        );

        Map<Integer, List<MusicCollection>> durationGroups = new HashMap<>();

        for (MusicCollection collection : collections) {
            try {
                int duration = collection.calculateTotalDuration();
                durationGroups
                    .computeIfAbsent(duration, _ -> new ArrayList<>())
                    .add(collection);
            } catch (InvalidDurationException e) {
                System.out.println(
                    "Ошибка при обработке " +
                        collection.getTitle() +
                        ": " +
                        e.getMessage()
                );
            }
        }

        boolean found = false;
        for (Map.Entry<
            Integer,
            List<MusicCollection>
        > entry : durationGroups.entrySet()) {
            if (entry.getValue().size() > 1) {
                found = true;
                System.out.println(
                    "\nДлительность: " +
                        entry.getKey() +
                        " сек (" +
                        formatTime(entry.getKey()) +
                        ")"
                );
                for (MusicCollection collection : entry.getValue()) {
                    System.out.println("  - " + collection.toString());
                }
            }
        }

        if (!found) {
            System.out.println(
                "Альбомы с одинаковой длительностью не найдены."
            );
        }
    }

    private static void splitByType() {
        if (collections.isEmpty()) {
            System.out.println("Коллекция пуста.");
            return;
        }

        List<StudioAlbum> studioAlbums = new ArrayList<>();
        List<LiveAlbum> liveAlbums = new ArrayList<>();

        for (MusicCollection collection : collections) {
            if (collection instanceof StudioAlbum) {
                studioAlbums.add((StudioAlbum) collection);
            } else if (collection instanceof LiveAlbum) {
                liveAlbums.add((LiveAlbum) collection);
            }
        }

        System.out.println("\n=== Разделение по типам ===");
        System.out.println(
            "\nСтудийные альбомы (" + studioAlbums.size() + "):"
        );
        if (studioAlbums.isEmpty()) {
            System.out.println("  (нет)");
        } else {
            for (StudioAlbum album : studioAlbums) {
                System.out.println("  - " + album.toString());
            }
        }

        System.out.println("\nКонцертные альбомы (" + liveAlbums.size() + "):");
        if (liveAlbums.isEmpty()) {
            System.out.println("  (нет)");
        } else {
            for (LiveAlbum album : liveAlbums) {
                System.out.println("  - " + album.toString());
            }
        }
    }

    private static String formatTime(int seconds) {
        int minutes = seconds / 60;
        int secs = seconds % 60;
        return String.format("%d:%02d", minutes, secs);
    }
}
