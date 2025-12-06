import java.awt.*;
import java.awt.event.MouseAdapter;
import java.awt.event.MouseEvent;
import java.io.*;
import java.util.*;
import java.util.List;
import javax.swing.*;
import javax.swing.border.EmptyBorder;

public class MissionDatabaseGUI extends JFrame {

    private List<SpaceMission> missions = new ArrayList<>();
    private JPanel contentPanel;
    private JScrollPane scrollPane;

    public MissionDatabaseGUI() {
        setTitle("База данных космических миссий");
        setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        setSize(800, 600);
        setLocationRelativeTo(null);

        createMenu();
        createContentPanel();

        setVisible(true);
    }

    private void createMenu() {
        JMenuBar menuBar = new JMenuBar();

        JMenu fileMenu = new JMenu("Файл");
        JMenuItem loadItem = new JMenuItem("Загрузить базу");
        loadItem.addActionListener(e -> loadDatabase());
        JMenuItem fillItem = new JMenuItem("Автозаполнение базы");
        fillItem.addActionListener(e -> autoFillDatabase());
        JMenuItem addItem = new JMenuItem("Добавить объект");
        addItem.addActionListener(e -> addObject());
        JMenuItem removeItem = new JMenuItem("Удалить объект по номеру");
        removeItem.addActionListener(e -> removeObject());

        fileMenu.add(loadItem);
        fileMenu.add(fillItem);
        fileMenu.add(addItem);
        fileMenu.add(removeItem);

        JMenu styleMenu = new JMenu("Стиль");
        ButtonGroup group = new ButtonGroup();
        for (UIManager.LookAndFeelInfo info : UIManager.getInstalledLookAndFeels()) {
            JRadioButtonMenuItem item = new JRadioButtonMenuItem(
                info.getName()
            );
            item.addActionListener(e -> changeLookAndFeel(info));
            group.add(item);
            styleMenu.add(item);
            if (UIManager.getLookAndFeel().getName().equals(info.getName())) {
                item.setSelected(true);
            }
        }

        menuBar.add(fileMenu);
        menuBar.add(styleMenu);
        setJMenuBar(menuBar);
    }

    private void createContentPanel() {
        contentPanel = new JPanel();
        contentPanel.setLayout(new BoxLayout(contentPanel, BoxLayout.Y_AXIS));
        contentPanel.setBorder(new EmptyBorder(10, 10, 10, 10));

        scrollPane = new JScrollPane(contentPanel);
        scrollPane.setHorizontalScrollBarPolicy(
            JScrollPane.HORIZONTAL_SCROLLBAR_AS_NEEDED
        );
        scrollPane.setVerticalScrollBarPolicy(
            JScrollPane.VERTICAL_SCROLLBAR_ALWAYS
        );

        add(scrollPane, BorderLayout.CENTER);
    }

    private void loadDatabase() {
        JFileChooser fileChooser = new JFileChooser();
        int result = fileChooser.showOpenDialog(this);
        if (result != JFileChooser.APPROVE_OPTION) return;

        File file = fileChooser.getSelectedFile();
        String name = file.getName().toLowerCase();

        List<SpaceMission> loaded = new ArrayList<>();
        try {
            if (name.endsWith(".bin") || name.endsWith(".dat")) {
                try (FileInputStream fis = new FileInputStream(file)) {
                    while (fis.available() > 0) {
                        loaded.add(StreamManager.inputSpaceMission(fis));
                    }
                }
            } else if (name.endsWith(".txt")) {
                try (
                    BufferedReader br = new BufferedReader(new FileReader(file))
                ) {
                    String line;
                    while ((line = br.readLine()) != null) {
                        if (!line.trim().isEmpty()) {
                            loaded.add(
                                StreamManager.readSpaceMission(
                                    new StringReader(line)
                                )
                            );
                        }
                    }
                }
            } else if (name.endsWith(".ser")) {
                try (FileInputStream fis = new FileInputStream(file)) {
                    while (fis.available() > 0) {
                        loaded.add(StreamManager.deserializeSpaceMission(fis));
                    }
                }
            } else if (name.endsWith("_formatted.txt")) {
                try (Scanner sc = new Scanner(file)) {
                    while (sc.hasNext()) {
                        loaded.add(StreamManager.readFormatSpaceMission(sc));
                    }
                }
            } else {
                JOptionPane.showMessageDialog(
                    this,
                    "Неподдерживаемый формат файла",
                    "Ошибка",
                    JOptionPane.ERROR_MESSAGE
                );
                return;
            }

            missions.clear();
            missions.addAll(loaded);
            refreshContent();
            JOptionPane.showMessageDialog(
                this,
                "База успешно загружена: " + loaded.size() + " объектов"
            );
        } catch (Exception e) {
            e.printStackTrace();
            JOptionPane.showMessageDialog(
                this,
                "Ошибка при загрузке: " + e.getMessage(),
                "Ошибка",
                JOptionPane.ERROR_MESSAGE
            );
        }
    }

    private void autoFillDatabase() {
        missions.clear();
        Random rnd = new Random();
        int count = 5 + rnd.nextInt(6);

        String[] planetNames = {
            "Альфа",
            "Бета",
            "Гамма",
            "Дельта",
            "Эпсилон",
        };
        String[] asteroidNames = {
            "Церера",
            "Паллада",
            "Веста",
            "Юнона",
            "Эрос",
        };

        for (int i = 0; i < count; i++) {
            int size = 2 + rnd.nextInt(5);
            int[] resources = new int[size];
            for (int j = 0; j < size; j++) {
                resources[j] = 100 + rnd.nextInt(400);
            }

            StreamManager.setSpaceMissionFactory(
                new PlanetExplorationMissionFactory()
            );
            missions.add(
                StreamManager.createInstance(
                    resources,
                    planetNames[rnd.nextInt(planetNames.length)] +
                        "-" +
                        (i + 1),
                    10 + rnd.nextInt(40)
                )
            );

            int[] minerals = new int[size];
            for (int j = 0; j < size; j++) {
                minerals[j] = 50 + rnd.nextInt(250);
            }

            StreamManager.setSpaceMissionFactory(
                new AsteroidMiningMissionFactory()
            );
            missions.add(
                StreamManager.createInstance(
                    minerals,
                    asteroidNames[rnd.nextInt(asteroidNames.length)] +
                        "-" +
                        (i + 1),
                    5 + rnd.nextInt(25)
                )
            );
        }

        refreshContent();
    }

    private void addObject() {
        String[] options = { "Исследование планет", "Добыча астероидов" };
        int type = JOptionPane.showOptionDialog(
            this,
            "Выберите тип объекта:",
            "Добавление",
            JOptionPane.DEFAULT_OPTION,
            JOptionPane.QUESTION_MESSAGE,
            null,
            options,
            options[0]
        );
        if (type == -1) return;

        String name = JOptionPane.showInputDialog(this, "Название:");
        if (name == null || name.isBlank()) return;

        String dataStr = JOptionPane.showInputDialog(
            this,
            "Данные ресурсов через запятую:"
        );
        if (dataStr == null || dataStr.isBlank()) return;
        int[] data;
        try {
            String[] parts = dataStr.split(",");
            data = new int[parts.length];
            for (int i = 0; i < parts.length; i++) {
                data[i] = Integer.parseInt(parts[i].trim());
            }
        } catch (NumberFormatException ex) {
            JOptionPane.showMessageDialog(
                this,
                "Данные должны быть числами",
                "Ошибка",
                JOptionPane.ERROR_MESSAGE
            );
            return;
        }

        String costStr = JOptionPane.showInputDialog(
            this,
            "Стоимость за единицу:"
        );
        if (costStr == null || costStr.isBlank()) return;
        int cost;
        try {
            cost = Integer.parseInt(costStr.trim());
        } catch (NumberFormatException ex) {
            JOptionPane.showMessageDialog(
                this,
                "Должно быть число",
                "Ошибка",
                JOptionPane.ERROR_MESSAGE
            );
            return;
        }

        try {
            if (type == 0) {
                StreamManager.setSpaceMissionFactory(
                    new PlanetExplorationMissionFactory()
                );
            } else {
                StreamManager.setSpaceMissionFactory(
                    new AsteroidMiningMissionFactory()
                );
            }
            missions.add(StreamManager.createInstance(data, name, cost));
        } catch (Exception ex) {
            JOptionPane.showMessageDialog(
                this,
                ex.getMessage(),
                "Ошибка создания",
                JOptionPane.ERROR_MESSAGE
            );
        }

        refreshContent();
    }

    private void removeObject() {
        String input = JOptionPane.showInputDialog(
            this,
            "Введите номер объекта для удаления (0 - первый):"
        );
        if (input == null || input.isBlank()) return;
        int idx;
        try {
            idx = Integer.parseInt(input.trim());
            if (
                idx < 0 || idx >= missions.size()
            ) throw new IndexOutOfBoundsException();
        } catch (Exception ex) {
            JOptionPane.showMessageDialog(
                this,
                "Неверный номер",
                "Ошибка",
                JOptionPane.ERROR_MESSAGE
            );
            return;
        }
        missions.remove(idx);
        refreshContent();
    }

    private void refreshContent() {
        contentPanel.removeAll();
        int index = 0;
        for (SpaceMission mission : missions) {
            JPanel panel = createMissionPanel(mission, index);
            contentPanel.add(panel);
            contentPanel.add(Box.createVerticalStrut(5));
            index++;
        }
        contentPanel.revalidate();
        contentPanel.repaint();
    }

    private JPanel createMissionPanel(SpaceMission mission, int index) {
        JPanel panel = new JPanel();
        panel.setLayout(new BorderLayout());
        panel.setBorder(BorderFactory.createLineBorder(Color.BLACK));
        panel.setBackground(index % 2 == 0 ? Color.LIGHT_GRAY : Color.WHITE);

        JLabel label = new JLabel(
            "<html><b>" +
                mission.getMissionName() +
                "</b> (" +
                mission.getClass().getSimpleName() +
                ")</html>"
        );
        label.setBorder(new EmptyBorder(5, 5, 5, 5));
        panel.add(label, BorderLayout.CENTER);

        panel.addMouseListener(
            new MouseAdapter() {
                @Override
                public void mouseClicked(MouseEvent e) {
                    showMissionDialog(mission, index);
                }
            }
        );

        return panel;
    }

    private void showMissionDialog(SpaceMission mission, int index) {
        try {
            JOptionPane.showMessageDialog(
                this,
                "Объект #" +
                    index +
                    "\nТип: " +
                    mission.getClass().getSimpleName() +
                    "\nНазвание: " +
                    mission.getMissionName() +
                    "\nДанные: " +
                    Arrays.toString(mission.getResourceData()) +
                    "\nРезультат calculateNetProfit(): " +
                    mission.calculateNetProfit(),
                "Детали объекта",
                JOptionPane.INFORMATION_MESSAGE
            );
        } catch (MissionBusinessException e) {
            JOptionPane.showMessageDialog(
                this,
                "Ошибка вычисления бизнес-метода: " + e.getMessage(),
                "Ошибка",
                JOptionPane.ERROR_MESSAGE
            );
        }
    }

    private void changeLookAndFeel(UIManager.LookAndFeelInfo info) {
        try {
            UIManager.setLookAndFeel(info.getClassName());
            SwingUtilities.updateComponentTreeUI(this);
        } catch (Exception ex) {
            JOptionPane.showMessageDialog(
                this,
                "Не удалось применить стиль",
                "Ошибка",
                JOptionPane.ERROR_MESSAGE
            );
        }
    }

    public static void main(String[] args) {
        SwingUtilities.invokeLater(MissionDatabaseGUI::new);
    }
}
